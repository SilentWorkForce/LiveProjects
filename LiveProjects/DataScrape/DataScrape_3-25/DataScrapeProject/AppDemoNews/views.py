from django.shortcuts import render, get_object_or_404
from django.http import HttpRequest, HttpResponse
from .models import NewsScraper

import urllib.request

# Create your views here.


def news_data(request):

    news = NewsScraper()
    return render(request, 'AppDemoNews/news_data.html', {'news': news})
