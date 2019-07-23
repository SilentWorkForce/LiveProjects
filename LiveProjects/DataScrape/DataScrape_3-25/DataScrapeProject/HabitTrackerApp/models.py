import datetime
from django.db import models
from django.conf import settings
from AccountsApp.models import UserProfile

class Habits(models.Model):
    MOOD_CHOICES = ((1, '1'), (2, '2'), (3, '3'), (4, '4'), (5, '5'), (6, '6'), (7, '7'), (8, '8'), (9, '9'), (10, '10')) # Limits answer to a scale of 1-10
    ENERGY_CHOICES = ((1, '1'), (2, '2'), (3, '3'), (4, '4'), (5, '5')) # Limits answer to a scale of 1-5

    user = models.ForeignKey(on_delete=models.CASCADE, to=settings.AUTH_USER_MODEL) # Associates the user with their entries
    date = models.DateField(default=datetime.date.today)
    exercise = models.IntegerField(default=0, null=True)
    sleep = models.IntegerField(default=0, null=True)
    mood = models.IntegerField(null=True, choices=MOOD_CHOICES) 
    energy = models.IntegerField(null=True, choices=ENERGY_CHOICES) 
    meditate = models.BooleanField(default=False)