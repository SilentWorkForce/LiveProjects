#This file was not automatically populated by startapp command

from django.urls import path #copied from DataScrapeProject/urls.py
from . import views

urlpatterns = [
    path('', views.bandsapp, name='bandsapp'),
] #also copied 