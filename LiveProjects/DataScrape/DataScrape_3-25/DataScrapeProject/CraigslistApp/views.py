''' This app allows the user to search portland.craigslist.org For Sale section. Improvements could
be made to select another region or city. As well, BeautifulSoup only scrapes the first 120 results given 
so it could help to use Selenium or Requests to click through all available pages of results.'''

from django.shortcuts import render, get_object_or_404
from django.http import HttpRequest, HttpResponse
from bs4 import BeautifulSoup
from .forms import SearchForm
from .models import Search
import requests, re
import urllib.request

def craigslistsearch(request):
    # Define website URL with empty spaces for input variables
    url = "https://portland.craigslist.org/search/sss?query={}&search_distance={}&postal={}"

    # Pulls up blank SearchForm when user visits page
    if request.method == 'GET':
        form = SearchForm(initial = {'keyword': 'ex: Trampoline'})
        context = {'form' : form}
        return render(request, 'CraigslistApp/craigslist.html', context)

    # Once form is filled out and "Search" button is pressed, form data is saved
    elif request.method == 'POST':
        form = SearchForm(request.POST)
        form.save()
    
        search = Search.objects.filter().order_by('-id')[0] # Gets latest data from Search model to use in building new URL

        response = requests.get(url.format(search.keyword, search.radius, search.postal)) # Formats Search model data into new URL and gets HTML info

        data = response.text # Returns the text from Request.get(url)

        soup = BeautifulSoup(data, 'lxml')

        results = soup.find_all('li', class_='result-row') # As of April 2019, Craigslist gives each unique search entry class of 'result-row'. This collects them from the HTML using Beautiful Soup

        links = [] # Empty list to store our search results as the useful text
        hrefs = [] # Empty list to store search results href link
        images = [] # Empty list to store image source results from the search

        # Loop over each result-row item to pull out our useful text and hrefs                    
        for result in results:
            title = result.find('a', class_= 'result-title hdrlnk')
            title = title.get_text()
            price = result.find('span', class_='result-price')
            price = price.get_text()
            boro = result.find('span', class_= 'result-hood')
            # Sometimes the neighborhood  or 'result-hood' class is empty so this eliminates the error if data-type is None
            if boro is not None:
                boro = boro.get_text()
            else:
                boro = str()
            link = title + " " + price + " " + boro # Combines all useful text into a single string
            href = result.find('a', class_= 'result-title hdrlnk')
            href = href["href"] # Pull out the actual link text from the href
            links.append(link) # Populates the links to their coresponding list
            hrefs.append(href) # Populates the hrefs to their coresponding list

        # Loop over the gallery images from results and pull out first image sources -- This does not include an exception if the result has no image.
        for image in soup.find_all('a', class_='result-image gallery'):
            galleryString = (image['data-ids']) # Within the result <a> tag, it stores image IDs from the result gallery. This pulls the gallery ID element out and saves it to a string
            startID = galleryString.find('1:') + 2 # To pull out only the main image ID from the gallery IDs, we give it a start and endpoint
            endID = galleryString.find(',', startID)
            imageCode = galleryString[startID:endID] 
            src = "https://images.craigslist.org/%s_300x300.jpg" % imageCode # Once the main image ID is extracted, we put it into the url structure template that creates the image source
            images.append(src) # Populates the image sources to their coresponding list
                                  
        sourceLists = zip(links, hrefs, images) # Combines our three lists into a tuple     
        context = {'sourceLists' : sourceLists, 'form' : form} # Puts the new combined list and SearchForm to render back to our HTML template 
        return render(request, 'CraigslistApp/craigslist.html', context)