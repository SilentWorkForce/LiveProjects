"""
Django settings for DataScrapeProject project.

Generated by 'django-admin startproject' using Django 2.1.5.

For more information on this file, see
https://docs.djangoproject.com/en/2.1/topics/settings/

For the full list of settings and their values, see
https://docs.djangoproject.com/en/2.1/ref/settings/
"""

import os


# Build paths inside the project like this: os.path.join(BASE_DIR, ...)
BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))


# Quick-start development settings - unsuitable for production
# See https://docs.djangoproject.com/en/2.1/howto/deployment/checklist/

# SECURITY WARNING: keep the secret key used in production secret!
SECRET_KEY = 'vfblsn)ku=9rk28p*13qbllk%m8$)!v-o=_b%@!4j&%i32qcn2'

#  API KEY FOR NASA
os.environ["NASA_API_KEY"] = "kK4OwaUtVghqi9A1m40BsWAueYlagAIUQFM6FYlw"

# SECURITY WARNING: don't run with debug turned on in production!
DEBUG = True

ALLOWED_HOSTS = [
    '*'
]


# Application definition

INSTALLED_APPS = [
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    'DataApp',
    'MessagesApp',
    'AccountsApp',
    'CalendarApp',
    'WeatherApp',
    'SpaceApp',
    'TrafficApp',
    'AppDemoNews',
    'PodcastsApp', #Adds PodcastsApp for top 5 podcasts on Stitcher
    'BandsApp', # added BandsApp - KW
    'CraigslistApp',
    'BudgetApp',
    'HabitTrackerApp',
    'RestaurantApp',
    'MealApp',
]

MIDDLEWARE = [
    'django.middleware.security.SecurityMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    'django.contrib.messages.middleware.MessageMiddleware',
    'django.middleware.clickjacking.XFrameOptionsMiddleware',
]

ROOT_URLCONF = 'DataScrapeProject.urls'

TEMPLATES = [
    {
        'BACKEND': 'django.template.backends.django.DjangoTemplates',
        'DIRS': [os.path.join(BASE_DIR, 'templates').replace('\\','/')], 
        'APP_DIRS': True,
        'OPTIONS': {
            'context_processors': [
                'django.template.context_processors.debug',
                'django.template.context_processors.request',
                'django.contrib.auth.context_processors.auth',
                'django.contrib.messages.context_processors.messages',
            ],
        },
    },
]

WSGI_APPLICATION = 'DataScrapeProject.wsgi.application'


# Database
# https://docs.djangoproject.com/en/2.1/ref/settings/#databases

DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.sqlite3',
        'NAME': os.path.join(BASE_DIR, 'db.sqlite3'),
    }
}


# Password validation
# https://docs.djangoproject.com/en/2.1/ref/settings/#auth-password-validators

# custom validators

AUTH_PASSWORD_VALIDATORS = [


    {
         'NAME': 'AccountsApp.validators.NumberValidator',
    },
    {
         'NAME': 'AccountsApp.validators.UppercaseValidator',
    },
    {
         'NAME': 'AccountsApp.validators.LowercaseValidator',
    },
    {
         'NAME': 'AccountsApp.validators.SymbolValidator',
    },

    


    

]
# Built in Django validators

# AUTH_PASSWORD_VALIDATORS = [
#     {'NAME': 'django.contrib.auth.password_validation.MinimumLengthValidator',
#         'OPTIONS': {
#             'min_length': 12, }
#      },
#     {'NAME': 'django.contrib.auth.password_validation.CommonPasswordValidator', },
#     {'NAME': 'my.project.validators.NumberValidator',
#         'OPTIONS': {
#             'min_digits': 3, }},
#     {'NAME': 'my.project.validators.UppercaseValidator', },
#     {'NAME': 'my.project.validators.LowercaseValidator', },
#     {'NAME': 'my.project.validators.SymbolValidator', },
# ]


EMAIL_BACKEND = 'django.core.mail.backends.console.EmailBackend'  # During development only



# Internationalization
# https://docs.djangoproject.com/en/2.1/topics/i18n/

LANGUAGE_CODE = 'en-us'

TIME_ZONE = 'UTC'

USE_I18N = True

USE_L10N = True

USE_TZ = True


# Static files (CSS, JavaScript, Images)
# https://docs.djangoproject.com/en/2.1/howto/static-files/

STATIC_URL = '/static/'
STATICFILES_DIRS = (
    os.path.join(BASE_DIR, 'static'),
)
STATIC_ROOT = os.path.join(BASE_DIR, 'staticfiles')

MEDIA_URL  = '/media/'
MEDIA_ROOT = os.path.join(BASE_DIR, 'DataScrapeProject/media')

LOGIN_REDIRECT_URL = 'home'
LOGOUT_REDIRECT_URL = 'home'

