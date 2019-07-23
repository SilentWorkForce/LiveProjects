import json
import urllib

from django.shortcuts import render
from .forms import CurrencyForm


# Create your views here.

# fixer.io API key : 2279dc03455788be16c0da66470aea36 limit of 1000 uses/month and base currency of EUR
def currency(request):
    url = 'http://data.fixer.io/api/latest?access_key=2279dc03455788be16c0da66470aea36'

    if request.method == "POST":
        form = CurrencyForm(request.POST)
        if form.is_valid():
            j = json.loads(urllib.request.urlopen(url).read())
            target = form.cleaned_data['target_currency']
            base = form.cleaned_data['base_currency']
            dollars = form.cleaned_data['money'] # Dollars, our base currency
            euros = dollars / j["rates"][base] # Convert to Euros, the API's base currency
            c = j["rates"][target]
            original = '{:,.2f}'.format(dollars)
            amount = '{:,.2f}'.format(round(euros * c, 2)) #Format to display money
            conversion = {
                'original' : original,
                'amount' : amount,
                'currency' : target,
                'base' : base,
                'rates' : j["rates"]
            }
            
            context = {'conversion' : conversion}
            return render(request, 'currency/currency_conversion.html', context)
    else:
        form = CurrencyForm(request.POST)
        return render(request, 'currency/currency.html', {'form' : form})