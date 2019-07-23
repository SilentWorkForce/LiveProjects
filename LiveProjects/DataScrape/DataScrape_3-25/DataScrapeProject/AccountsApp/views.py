from __future__ import print_function
from django.shortcuts import render
from django.contrib.auth.forms import UserCreationForm
from django.urls import reverse_lazy
from django.views import generic
import requests

from django.contrib.auth.models import User
from AccountsApp.models import UserProfile
from AccountsApp.forms import ProfileForm, RegistrationForm
#from AccountsApp.validators import NumberValidator, UppercaseValidator, LowercaseValidator, SymbolValidator
from django.views.generic import TemplateView
from django.http import Http404, HttpResponse, HttpResponseRedirect

# Signup page.

class SignUp(generic.CreateView):
    form_class = RegistrationForm
    success_url = reverse_lazy('login')
    template_name = 'signup.html'
    








#Get user's current location   

def location(request):
    url='https://api.ipgeolocation.io/ipgeo?apiKey=b19ee562aa72436e94a39580c8265ad3'
    
    r = requests.get(url).json()

    location_data  = {
        'city' : r['city'],
        'state' : r['state_prov'],
        'zip' : r['zipcode']
    }

    print(location_data)
    context = {"location_data" : location_data}

    return render(request, 'location/location.html', context)

#Profile Image Upload and Save

class ProfileView(TemplateView):
    template_name = 'profile.html'
    
    def get(self,request):
        form = ProfileForm()
        #Get preference settings for buttons
        userTracking = request.user.userprofile.user_tracking
        context = {'form':form, 'tracking': userTracking}
        return render (request, self.template_name, context)

    def post(self, request):
        
        if request.method == 'POST':

            form_old = ProfileForm(data=request.POST, files=request.FILES)
            if form_old.is_valid() and 'image' in request.FILES:
                request.user.userprofile.image.delete()

            form = ProfileForm(request.POST, request.FILES, instance=request.user.userprofile)
            if form.is_valid():
                form.save()
        else:
            form = ProfileForm(instance=request.user.userprofile)
        return render (request, self.template_name, {'form':form})

#Check if Geolocation is on
def create_post(request):
    check_value = request.POST['text'] #Get value from AJAX post
    curruser = request.user.userprofile
    
    #Save user's preferences
    if check_value == '1':
        curruser.user_tracking = 1
        #Save user's new location
        url='https://api.ipgeolocation.io/ipgeo?apiKey=b19ee562aa72436e94a39580c8265ad3'
        r = requests.get(url).json()
        curruser.city = r['city']
        curruser.lng = float(r['longitude'])
        curruser.lat = float(r['latitude'])
    else:
        curruser.user_tracking = 0

    curruser.save()
    return HttpResponse(request)