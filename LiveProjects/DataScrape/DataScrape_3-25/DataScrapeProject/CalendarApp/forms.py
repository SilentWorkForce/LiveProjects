from django import forms
from AccountsApp.models import UserProfile
from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.models import User
from datetime import datetime

#Create User Event form

class EventForm(forms.Form):
    event_title = forms.CharField(max_length=100,required=True)
    start_date = forms.DateField(initial=datetime.now(), required=True)
    start_time = forms.TimeField(initial=datetime.now(), required=True)
    end_date = forms.DateField(initial=datetime.now(), required=True)
    end_time = forms.TimeField(initial=datetime.now(), required=True)
