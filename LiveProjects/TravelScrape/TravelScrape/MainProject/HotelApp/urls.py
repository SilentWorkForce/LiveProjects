from django.urls import path
from . import views
from django.views.generic.base import TemplateView

urlpatterns = [
    path('', views.hotelIndex, name="Hotels"),
    # path(r'results/<IATA>', views.hotelSearch, name ="Hotel_Search"),
    path(r'<IATA>', views.hotelSearch, name="Hotels")
]
