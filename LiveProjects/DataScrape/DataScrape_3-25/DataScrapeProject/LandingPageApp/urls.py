from django.urls import path
from . import views
from django.views.generic.base import TemplateView

urlpatterns = [
    path('', TemplateView.as_view(template_name='home.html'), name='home'),
    path('get_weather_from_ip/', views.get_weather_from_ip, name='get_weather_from_ip') #Adds path to weather function when user is on landing page
]