from django.forms import ModelForm, TextInput
from .models import Search

class SearchForm(ModelForm):
    class Meta:
        model = Search
        fields = ['destination']
        labels = {
            'destination' : 'Your Destination'
        }
