from django.shortcuts import render
from django.http import HttpRequest, HttpResponse
import requests
import json
from .forms import RestaurantSearchForm
from .models import RestaurantSearch
import urllib.request

# Create your views here.


def restaurant(request):
    if request.method == 'GET':
        form = RestaurantSearchForm(initial = {'Postal': 'ex: 90210'})
        context = {'form' : form}
        return render(request, 'RestaurantApp/restaurant.html', context)

    elif request.method == 'POST':
        form = RestaurantSearchForm(request.POST)
        form.save()

        #zipcode = RestaurantSearch.objects.filter().order_by('id')[0]  - Old code returning results in France
        zipcode = form.cleaned_data

        api_key = '9qkoCsNVwwit9mGvJw3UMIkK5NcsRqPIzIMmhPPrSY7MnxpzMNY5zDLTFF0d8Vzo8xx3xeWuf0pzB1Gu_SW2DGxnXMTBnrKlFPoAV5PbHhgU6z4JVTcZ1DJg977ZXHYx'
        headers = {'Authorization': 'Bearer %s' % api_key}

        url = 'https://api.yelp.com/v3/businesses/search'
        params = {'categories':'restaurants','location': str(zipcode),'limit':'5','sort_by':'rating'}

        req = requests.get(url, params=params, headers=headers)

        parsed = json.loads(req.text)
        
        businesses = parsed['businesses']


        context = {'businesses' : businesses, 'form' : form}
        return render(request, 'RestaurantApp/restaurant.html', context)


