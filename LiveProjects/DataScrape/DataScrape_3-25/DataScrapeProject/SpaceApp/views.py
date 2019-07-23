from django.shortcuts import render

# Create your views here.
from nasa import apod
from django.http import HttpResponse
from django.template import loader
from django.shortcuts import render
from datetime import date
import json
import requests


# import pprint used for debugging

# OBJECT FOR APOD FEATURE:
def currentapod(request):
    # create base_url variable containing API call with API key as parameter
    base_url = "https://api.nasa.gov/planetary/apod?api_key=kK4OwaUtVghqi9A1m40BsWAueYlagAIUQFM6FYlw"
    # get apod json object based on base_url API call
    json_apod = requests.get(base_url).json()
    #assign base_url to variable url to be passed into apod.html file to later retrieve apod
    url = base_url
    #assign value of json object key explanation to variable desc to be passed into apod.html
    desc = json_apod['explanation']
    #assign value of json object key title to variable title to be passed into apod.html
    title = json_apod['title']

    #call get_launches function and assign returned result to the variable launches_data to be passed to apod.html
    launches_data = get_launches()

    # pass in title, picture URL, pic description and data for launches to context to be passed into HTML page
    context = {'title': title, 'url': url, 'desc': desc, 'launches_data': launches_data}
    return render(request, 'space/apod.html', context)


def get_launches():
    # launchlibrary API call for next 5 launches
    base_url = 'https://launchlibrary.net/1.4/launch/next/5'

    # get json onject response from API
    # result is a large nested dictionary
    json_data_launch = requests.get(base_url).json()

    # declare launches dictionary
    launches_data = {}

    # for loop to create each dictionary entry
    for i in range(0, 5):
        # go to launches key at index "i" in json object and retrieve value associated with the key "name".
        json_name = json_data_launch['launches'][i]['name']
        # go to launches key at index "i" in json object, then go to key "location", then go to key "pads" and at value index 0, retrieve value associated with the key "name".
        json_location = json_data_launch['launches'][i]['location']['pads'][0]['name']
        # go to launches key at index "i" in json object and retrieve value associated with the key "windowstart".
        json_windowstart = json_data_launch['launches'][i]['windowstart']
        # go to launches key at index "i" in json object and retrieve value associated with the key "windowend".
        json_windowend = json_data_launch['launches'][i]['windowend']
        # go to launches key at index "i" in json object, then go to key "lsp", and retrieve value associated with the key "name".
        json_agency_name = json_data_launch['launches'][i]['lsp']['name']

        # create dictionary "launch_data" for each individual launch and give each key its value retrieved from the json object.
        launch_data = {"name": json_name, "agency": json_agency_name, "location": json_location,
                       "windowstart": json_windowstart, "windowend": json_windowend}
        # add each "launch_data" dictionary created as a value at key "i" to the dictionary "launches_data"
        launches_data[i] = launch_data

    # returns launces_data dictionary
    return launches_data

