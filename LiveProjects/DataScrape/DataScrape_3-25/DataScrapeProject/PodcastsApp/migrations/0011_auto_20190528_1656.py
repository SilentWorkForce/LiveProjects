# Generated by Django 2.1.5 on 2019-05-28 23:56

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('PodcastsApp', '0010_auto_20190528_1447'),
    ]

    operations = [
        migrations.AlterField(
            model_name='podcast',
            name='title',
            field=models.CharField(max_length=6000),
        ),
    ]
