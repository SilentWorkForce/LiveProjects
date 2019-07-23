from django.forms import ModelForm, TextInput
from .models import Route
from django import forms

# class RouteForm(ModelForm):
#     class Meta:
#         model = Route
#         fields = ['start', 'end']
#         labels = {
#             'start' : 'From',
#             'end' : 'To'
#         }
        
        
        


class RouteForm(ModelForm):
    class Meta:
        model = Route
        fields = ['start', 'end']
        labels = {
            'start' : 'From',
            'end' : 'To'
        }
        widgets = { 'start' : forms.TextInput(attrs={'size': 50}),
                    'end' : forms.TextInput(attrs={'size': 50})
        }
        