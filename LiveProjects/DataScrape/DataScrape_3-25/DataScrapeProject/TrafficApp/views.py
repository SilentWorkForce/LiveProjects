from django.shortcuts import render
import requests
import json
from .forms import RouteForm
from .models import Route



def directions(request):
    url = 'https://www.mapquestapi.com/directions/v2/route?key=qGyMswGafi2puTNqSP91ETXcNRDFrAyG&from={}&to={}&outFormat=json&ambiguities=ignore&routeType=fastest&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false'

    if request.method == 'GET':
        form = RouteForm()
        context = {'form' : form}
        return render(request, 'Traffic/traffic.html', context)
        
    elif request.method == 'POST':
        form = RouteForm(request.POST)
        form.save()
       
        latest_direction = Route.objects.filter().order_by('-id')[0]
    
        r = requests.get(url.format(latest_direction.start, latest_direction.end)).json()
        
        direction_list = []

        json_list = r['route']['legs']

        for i in json_list:
            for j in i['maneuvers']:
                direction_list.append(j['narrative'])

        route = {
            'from' : latest_direction.start,
            'to' : latest_direction.end,
            'directions' : direction_list,
        }

        context = {'route' : route, 'form' : form}
        return render(request, 'Traffic/traffic.html', context)