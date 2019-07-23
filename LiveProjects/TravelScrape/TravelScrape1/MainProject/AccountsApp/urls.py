
from django.urls import path
from django.conf.urls import url
from . import views
from django.contrib import admin
from django.urls import path, include 

urlpatterns = [
    path('admin/', admin.site.urls),
    path('AccountsApp/', include('django.contrib.auth.urls')), 
]


urlpatterns = [
    path('', views.SignUp),
    path('login/', views.login),
]

# urlpatterns = [
#     ...
#     url(r'^signup/$', views.signup, name='signup'),
# ]
