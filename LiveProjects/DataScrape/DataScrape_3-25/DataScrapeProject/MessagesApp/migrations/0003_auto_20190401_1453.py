# Generated by Django 2.1.7 on 2019-04-01 21:53

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('MessagesApp', '0002_conversation'),
    ]

    operations = [
        migrations.AlterField(
            model_name='conversation',
            name='message',
            field=models.ForeignKey(default=1, on_delete=django.db.models.deletion.SET_DEFAULT, related_name='message', to='MessagesApp.Message'),
        ),
    ]
