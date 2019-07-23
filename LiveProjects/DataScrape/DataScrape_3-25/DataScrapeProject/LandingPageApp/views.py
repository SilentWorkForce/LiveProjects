from django.contrib.auth.models import User
from AccountsApp.models import UserProfile
from django.shortcuts import render, render_to_response
from django.http import HttpResponse
from django.template import loader
from django.http import JsonResponse
import requests
# Create your views here.


# Pings ip-api.com with IP address for location
def get_location_from_ip(ip_address):
    response = requests.get("http://ip-api.com/json/{}".format(ip_address))
    print(response)
    return response.json()

# Pulls weather data from openweathermap.org for zip code associated with IP Address
def get_weather_from_location(zip_code):
    url = 'https://api.openweathermap.org/data/2.5/weather?zip={}&units=imperial&appid=717773b8d51cee768b8ceb819ad9aeb3'.format(zip_code)
    response = requests.get(url)
    return response.json()

# Compiles weather data to send to HTML doc
def get_weather_from_ip(request):
    ip_address = request.GET.get("ip")
    location = get_location_from_ip(ip_address)
    city = location.get("city")
    zip_code = location.get("zip")
    weather_data = get_weather_from_location(zip_code)
    description = weather_data['weather'][0]['description']
    temperature = weather_data['main']['temp']
    icon = weather_data['weather'] [0] ['icon']
    iconImage = "http://openweathermap.org/img/w/{}.png".format(icon)
    s = "Current Weather for {} zip code {} {} {}".format(city, zip_code, description, temperature)
    data = {"weather_data": s, "icon": iconImage}
    return JsonResponse(data)