# Generated by Django 2.1.5 on 2019-04-29 16:59

from django.db import migrations
import django.db.models.manager


class Migration(migrations.Migration):

    dependencies = [
        ('BudgetApp', '0004_auto_20190425_0209'),
    ]

    operations = [
        migrations.AlterModelManagers(
            name='budget',
            managers=[
                ('existing', django.db.models.manager.Manager()),
            ],
        ),
    ]
