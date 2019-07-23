from django.db import models
import csv

# Create your models here.
class Search(models.Model):
    destination = models.CharField(max_length = 50)
    
