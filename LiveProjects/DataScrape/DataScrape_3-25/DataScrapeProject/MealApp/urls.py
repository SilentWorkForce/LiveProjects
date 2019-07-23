from django.urls import path

from . import views

urlpatterns = [
    path('', views.mealsearch, name='search'),
    path('recipe/<id>/', views.recipe, name='recipe'),
]
