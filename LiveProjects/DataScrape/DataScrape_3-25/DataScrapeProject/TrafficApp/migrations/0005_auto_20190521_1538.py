# Generated by Django 2.1.5 on 2019-05-21 22:38

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('TrafficApp', '0004_auto_20190513_1552'),
    ]

    operations = [
        migrations.AlterField(
            model_name='route',
            name='end',
            field=models.CharField(max_length=75),
        ),
        migrations.AlterField(
            model_name='route',
            name='start',
            field=models.CharField(max_length=75),
        ),
    ]
