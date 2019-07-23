# Generated by Django 2.1.5 on 2019-05-16 21:15

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('DataApp', '0005_auto_20190516_1124'),
    ]

    operations = [
        migrations.RenameField(
            model_name='moviepreferences',
            old_name='sort_type',
            new_name='year_sort',
        ),
        migrations.AddField(
            model_name='moviepreferences',
            name='genre_sort',
            field=models.CharField(default='all', max_length=20, null=True),
        ),
    ]
