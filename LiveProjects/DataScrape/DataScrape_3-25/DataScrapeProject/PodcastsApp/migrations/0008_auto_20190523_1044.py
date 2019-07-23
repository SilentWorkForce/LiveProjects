# Generated by Django 2.1.5 on 2019-05-23 17:44

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('PodcastsApp', '0007_auto_20190523_1035'),
    ]

    operations = [
        migrations.AlterField(
            model_name='podcast',
            name='title',
            field=models.CharField(choices=[('Comedy', 'Comedy'), ('News & Politics', 'News & Politics'), ('Society & Culture', 'Society & Culture'), ('Pop Culture, TV & Film', 'Pop Culture, TV & Film'), ('Kids & Family', 'Kids & Family'), ('Games & Hobbies', 'Games & Hobbies'), ('Storytelling', 'Storytelling'), ('Technology', 'Technology'), ('Business', 'Business'), ('Music & Commentary', 'Music & Commentary'), ('Lifestyle & Health', 'Lifestyle & Health'), ('Education', 'Education'), ('Everything', 'Everything'), ('Sports', 'Sports'), ('Religion & Spirituality', 'Religion & Spirituality'), ('Science & Medicine', 'Science & Medicine')], max_length=60),
        ),
    ]
