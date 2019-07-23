# Generated by Django 2.1.5 on 2019-05-02 03:46

from django.db import migrations, models
import django.db.models.deletion
import django.db.models.manager


class Migration(migrations.Migration):

    dependencies = [
        ('AccountsApp', '0003_auto_20190414_1625'),
        ('HabitTrackerApp', '0003_auto_20190501_1523'),
    ]

    operations = [
        migrations.CreateModel(
            name='Habits',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('exercise', models.IntegerField(default=0, null=True)),
                ('sleep', models.IntegerField(default=0, null=True)),
                ('mood', models.IntegerField(choices=[(1, '1'), (2, '2'), (3, '3'), (4, '4'), (5, '5'), (6, '6'), (7, '7'), (8, '8'), (9, '9'), (10, '10')], null=True)),
                ('energy', models.IntegerField(choices=[(1, '1'), (2, '2'), (3, '3'), (4, '4'), (5, '5')], null=True)),
                ('meditate', models.BooleanField(default=False)),
                ('user', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='AccountsApp.UserProfile')),
            ],
            managers=[
                ('existing', django.db.models.manager.Manager()),
            ],
        ),
        migrations.DeleteModel(
            name='UserHabits',
        ),
    ]
