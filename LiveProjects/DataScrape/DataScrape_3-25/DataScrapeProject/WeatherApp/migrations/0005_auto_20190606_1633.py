# Generated by Django 2.1.5 on 2019-06-06 16:33

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('WeatherApp', '0004_auto_20190605_2316'),
    ]

    operations = [
        migrations.CreateModel(
            name='Location',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('city', models.CharField(max_length=50)),
                ('zip', models.CharField(max_length=5)),
            ],
        ),
        migrations.DeleteModel(
            name='City',
        ),
        migrations.DeleteModel(
            name='Weather',
        ),
        migrations.DeleteModel(
            name='Zip',
        ),
    ]
