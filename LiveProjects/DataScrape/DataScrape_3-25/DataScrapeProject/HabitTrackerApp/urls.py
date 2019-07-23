from django.urls import path
from HabitTrackerApp import views

urlpatterns = [
    path('', views.HomeView.as_view(), name='habit-home'),
    path('history_view/', views.history_view, name='habit-history'),
]