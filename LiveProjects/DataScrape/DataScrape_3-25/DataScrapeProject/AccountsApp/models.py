from django.db import models
from django.contrib.auth.models import User
from django.dispatch import receiver
from django.db.models.signals import post_save
from django.conf import settings



# Create your models here.

#User Profile Model (inclusing Profile Image column, 2 other fields were creating for test purposes)
class UserProfile(models.Model):
    user = models.OneToOneField(User, on_delete=models.CASCADE)
    lng = models.DecimalField(max_digits=9, decimal_places=4, default=45.5155)
    lat = models.DecimalField(max_digits=9, decimal_places=4, default=122.6793)
    user_tracking = models.BooleanField(default=False)
    bio = models.TextField(max_length=500, blank=True)
    city = models.CharField(max_length=30, blank=True)
    image = models.ImageField(upload_to='profile_image',null=True, blank=True)

#Create UserProfile when user signs up
@receiver(post_save, sender=User)
def create_user_profile(sender, instance, created, **kwargs):
    if created:
        UserProfile.objects.create(user=instance)

@receiver(post_save, sender=User)
def save_user_profile(sender, instance, **kwargs):
    instance.userprofile.save()

