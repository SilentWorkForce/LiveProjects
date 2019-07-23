import csv
import time
from calendar import monthrange
from decimal import Decimal
from dateutil.parser import parse
from django.shortcuts import render, redirect
from .models import Budget
from .forms import BudgetForm, BudgetInputForm


def budget_input(request):
    budget_form_input = BudgetInputForm()
    has_user_data_existing = (
        True if Budget.existing.filter(user=request.user.id).exists() else False
    )
    return render(
        request,
        "BudgetApp/budget-input.html",
        {
            "budget_input": budget_form_input,
            "has_user_data_existing": has_user_data_existing,
        },
    )


def budget_output(request, username):
    user_data_existing = Budget.existing.filter(user=request.user.id)
    # if another user's budget or a blank budget is requested, redirect to input form
    if request.method == "GET" and (
        username != request.user.username or not user_data_existing.exists()
    ):
        return redirect("budget_input")
    else:
        budget_form_output = {"accepted": {"actuals": [], "budget": []}, "rejected": []}
        dates_and_actuals_existing = user_data_existing.values("date", "actual")
        budget_existing = user_data_existing.values("budget")
        # fields not contained in the request default to existing data
        dates_and_actuals_rcvd = request.POST.get(
            "dates_and_actuals", dates_and_actuals_existing
        )
        budget_rcvd = request.POST.get(
            "budget",
            budget_existing[:1][0]["budget"] if budget_existing.exists() else 0,
        )
        # if request contained dates_and_actuals, clear old values and validate new ones
        if request.method == "POST" and (
            dates_and_actuals_rcvd != dates_and_actuals_existing
            and dates_and_actuals_rcvd != ""
        ):
            # TODO: allow user to append new transactions to existing dataset
            user_data_existing.delete()
            dates_and_actuals_rcvd = list(
                csv.reader(dates_and_actuals_rcvd.splitlines())
            )
            for row, data in enumerate(dates_and_actuals_rcvd):
                try:
                    user = request.user.id
                    date = parse(data[0].strip())
                    actual = Decimal(data[1].strip())
                    budget = Decimal(
                        "%.2f"
                        % (
                            Decimal(budget_rcvd)
                            / Decimal(monthrange(date.year, date.month)[1])
                        )
                    )
                    budget_form_input_clean = BudgetForm(
                        data={
                            "user": user,
                            "date": date,
                            "actual": actual,
                            "budget": budget,
                        }
                    )
                    if budget_form_input_clean.is_valid():
                        budget_form_input_clean.save()
                        # reconcile py vs js expected data formats
                        budget_form_output["accepted"]["actuals"].append(
                            {"t": date.timestamp() * 1000, "y": "%.2f" % actual}
                        )
                        budget_form_output["accepted"]["budget"].append(
                            {"t": date.timestamp() * 1000, "y": "%.2f" % budget}
                        )
                        print(
                            "Row {} of {}: OK".format(
                                row + 1, len(dates_and_actuals_rcvd)
                            )
                        )
                    else:
                        budget_form_output["rejected"].append(
                            {
                                "data": data,
                                "message": budget_form_input_clean.errors.as_text,
                            }
                        )
                        print(
                            "Row {} of {}: ERROR! -- {}".format(
                                row + 1,
                                len(dates_and_actuals_rcvd),
                                budget_form_input_clean.errors.as_text,
                            )
                        )
                # TODO: user-friendly output for caught exceptions
                except Exception as e:
                    budget_form_output["rejected"].append({"data": data, "message": e})
                    print(
                        "Row {} of {}: ERROR! -- {}".format(
                            row + 1, len(dates_and_actuals_rcvd), e
                        )
                    )
        # if request did not contain any new data, return existing data
        else:
            for record in user_data_existing:
                budget_form_output["accepted"]["actuals"].append(
                    {
                        "t": time.mktime(record.date.timetuple()) * 1000,
                        "y": "%.2f" % record.actual,
                    }
                )
                budget_form_output["accepted"]["budget"].append(
                    {
                        "t": time.mktime(record.date.timetuple()) * 1000,
                        "y": "%.2f" % record.budget,
                    }
                )
    # ensure transactions are ordered by date before passing the data to our chart
    budget_form_output["accepted"]["actuals"] = sorted(
        budget_form_output["accepted"]["actuals"], key=lambda field: field["t"]
    )
    budget_form_output["accepted"]["budget"] = sorted(
        budget_form_output["accepted"]["budget"], key=lambda field: field["t"]
    )
    return render(
        request, "BudgetApp/budget-output.html", {"budget_output": budget_form_output}
    )
