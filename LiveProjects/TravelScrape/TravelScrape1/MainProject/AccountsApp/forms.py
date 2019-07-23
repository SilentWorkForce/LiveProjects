from django import forms
from .models import UserProfile
from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.models import User
from datetime import datetime 


# Extending UserCreation Form
class RegistrationForm(UserCreationForm):
    email = forms.EmailField(max_length=75)

    class Meta:
        model = User
        fields = ( 
            "username", 
            "email",      
            "password1",
            "password2" 
        )
    


    def save(self, commit=True):
        user = super(RegistrationForm, self).save(commit=False)
        user.email = self.cleaned_data["email"]
        if commit:
            user.save()
        return user

#Upload Profile Image

class ProfileForm(forms.ModelForm):
    class Meta:
        model = UserProfile
        fields = ['image']

