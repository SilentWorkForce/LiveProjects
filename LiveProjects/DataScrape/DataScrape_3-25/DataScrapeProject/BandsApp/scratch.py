from bs4 import BeautifulSoup #to parse the html text
import requests #grab the html page itself (or you could use urllib - they accomplish the same thing)
#from .models import ShowGuide
import re

response = requests.get('https://pc-pdx.com/show-guide/?date=4/13/2019') #https://pc-pdx.com/show-guide/?date=4/3/2019 (this is the date where my code works)
soup = BeautifulSoup(response.text, 'html.parser') #Passes the html page through the BeautifulSoup parser and indicates that it is an html page
showDetails = soup.find_all(class_='show-listing') #Get all the data in the show-listing class. 
                                                      #class* is a wildcard selector that will select any classes that include "show-listing" 
print(showDetails)
artistDetails = soup.find_all(class_='bands slider-spot')
print(artistDetails)
venueDetails = soup.find_all(class_='venues slider-spot')
print(venueDetails)
detailsDetails = soup.find_all(class_='detail')
print(detailsDetails)
flyerDetails = soup.find_all(class_='flyer')
print(flyerDetails)

showsList = [] #Create empty list to store the shows list from the for loop.




for showDetail in showDetails: 
    artist = showDetail.find(class_='bands slider-spot').get_text().replace('\n','')  
    print(artist)
    venue = showDetail.find(class_='venues slider-spot').get_text().replace('\n','')
    print(venue)
    details = showDetail.find(class_='detail').get_text().replace('\n','')
    print(details)
    flyer = showDetail.find(class_='flyer').get_text().replace('\n','')
    print(flyer)
    
    shows = [
       {
            'artists': artist,
            'venue': venue,
            'timeCost': details,
            'flyer': flyer
        }
    ]
    showsList.extend(shows)
print(showsList)