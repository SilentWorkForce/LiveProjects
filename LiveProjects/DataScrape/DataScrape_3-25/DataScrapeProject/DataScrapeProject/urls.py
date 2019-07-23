"""DataScrapeProject URL Configuration

The `urlpatterns` list routes URLs to views. For more information please see:
    https://docs.djangoproject.com/en/2.1/topics/http/urls/
Examples:
Function views
    1. Add an import:  from my_app import views
    2. Add a URL to urlpatterns:  path('', views.home, name='home')
Class-based views
    1. Add an import:  from other_app.views import Home
    2. Add a URL to urlpatterns:  path('', Home.as_view(), name='home')
Including another URLconf
    1. Import the include() function: from django.urls import include, path
    2. Add a URL to urlpatterns:  path('blog/', include('blog.urls'))
"""
from django.contrib.staticfiles.urls import staticfiles_urlpatterns
from django.contrib import admin
from AppDemoNews import views
from django.urls import path, include #added an include, fixed server issue. -MJ
from django.views.generic.base import TemplateView #used to make templates viewable - MJ
from django.conf import settings
from django.conf.urls.static import static
from AccountsApp.views import ProfileView #NZ
from CalendarApp.views import EventCalendar #import EventCalendar function from CalendarApp.views
from CalendarApp.views import UpdateCalendar #import UpdateCalendar function from CalendarApp.views
from django.contrib.auth import views as auth_views #this is for the password reset views
from SpaceApp import views

urlpatterns = [
    path('admin/', admin.site.urls),
    path('accounts/', include('AccountsApp.urls')), # path for signing up
    path('accounts/', include('django.contrib.auth.urls')), #Path for login - MJ
    path('home/', TemplateView.as_view(template_name='home.html'), name='home'),  # Path for homepage
    path('', include('LandingPageApp.urls')), # Path to homepage
    path('chat/', include('MessagesApp.urls')), # Path to message
	path('movie/', include('DataApp.urls')), # Path to top five movies
    path('weather/', include('WeatherApp.urls')), # Path to Weather
    path('traffic/', include('TrafficApp.urls')), # Path to Traffic 
    path('news/', include('AppDemoNews.urls')),
    path('message', include('MessagesApp.urls')), # Path to message
    path('profile/', ProfileView.as_view(), name='profile'),
    path('space/', include('SpaceApp.urls')), # Path to Space
    path('bands/', include('BandsApp.urls')), #Path to bandsApp -KW
    path('podcast/', include('PodcastsApp.urls')), # Path to top 5 stitcher podcasts
    path('news/', include('AppDemoNews.urls')),
    path('events-calendar/', EventCalendar.as_view(), name='events-calendar'), # Call function EventCalendar and open corresponding html page as view
    path('events-calendar/update-calendar/', UpdateCalendar.as_view(), name='update-calendar'),# Call function UpdateCalendar and open corresponding html page as view
    path('events-calendar/update-calendar/', UpdateCalendar.as_view(), name='update-calendar'),# Call function UpdateCalendar and open corresponding html page as view
    path('forsale/', include('CraigslistApp.urls')), #Path to Craigslist search App
    path('habit-tracker/', include('HabitTrackerApp.urls')),
    path('budget/', include('BudgetApp.urls')),
    path('profile/', include('AccountsApp.urls')),
    path('toprestaurants/', include('RestaurantApp.urls')),
    path('meals/', include('MealApp.urls')),
]+static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)

urlpatterns += staticfiles_urlpatterns()
