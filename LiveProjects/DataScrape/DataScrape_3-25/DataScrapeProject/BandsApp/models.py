from django.db import models
from bs4 import BeautifulSoup
from django.shortcuts import get_object_or_404
import datetime
import requests


class Bands(models.Model):
    date = models.DateField(default=datetime.date.today)
    show = models.CharField(max_length = 100)

class ShowGuide:

    def __init__(self):

        content = requests.get('https://pc-pdx.com/show-guide/').content
        soup = BeautifulSoup(content, 'html5lib') #Passes the html page through the BeautifulSoup parser and uses lenient parser due to poor source code.

        #get all the data in the show-listing class. This includes:
        #class_=bands slider-spot --> lists the artists performing at specific show listing
        #class_=venues slider-spot --> lists the venue for specific show listing
        #class_=detail --> lists the show time and price of the show listing
        showDetails = soup.select("div[class*=show-listing]")
        
        artistList = [] #Create empty list to store the artist list from the for loop.
        venueList = [] #Create empty list to store the venue list from the for loop.
        detailsList = [] #Create empty list to store the detail list from the for loop.
        flyerList = []

        #Loop through showDetails(class=show-listing) for every occurance indicating a unique show listing.
        for showDetail in showDetails:
            artistList.append(showDetail.find(class_='bands slider-spot').get_text().replace('\n',''))
            venueList.append(showDetail.find(class_='venues slider-spot').get_text().replace('\n',''))
            detailsList.append(showDetail.find(class_='detail').get_text().replace('\n',''))
        for a in soup.find_all('div', class_='show-listing'):
                flyerList.append(a.img['src'])
            


        #create list of artist, venue, and details list by show (ith element)    
        all_list = zip(artistList, venueList, detailsList, flyerList)
        self.all_list = all_list
        #reference all_list in bands.html to loop through each show

        

  