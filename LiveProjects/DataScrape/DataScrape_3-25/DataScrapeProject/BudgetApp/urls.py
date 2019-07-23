from django.urls import path
from . import views


urlpatterns = [
    path("", views.budget_input, name="budget_input"),
    path("<username>", views.budget_output, name="budget_output"),
]
