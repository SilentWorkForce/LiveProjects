from django.urls import path
from . import views

urlpatterns = [
    path('', views.get_top10, name='podcast')
]
