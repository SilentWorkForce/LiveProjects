from django import forms
from .models import Budget


class BudgetInputForm(forms.Form):
    dates_and_actuals = forms.CharField(
        label="Spending Data",
        widget=forms.Textarea(
            attrs={
                "placeholder": "Paste data in the following format:\nDate,Amount\n\n"
                "For example:\n12/31/18,101.23\n1/1/2019,105\n02/03/19,99.5\n03/4/2019,105.00\n\n"
                "Note that any excess whitespace will be trimmed.",
                "style": "display:block",
                "class": "mb-3",
            }
        ),
    )
    budget = forms.DecimalField(
        label="Monthly Budget",
        max_digits=8,
        decimal_places=2,
        widget=forms.NumberInput(attrs={"placeholder": "$ per month"}),
    )


class BudgetForm(forms.ModelForm):
    class Meta:
        model = Budget
        fields = ["user", "date", "actual", "budget"]
