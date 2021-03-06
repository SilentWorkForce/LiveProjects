# Generated by Django 2.1.5 on 2019-05-22 23:22

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('PodcastsApp', '0005_auto_20190522_1502'),
    ]

    operations = [
        migrations.AlterField(
            model_name='podcast',
            name='title',
            field=models.CharField(choices=[('Pop Culture, TV & Film', 'Pop Culture, TV & Film'), ('Science & Medicine', 'Science & Medicine'), ('Storytelling', 'Storytelling'), ('Society & Culture', 'Society & Culture'), ('News & Politics', 'News & Politics'), ('Everything', 'Everything'), ('Lifestyle & Health', 'Lifestyle & Health'), ('Technology', 'Technology'), ('Religion & Spirituality', 'Religion & Spirituality'), ('Sports', 'Sports'), ('Kids & Family', 'Kids & Family'), ('Business', 'Business'), ('Games & Hobbies', 'Games & Hobbies'), ('Comedy', 'Comedy'), ('Music & Commentary', 'Music & Commentary'), ('Education', 'Education')], max_length=60),
        ),
    ]
