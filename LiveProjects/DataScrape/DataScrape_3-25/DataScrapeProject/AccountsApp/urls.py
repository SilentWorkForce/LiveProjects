from django.urls import path
from django.conf.urls import url
from . import views
from django.contrib.auth import views as auth_views



urlpatterns = [
    path('signup/', views.SignUp.as_view(), name='signup'),
    path('password-reset/',
        auth_views.PasswordResetView.as_view(template_name='registration/password_reset.html'),
            name='password_reset'),
    path('password-reset/done/', auth_views.PasswordResetDoneView.as_view(template_name='registration/password_reset_done.html'), name='password_reset_done'),
    path('password-reset/confirm/<uidb645>/<token>/', auth_views.PasswordResetConfirmView.as_view(template_name='registration/password_reset_confirm.html'), name='password_reset_confirm'),
    path('', views.location, name='location'),
    #path('create_post/', views.create_post, name='create_post'),
]