import re

from django.core.exceptions import ValidationError
from django.utils.translation import ugettext as _


class NumberValidator(object):
    def validate(self, password, user=None):
        if not re.findall('\d', password):
            raise ValidationError(
                _("The password must contain at least 1 digit, 0-9."),
                code='password_no_number',
            )

    def get_help_text(self):
        return _(
            "Your password must contain at least 1 digit, 0-9."
        )


class UppercaseValidator(object):
    def validate(self, password, user=None):
        if not re.findall('[A-Z]', password):
            raise ValidationError(
                _("The password must contain at least 1 uppercase letter, A-Z."),
                code='password_no_upper',
            )

    def get_help_text(self):
        return _(
            "Your password must contain at least 1 uppercase letter, A-Z."
        )


class LowercaseValidator(object):
    def validate(self, password, user=None):
        if not re.findall('[a-z]', password):
            raise ValidationError(
                _("The password must contain at least 1 lowercase letter, a-z."),
                code='password_no_lower',
            )

    def get_help_text(self):
        return _(
            "Your password must contain at least 1 lowercase letter, a-z."
        )


class SymbolValidator(object):
    def validate(self, password, user=None):
        if not re.findall('[()[\]{}|\\`~!@#$%^&*_\-+=;:\'",<>./?]', password):
            raise ValidationError(
                _("The password must contain at least 1 symbol: " +
                  "()[]{}|\`~!@#$%^&*_-+=;:'\",<>./?"),
                code='password_no_symbol',
            )

    def get_help_text(self):
        return _(
            "Your password must contain at least 1 symbol: " +
            "()[]{}|\`~!@#$%^&*_-+=;:'\",<>./?"
        )

# How to Make Your Validators Configurable

# If you plan to reuse this code on different projects, you can make your validators more configurable 
# through the Django settings. Then you can pass variables into your validator like the min_length 
# value sent to the MinimumLengthValidator shown above.

# Here I've adjusted my NumberValidator to have a configurable number of digits by adding the __init__ 
# method and adjusting the validate method to check the length of the number of matches:

# class NumberValidator(object):
#     def __init__(self, min_digits=0):
#         self.min_digits = min_digits

#     def validate(self, password, password1, user=None):
#         if not len(re.findall('\d', password)) >= self.min_digits:
#             raise ValidationError(
#                 _("The password must contain at least %(min_digits)d digit(s), 0-9."),
#                 code='password_no_number',
#                 params={'min_digits': self.min_digits},
#             )

#     def get_help_text(self):
#         return _(
#             "Your password must contain at least %(min_digits)d digit(s), 0-9." % {'min_digits': self.min_digits}
#         )