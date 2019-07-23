from django.db import models

# Create your models here.


class RestaurantSearch(models.Model):
    postal = models.CharField(max_length= 5)