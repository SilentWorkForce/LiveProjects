
from bs4 import BeautifulSoup as bs
from django.shortcuts import get_object_or_404
import requests

# Create your models here.


class NewsScraper:

    def __init__(self):

        content = requests.get('https://www.theverge.com/features').content
        soup = bs(content, 'html.parser')
        # overall div class for articles
        articles = soup.find_all('div', class_='c-compact-river__entry')
        title_list = []  # list to iterate in below for loop.
        url_list = []
        img_list = []
        index_list = []

        for index in range(0, 6):  # for top 6 articles list the following

            title_list.append(articles[index].h2.get_text())
            url_list.append(articles[index].a['href'])
            img_list.append(articles[index].noscript.img['src'])
            index_list.append(index)

        # group iterations through zip function
        all_list = zip(title_list, url_list, img_list, index_list)
        self.all_list = all_list


