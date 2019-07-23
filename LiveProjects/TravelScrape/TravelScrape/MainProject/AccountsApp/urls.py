
from django.urls import path
from django.conf.urls import url
from . import views
from django.contrib import admin
from django.urls import path, include 

urlpatterns = [
    path('', views.dashboard),
    path('login/', views.login),

]

