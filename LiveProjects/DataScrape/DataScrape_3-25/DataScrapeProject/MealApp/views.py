from django.shortcuts import render
from django.http import HttpRequest, HttpResponse
from .models import RecipeBasic
from .models import FullRecipe
import requests
import json
import urllib.request

# Create your views here.

def mealsearch(request):
    if(request.method == "GET"):
        return render(request, "Meal/mealsearch.html")
    else:
        searchIngredient = str(request.POST.get('search', None))
        #Replace spaces with URL safe equivalent
        searchIngredient = searchIngredient.replace(" ", "%20")
        all_meals = []
        
        #Search by main ingredient
        ingredient_URL = "https://www.themealdb.com/api/json/v1/1/filter.php?i="
        ingredient_URL += searchIngredient
        search1 = get_json(ingredient_URL)
        jsonList1 = search1['meals']
        
        if jsonList1 != None:
            #Fetch attributes for each meal in list
            for meal in jsonList1:
                id = meal['idMeal']
                title = meal['strMeal']
                image = meal['strMealThumb']

                cur_meal = RecipeBasic(id, title, image)
                all_meals.append(cur_meal)
        
        #Search by meal name
        meal_URL = "https://www.themealdb.com/api/json/v1/1/search.php?s="
        meal_URL += searchIngredient
        search1 = get_json(meal_URL)
        jsonList2 = search1['meals']

        if jsonList2 != None:
            #Go through each meal in JSON object
            for meal in jsonList2:
                counter = 0
                #Check if this object is already in the list (we don't want duplicates)
                for MEAL in all_meals:
                    if MEAL.id == meal['idMeal']:
                        break
                    counter += 1
            #If no duplicates, assign the appropriate data to a recipe instance, and add to existing list
            if counter == len(all_meals):
                id = meal['idMeal']
                title = meal['strMeal']
                image = meal['strMealThumb']

                cur_meal = RecipeBasic(id, title, image)
                all_meals.append(cur_meal)
        
        if len(all_meals) == 0:
            resultBool = False
        else:
            resultBool = True
        #Return list to HTML
        context = {'meals': all_meals, 'results': resultBool, 'category': searchIngredient}
        return render(request, "Meal/mealsearch.html", context)

def recipe(request, id):
    #Create URL for API
    meal_URL = "https://www.themealdb.com/api/json/v1/1/lookup.php?i="
    meal_URL += id
    #Get JSON response
    recipeJSON = get_json(meal_URL)
    recipeJSON = recipeJSON['meals'][0]
    #Fetch recipe details
    title = recipeJSON['strMeal']
    area = recipeJSON['strArea']
    image = recipeJSON['strMealThumb']
    instructions = recipeJSON['strInstructions'] 
    youtube = recipeJSON['strYoutube']
    ingreds = []

    #The JSON always has 20 ingredient and keys, and 20 corresponding measurement keys.
    # Now we need to fetch the ones that aren't empty.
    for index in range(1, 20):
        # Get ingredient name
        ingredKey = "strIngredient" + str(index)
        cur_ingred = recipeJSON[ingredKey]
        #If we have an ingredient name, then get the measurement associated with it, assuming it's not null
        if cur_ingred != "" and cur_ingred != "null" and cur_ingred != None:
            #Get measurement
            measureKey = "strMeasure" + str(index)
            cur_measure = recipeJSON[measureKey]
            #Join the ingredient with its measurement. Make sure there is actually a value for the measurement
            if cur_measure != "" and cur_measure != "null" and cur_measure != None:
                complete_ingred = cur_measure + " " + cur_ingred
            else:
                complete_ingred = cur_ingred
            ingreds.append(complete_ingred)

    #Instance of recipe class, with its properties assigned the values previously found
    recipe = FullRecipe(id, title, image, area, instructions, youtube, ingreds)
    
    #Call method to alter URL so it can be emedded
    recipe.embedYouTube()
    #Call function to parse out instructions into separate steps
    recipe.instructions = parseInstructions(instructions)

    context = { "recipe": recipe }
    return render(request, "Meal/recipe.html", context)

#Given a URL to an API, returns a JSON object
def get_json(url):
    json_object = urllib.request.urlopen(url).read()
    return json.loads(json_object)

#Modifies yotube URL so that it will be embedded in HTML
def embedYouTube(url):
    if "watch?v=" in url:
        url = url.replace("watch?v=", "embed/")
        url += "?controls=1"

def parseInstructions(instructions):
    #Find newline characters in instruction, and split string into list of strings,
    #previously separated by these characters
    directionList = instructions.split("\r\n")
    finishedDirections = []
    
    counter = 0
    for direction in directionList:
        #Append step number before string
        stepLabel = str(counter + 1) + ".  "
        direction = stepLabel + direction
        #Make sure the current string is not nothing but the step number, i.e.,
        #the string after splitting was another newline character
        if direction != str(counter + 1) + ".  ":
            finishedDirections.append(direction)
            counter += 1
    #Convert previous string property to the list of directions. The benefit of a weakly typed language :)
    return finishedDirections