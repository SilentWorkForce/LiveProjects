from django.forms import ModelForm, TextInput
from .models import RestaurantSearch

class RestaurantSearchForm(ModelForm):
    class Meta:
        model = RestaurantSearch
        fields = ['postal']
        labels = {
            'postal': 'Zip Code'
        }
        