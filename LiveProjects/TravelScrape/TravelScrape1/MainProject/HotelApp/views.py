from django.shortcuts import render, get_object_or_404
from django.http import HttpRequest, HttpResponse
from .models import Search
from .forms import SearchForm
from amadeus import Client, Response
import requests 
import json
from bs4 import BeautifulSoup
import csv
import lxml
from .app_logic import findMatches

# Create your views here.


def hotelIndex(request):
    # Creates blank form on initial page load
    if request.method == 'GET':
        form = SearchForm(initial = "")
        context = {'form' : form}
        return render(request, "HotelApp\index.html", context)
            
    elif request.method =='POST':
        form = SearchForm(request.POST)
        form.save()

        search = Search.objects.filter().order_by('-id')[0]
        cities = findMatches(search.destination)
        context = {'cities' : cities, 'form': form}
        return render(request, "HotelApp\index.html", context)


def hotelSearch(request, IATA):
    form = SearchForm(initial = "")
   #Initialize Amadeus
    amadeus = Client(
        client_id = "oXoHcPGNhQAKNcvvhFIkB9kFudwrBYTy",
        client_secret = "zs3PhgM4HNpZbCm4"
    )
    response = amadeus.shopping.hotel_offers.get(cityCode = IATA)
    with open("log.json", "w+", encoding="utf-8") as log:
        json.dump(response.data, log, indent=2)
    result = response.data
    context = {'form' : form, 'results' : result}
    return render(request, "HotelApp\index.html", context)






