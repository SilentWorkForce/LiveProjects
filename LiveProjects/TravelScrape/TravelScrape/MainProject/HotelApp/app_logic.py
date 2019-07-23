from bs4 import BeautifulSoup
import csv
import lxml
import requests 



# Used to collect initial data. In case the .csv file ever gets deleted, running this for each letter of the alphabet will recreate it. You will need to move it to the HotelApp/resources folder.
def scrape(input): #Input must be a capital letter
    source="https://en.wikipedia.org/wiki/List_of_airports_by_IATA_code:_{}".format(input) #Uses BeatifulSoup to scrape data from Wikipedia.
    response = requests.get(source)
    soup = BeautifulSoup(response.content, 'lxml')
    database = soup.find("tbody").findAll("tr")
    for entry in database:
        try:
            code = entry.select('td')[0].get_text(strip=True) 
            code_bare = code.split("[")[0]  #This removes the footnote references that are attached to some of the entries on Wikipedia.
            city = entry.select('td')[3].get_text(strip=True)
            with open("airports.csv", "a") as logger:
                logger.write("\n" + code_bare + "," + city)
        except:
            continue
            
#Return a list of all potential matches for the user to select from. This handles problems arising from cities with multiple codes and from duplicate city names.
def findMatches(request):
    with open("./HotelApp/resources/airports.csv") as database:
        reader = csv.reader(database)
        results = {}
        for row in reader:
            if request in row[1] or request in row[4]:   #row 4 contains place names stripped of accented characters, to make those characters unnecessary when searching.
                if row[3] != "":  #There will be no row[3] for entries that have no state names.
                    city = "{}, {}, {} - ({}) ".format(row[1],row[2],row[3],row[0])
                else:
                    city = "{}, {} - ({}) ".format(row[1],row[2],row[0])
                results[city] = row[0]
    return results