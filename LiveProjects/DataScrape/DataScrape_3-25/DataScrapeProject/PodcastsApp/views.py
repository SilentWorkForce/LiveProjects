import re
import requests
from django.shortcuts import render
from django.shortcuts import HttpResponse
from django.core.exceptions import *



def get_top10(request):
    url = "https://www.stitcher.com/stitcher-list/get-js.php"
    data = requests.get(url).text
    images = re.findall(r"[\n\r].*imageURL\":.\"\s*([^\n\r]*)\"", data)
    titles = re.findall(r"[\n\r].*showName\":.\"\s*([^\n\r]*)\"", data)
    details = re.findall(r"[\n\r].*seouri\":.\"\s*([^\n\r]*)\"", data)
    categories = re.findall(r"[\n\r].*category\":.\"\s*([^\n\r]*)\"", data)

    top_ten = {
        "podcasts": []
        }

    
    if request.method != 'POST':  # default page displaying top 10 podcasts
        #heading = "Current Top 10 Podcasts on Stitcher"
        for index, title in enumerate(titles[:10]):        
            top_ten["podcasts"].append({
                "images": images[index],
                "title": title,
                "categories": categories[index], 
                "details": details[index],
               # "heading": heading
                })

    elif request.method == 'POST': # displays top 10 podcasts, same as default
        search_cat = request.POST.get('cat_search', None)
        if search_cat == "top":
            for index, title in enumerate(titles[:10]):              
                top_ten["podcasts"].append({
                    "images": images[index],
                    "title": title,
                    "categories": categories[index], 
                    "details": details[index],
                
                    })    

        elif request.method == 'POST': # organizes podcasts by category
            
            search_cat = request.POST.get('cat_search', None)
            heading = " ({})".format(search_cat)
            if search_cat:
                for index, title in enumerate(titles[:100]):  
                    if search_cat == categories[index]:      
                        top_ten["podcasts"].append({
                            "images": images[index],
                            "title": titles[index],
                            "category": categories[index],
                            "details": details[index],
                            "heading": heading
                            })
        
        else:           # searches titles - not programmed yet
            search_title = request.POST.get('title_search', None)
            print(search_title)
            print(search_cat)
            print(top_ten)
            return render(request, "Podcast/podcast.html", top_ten)
            
        


            
    return render(request, "Podcast/podcast.html", top_ten,)


