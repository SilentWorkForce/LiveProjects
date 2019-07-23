from django.urls import path
from . import views


urlpatterns = [
    path('', views.advisory, name='advisory'),
]