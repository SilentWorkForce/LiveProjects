from django.shortcuts import render
from django.db import models
from DataApp.models import Movie
from DataApp.models import MoviePreferences
from django.contrib.auth.models import User
from django.contrib import auth
from AccountsApp.models import UserProfile
import datetime
import requests
import urllib.request
import urllib
import urllib.parse
from w3lib.url import safe_url_string
import json
import re
from bs4 import BeautifulSoup
import requests
import re
from django.db.models import Q


# Download IMDB's Top 250 data
url = 'https://www.imdb.com/chart/moviemeter?pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=4da9d9a5-d299-43f2-9c53-f0efa18182cd&pf_rd_r=YYMKC2RG21YYXR2YZZRH&pf_rd_s=right-4&pf_rd_t=15506&pf_rd_i=boxoffice&ref_=chtbo_ql_2'
response = requests.get(url)
soup = BeautifulSoup(response.text, 'lxml')
#Get all necessary information from the IMDb webpage (title, link, cast)
movies = soup.select('td.titleColumn')
#creating list to store movie titles
#print(response)

#Handle GET and POST requests
def topfive(request):
   
    #Object representing the current user's row in DB
	try:
		db = MoviePreferences.objects.get(user_id = request.user.id)
	except: #If user does not have a row in the DB, create one
		dbCreate = MoviePreferences.objects.create(user_id = request.user.id)
		dbCreate.save()
		db = MoviePreferences.objects.get(user_id = request.user.id)
	
	if request.method == "GET":
		return get_movies(request, db.display, db.year_sort, db.genre_sort)
	elif request.method == "POST":
		#If different number of movies to be displayed was changed
		if request.POST.get('displayNumber', None) != None:
			custom_display = int(request.POST.get('displayNumber', None), 10)
			db.display = custom_display #Save value to display column
			db.save()
			return get_movies(request, custom_display, db.year_sort, db.genre_sort)
		#If a year sort was requested
		elif request.POST.get('year-sort', None) != None:
			db.year_sort = int(request.POST.get('year-sort', None), 10)
			db.save()
			return get_movies(request, db.display, db.year_sort, db.genre_sort)
		#If a genre sort was requested
		else:
			if request.POST.get('sort-type', None) != None:
				genreSort = request.POST.get('sort-type', None)
				db.genre_sort = genreSort
				db.save()
				return get_movies(request, db.display, db.year_sort, genreSort)
				
#Fetches the movies
def get_movies(request, displayNum, yearSort, genreSort):
	all_movies = []
	api_beginning = "http://www.omdbapi.com/?apikey=360766ac&t="
	# The following loop will run once for each movie title fetched from the IMDB, until
	# the desired number of movies is fetched, or until the IMDB list is exausted
	counter = 0
	while len(all_movies) < displayNum and counter < 100:
		# get title
		movie_string = movies[counter].get_text()
		movie_title = movie_string.split("(")[0] #Get just movie title	
		
		fixUrlParam(movie_title)

		movie_title_parameter = "".join(movie_title) #Convert back to string (unicode)
		fullURL = api_beginning + movie_title_parameter #Create full URL
		fullURL = fullURL.encode('utf-8') #Converts the string into bytes using utf-8 encoding
		fullURL = safe_url_string(fullURL) #Pass this byte list into w3lib's safe url method, and fix illegal characters	
		
		print(fullURL)

		cur_json = urllib.request.urlopen(fullURL).read() # Fetch a JSON object from API
		cur_json = json.loads(cur_json) #Decode object into dictionary
		
		#Try this next section if there are no errors in reading the JSON,
		# i.e., the movie does not exist in the DB, etc.
		try:
			#Save the movie's details
			title = cur_json['Title']
			year = cur_json['Year']
			runtime = cur_json['Runtime']
			director = cur_json['Director']
			genre = cur_json['Genre']
			plot = cur_json['Plot']
			image = cur_json['Poster']
			index_place = counter
			
			#Year variable must be an int in order for the year sort logic to work. The try/catch statement
			# covers the case where JSON returns a non-int value 
			try:
				year = int(year)
			except:
				if "-" in year:
					year = int(year.split("-")[0])
				else:
					year = 0 # Dummy year of 0 to put this movie at end of list
			
			# If movie's genre matches genre selection, all good
			if genreSort in genre or genreSort == "All":
				pass
			# If non, break the loop and begin a new iteration
			else:
				counter += 1
				continue
			
			# Create instance of Movie class and save the previous movie details
			# to the appropriate properties
			cur_movie = Movie(title, year, runtime, director, plot, genre, image, index_place)

			#Sort by year if this is user's preference
			if yearSort == 1:
				sortMovies(all_movies, cur_movie)
			# Otherwise, jsut add movie to end of movie list
			else:
				all_movies.append(cur_movie)
			
			counter += 1
		
		# Catches potential key error if JSON cannot be read
		except KeyError as e:
			print(e)
			counter += 1

	#Replace any year 0 with "N/A"
	for movie in all_movies:
		if movie.year == 0:
			movie.year = "N/A"
	# Dictionary to be passed to HTML
	context = {'imdb' : all_movies, 'sortBool': yearSort}
	
	return render(request, 'movie/movie.html', context)


# Function to remove spaces from title parameter
def fixUrlParam(movie_title):
	movie_title_chars = list(movie_title) #Turn movie title into list of chars
	movie_title_chars.remove('\n') #Remove newline characters at beginning...
	movie_title_chars.remove('\n') #...and end of title

	for charIndex,char in enumerate(movie_title_chars): #Replace spaces with "%2C"
		if char == " ":
			movie_title_chars[charIndex] = "%"
			movie_title_chars.insert(charIndex + 1, "2C")
			charIndex += 1


#Insert at sorted position function
def sortMovies(totalMovies, currentMovie):
	index = 0
	if len(totalMovies) == 0: #If first movie to be added to list, no sorting necessary
		totalMovies.append(currentMovie)					
	else: #Otherwise, insert movie at appropriate position
		while(index < len(totalMovies) and currentMovie.year < totalMovies[index].year):
			index += 1
		totalMovies.insert(index, currentMovie)