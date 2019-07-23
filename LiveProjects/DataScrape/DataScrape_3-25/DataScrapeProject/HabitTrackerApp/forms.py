from django import forms
from django.forms import ModelForm
from django.utils.translation import gettext_lazy as _
from HabitTrackerApp.models import Habits
from AccountsApp.models import UserProfile

class HabitsEntryForm(ModelForm):
    class Meta:
        model = Habits
        fields = [
            'user',
            'date',
            'exercise',
            'sleep',
            'mood',
            'energy',
            'meditate',
        ]
        labels = {
            'user': _('Current User'),
            'date': _('Entry date'),
            'exercise': _('Minutes of exercise'),
            'sleep': _('Hours of sleep'),
            'mood': _('Rate your mood today'),
            'energy': _('Current energy level'),
            'meditate': _('Check if you spent time meditating'),
        }