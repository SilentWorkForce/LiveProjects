from django.db import models

PODCAST_CATEGORIES = {
	('Comedy', 'Comedy'),
	('Society & Culture', 'Society & Culture'),
	('Science & Medicine','Science & Medicine'),
	('News & Politics', 'News & Politics'),
	('Sports', 'Sports'),
	('Pop Culture, TV & Film', 'Pop Culture, TV & Film'),
	('Business', 'Business'),
	('Education', 'Education'),
	('Kids & Family', 'Kids & Family'),
	('Pop Culture, TV & Film', 'Pop Culture, TV & Film'),
	('Games & Hobbies', 'Games & Hobbies'),
	('Storytelling', 'Storytelling'),
	('Technology', 'Technology'),
	('Music & Commentary','Music & Commentary'),
	('Everything', 'Everything'),
	('Lifestyle & Health', 'Lifestyle & Health'),
	('Religion & Spirituality', 'Religion & Spirituality'),
}


class Podcast(models.Model):
	title = models.CharField(max_length = 6000)
	#category = models.CharField(max_length = 60, choices = PODCAST_CATEGORIES)




	def __str__(self):
		return self.title