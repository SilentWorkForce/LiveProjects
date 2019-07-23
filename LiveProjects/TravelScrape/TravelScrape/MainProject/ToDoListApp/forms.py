from django import forms
from .models import *

#No List Model used anymore therefore no ListForm needed - NZ
# class ListForm(forms.Form):
#     class Meta:
#         model = List
#         fields = ["item", "completed"]

class ListForm_today(forms.ModelForm):
    class Meta:
        model = List_today
        fields = ["item", "completed"]
       
class ListForm_week(forms.ModelForm):
    class Meta:
        model = List_thisweek
        fields = ["item", "completed"]

class ListForm_month(forms.ModelForm):
    class Meta:
        model = List_thismonth
        fields = ["item", "completed"]

class ListForm_spare(forms.ModelForm):
    class Meta:
        model = List_spare_time
        fields = ["item", "completed"]

