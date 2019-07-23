# The model for message will have the following properties.  
# Charfield-Sender(max_length of 50), 
# Charfield-Recipient(max length of 50),
# Datetime-Date sent,  
# text_field-MessageBody.  
# When this model is created, a migration will need to be create and run.


from django.db import models
from django.contrib.auth.models import User
from datetime import datetime
# Create your models here.

def mydefault(context):
    return context.get_current_parameters()['id']

class Message(models.Model):
    id = models.AutoField(primary_key=True)
    sender = models.CharField(max_length=50, blank=True, null=True)
    recipient = models.CharField(max_length=50, blank=True, null=True)
    messageBody = models.TextField(blank=True, null=True)
    sent = models.DateTimeField(default=datetime.now, blank=True)
    thread = models.IntegerField(null=True)
