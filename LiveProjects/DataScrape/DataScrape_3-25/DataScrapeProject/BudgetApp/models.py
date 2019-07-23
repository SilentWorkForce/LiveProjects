from django.db import models
from AccountsApp.models import UserProfile


class Budget(models.Model):
    user = models.ForeignKey(UserProfile, models.CASCADE)
    date = models.DateField()
    actual = models.DecimalField(max_digits=8, decimal_places=2)
    budget = models.DecimalField(max_digits=8, decimal_places=2)
    existing = models.Manager()
