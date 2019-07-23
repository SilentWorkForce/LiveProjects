from django.urls import path
from . import views
from django.views.generic.base import TemplateView
urlpatterns = [
    path('', TemplateView.as_view(template_name='chat/navigation.html'), name='chatnavigation'),
    path('message/', views.message, name='message'),
    path('inbox/', views.inbox, name='inbox')
]