from django.forms import ModelForm, TextInput
from .models import Search

class SearchForm(ModelForm):
    class Meta:
        model = Search
        fields = ['keyword', 'radius', 'postal']
        labels = {
            'keyword' : 'Search For Sale',
            'radius' : 'Miles from Zip',
            'postal': 'Zip Code'
        }