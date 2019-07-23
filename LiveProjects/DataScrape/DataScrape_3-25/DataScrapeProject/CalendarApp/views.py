from __future__ import print_function
import datetime
import pickle
import os.path
from googleapiclient.discovery import build
from google_auth_oauthlib.flow import InstalledAppFlow
from google.auth.transport.requests import Request
from django.views.generic import TemplateView
from django.shortcuts import render
from django.http import Http404, HttpResponse, HttpResponseRedirect
from CalendarApp.forms import EventForm

#Google Events Calendar 

# Connect to Google calendar API and declare auth SCOPE
SCOPES = ['https://www.googleapis.com/auth/calendar']

def GetCredentials():
    creds = None
    # The file token.pickle stores the user's access and refresh tokens, and is
    # created automatically when the authorization flow completes for the first
    # time.
    if os.path.exists('token.pickle'):
        with open('token.pickle', 'rb') as token:
            creds = pickle.load(token)
    # If there are no (valid) credentials available, let the user log in.
    if not creds or not creds.valid:     
        if creds and creds.expired and creds.refresh_token:
            creds.refresh(Request())
        else:
            flow = InstalledAppFlow.from_client_secrets_file(
                'credentials.json', SCOPES)
            creds = flow.run_local_server()
        # Save the credentials for the next run
        with open('token.pickle', 'wb') as token:
            pickle.dump(creds, token)

    #call Google API for calendar version and attach credentials 
    service = build('calendar', 'v3', credentials=creds)
    return service
    
class EventCalendar(TemplateView):
    #passes the events-calendar.html into the variable template_name
    template_name = 'CalendarApp/events-calendar.html'
    #Display the Event creation Form
    def get(self,request):
        #create instantiation of the class EventForm and store it in the variable form
        form = EventForm()
        #return html page held in the variable template_name and pass in the varible "form" as "form" 
        return render (request, self.template_name, {'form':form})

    #Process an Event from the Event creation Form
    def post(self, request):
        #Checks if HTTP request method is equale to "POST"
        if request.method == 'POST':
            #If method is equal to "POST", retrieve event form with user entries in each field 
            form = EventForm(request.POST)
            #If call method ".is_valid" on form to check if it is valid
            if form.is_valid():
                data = request.POST.copy() #retrieve data from form at update-calendar.html
                event_summary = data.get('event_title') #assign data in event_title field to variable event_summary
                event_start_date = data.get('start_date') #assign data in start_date field to variable event_start_date
                event_start_time = data.get('start_time') #assign data in start_time field to variable event_start_time
                event_end_date = data.get('end_date') #assign data in end_date field to variable event_end_date
                event_end_time = data.get('end_time') #assign data in end_time field to variable event_end_time
                print(event_summary, event_start_date, event_start_time, event_end_date, event_end_time) #prints summary and start and end time and date of newly created event
                    
                #Get the event form the Event Form and insert into Google Calendar
                event = {
                    'summary': event_summary,
                    'start': {
                        'dateTime': event_start_date+'T'+event_start_time+'-07:00',
                        'timeZone': 'America/Los_Angeles',
                    },
                    'end': {
                    'dateTime': event_end_date+'T'+event_end_time+'-07:00',
                    'timeZone': 'America/Los_Angeles',
                    },
                    'reminders': {
                        'useDefault': False,
                        'overrides': [
                        {'method': 'email', 'minutes': 24 * 60},
                        {'method': 'popup', 'minutes': 10},
                        ],
                    },
                }
                #call GetCredentials and assign returned value to the variable service 
                service = GetCredentials()
                #call the APIs "insert" method and pass in the calendarId and event as aurguments
                event = service.events().insert(calendarId='primary', body=event).execute()
               # print (event.get('htmlLink')) 
                return HttpResponseRedirect('/events-calendar/')       
        else:
            form = EventForm()
            args = {'form':form}
            return render (request, self.template_name, args)

class UpdateCalendar(TemplateView):
    #passes the events-calendar/update-calendar.html into the variable template_name
    template_name = 'CalendarApp/events-calendar/update-calendar.html'
    #Display the Event update Form
    def get(self,request):
        print("Enter UpdateCalendar> GET()")
        #create instantiation of the class EventForm and store it in the variable form
        form = EventForm()
        #return html page held in the variable template_name and pass in the varible "form" as "form" 
        return render (request, self.template_name, {'form':form})

    #Process an Event from the Event creation Form
    def post(self,request):
        #Checks if HTTP request method is equale to "POST"
        print("Enter UpdateCalendar> POST()")
        if request.method == 'POST':
            #If method is equal to "POST", retrieve event form with user entries in each field 
            form = EventForm(request.POST)
            #If call method ".is_valid" on form to check if it is valid
            if form.is_valid():
                print("Enter UpdateCalendar> form IS valid()")
                data = request.POST.copy() #retrieve data from form at update-calendar.html
                event_summary = data.get('event_title') #assign data in event_title field to variable event_summary
                event_start_date = data.get('start_date') #assign data in start_date field to variable event_start_date
                event_start_time = data.get('start_time') #assign data in start_time field to variable event_start_time
                event_end_date = data.get('end_date') #assign data in end_date field to variable event_end_date
                event_end_time = data.get('end_time') #assign data in end_time field to variable event_end_time
                print(event_summary, event_start_date, event_start_time, event_end_date, event_end_time)

                #call GetCredentials and assign returned value to the variable service 
                service = GetCredentials()
                print("^^service = GetCredentials()")
                #retrieves 20 events based off the relative calendar being used
                events_result = service.events().list(calendarId='primary',
                                                    maxResults=20, singleEvents=True,
                                                    orderBy='startTime').execute()
                #creates list based off the results of service.events().list 
                events = events_result.get('items', [])

                #creates for loop to iterate over events list
                for event in events:
                    #if statement to check if the user entry in the field "summary" is equal to the value of the "summary" key of the current event 
                    if event['summary'] == event_summary:
                        #if the "summary" entered by user matches the "summary" of current event, store that events "id" in the variable "eventId"
                        eventId = event['id']      

                        #call the APIs "get" method and pass in the "calendarId" and "eventId" as aurguments
                        event = service.events().get(calendarId='primary', eventId=eventId).execute()
                        
                        #take the currently existing event and alter the values of the keys according to user imput
                        event = {
                                    'summary': event_summary,
                                    'start': {
                                        'dateTime': event_start_date+'T'+event_start_time+'-07:00',
                                        'timeZone': 'America/Los_Angeles',
                                    },
                                    'end': {
                                        'dateTime': event_end_date+'T'+event_end_time+'-07:00',
                                        'timeZone': 'America/Los_Angeles',
                                    },
                                }

                        #call the APIs "update" method and pass in the calendarId, eventId and event info as aurguments
                        updated_event = service.events().update(calendarId='primary', eventId=eventId, body=event).execute()
                        # Print the updated date to the console.
                        print (updated_event['updated'])
                        #redirect user back to the events-calendar html page.
                return HttpResponseRedirect('/events-calendar/')    
            else:
                print("CalendarApp > form IS NOT valid()")    
                return HttpResponseRedirect('/events-calendar/')
