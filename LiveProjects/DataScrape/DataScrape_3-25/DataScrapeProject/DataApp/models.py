from django.db import models


#Creating class for the movies. Just including title for now
class Movie(models.Model):

	def __init__(self, title, year, runtime, director, plot, genre, image, index):
		self.title = title
		self.year = year
		self.runtime = runtime
		self.director = director
		self.plot = plot
		self.genre = genre
		self.image = image
		self.index = index

class MoviePreferences(models.Model):
	user_id = models.IntegerField(null=True)
	display = models.IntegerField(null=True, default = 5)
	year_sort = models.IntegerField(null=True, default = 0)
	genre_sort = models.CharField(max_length=20, null=True, default = "All")