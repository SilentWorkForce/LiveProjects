from django.shortcuts import render, get_object_or_404 
from django.http import HttpResponse, HttpRequest
from .models import ShowGuide
from .models import Bands
import urllib.request

# Create your views here.

def bandsapp(request):
    showsList = ShowGuide()

    if request.method == 'POST':
        if request.POST.get('show'):
            bands=Bands()
            bands.show=request.POST.get('show')
            bands.save()

            return render(request,'BandsApp/Bands.html', {'showsList': showsList})
        else:
            return render(request,'BandsApp/Bands.html', {'showsList': showsList})

    print('entered views.py bandsapp()')
    
    
    return render(request,'BandsApp/Bands.html', {'showsList': showsList}) #will let you access this information from the template

    