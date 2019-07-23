from django.urls import path

from . import views

urlpatterns = [
    path('', views.craigslistsearch, name='search'),
]
