from django.db import models


class Search(models.Model):
    keyword = models.CharField(max_length= 50)
    radius = models.CharField(max_length= 5)
    postal = models.CharField(max_length= 5)