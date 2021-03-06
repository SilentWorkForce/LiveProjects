# Generated by Django 2.1.5 on 2019-05-22 22:02

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('PodcastsApp', '0004_auto_20190522_1211'),
    ]

    operations = [
        migrations.AlterField(
            model_name='podcast',
            name='title',
            field=models.CharField(choices=[('Sports', 'Sports'), ('Pop Culture, TV & Film', 'Pop Culture, TV & Film'), ('News & Politics', 'News & Politics'), ('Education', 'Education'), ('Business', 'Business'), ('Kids & Family', 'Kids & Family'), ('Lifestyle & Health', 'Lifestyle & Health'), ('Technology', 'Technology'), ('Music & Commentary', 'Music & Commentary'), ('Storytelling', 'Storytelling'), ('Religion & Spirituality', 'Religion & Spirituality'), ('Society & Culture', 'Society & Culture'), ('Games & Hobbies', 'Games & Hobbies'), ('Everything', 'Everything'), ('Science & Medicine', 'Science & Medicine'), ('Comedy', 'Comedy')], max_length=60),
        ),
    ]
