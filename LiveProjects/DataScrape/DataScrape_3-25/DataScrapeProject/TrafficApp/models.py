from django.db import models

# Create your models here.
class Route(models.Model):
    start = models.CharField(max_length= 75)
    end = models.CharField(max_length= 75)