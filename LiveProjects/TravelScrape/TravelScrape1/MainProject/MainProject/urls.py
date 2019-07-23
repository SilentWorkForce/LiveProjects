"""MainProject URL Configuration

The `urlpatterns` list routes URLs to views. For more information please see:
    https://docs.djangoproject.com/en/2.2/topics/http/urls/
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
from django.contrib import admin
from django.urls import path, include
from ToDoListApp import views
from django.views.generic.base import TemplateView
from DashboardApp import views


urlpatterns = [
    path('', TemplateView.as_view(template_name='DashboardApp/dashboard.html'),name='dashboard'),
    path('budget/', include('budget.urls')),
    path('account/', include('django.contrib.auth.urls')),
    path('admin/', admin.site.urls),
    path('ToDoListApp/', include('ToDoListApp.urls')),
    path('dummy/', include('dummy.urls')),
    path('currency/', include('CurrencyApp.urls')),
    path('advisory/', include('AdvisoryApp.urls')),
    path('AccountsApp/', include('AccountsApp.urls')),
    path('hotels/', include('HotelApp.urls')),
    path('DashboardApp/', include('DashboardApp.urls')),
]
