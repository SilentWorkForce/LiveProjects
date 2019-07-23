from django.urls import path # copied from BandsApp urls
from . import views 


# copied from BandsApp urls
# route to currentapod HTML file 
urlpatterns = [
    path('', views.currentapod, name='currentapod'),
]