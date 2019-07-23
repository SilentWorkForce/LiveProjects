# Generated by Django 2.2.1 on 2019-06-24 18:24

from django.conf import settings
from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('AccountsApp', '0001_initial'),
    ]

    operations = [
        migrations.CreateModel(
            name='UserProfile',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('lng', models.DecimalField(decimal_places=4, default=45.5155, max_digits=9)),
                ('lat', models.DecimalField(decimal_places=4, default=122.6793, max_digits=9)),
                ('user_tracking', models.BooleanField(default=False)),
                ('bio', models.TextField(blank=True, max_length=500)),
                ('city', models.CharField(blank=True, max_length=30)),
                ('image', models.ImageField(blank=True, null=True, upload_to='profile_image')),
                ('user', models.OneToOneField(on_delete=django.db.models.deletion.CASCADE, to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.DeleteModel(
            name='Profile',
        ),
    ]
