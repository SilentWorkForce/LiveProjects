from django.shortcuts import render, redirect, get_object_or_404
from .forms import MessageForm
from datetime import datetime
from .models import Message
from django.contrib.auth.models import User
from django.db.models import Q


def message(request):
    if request.method == "POST":
        form = MessageForm(request.POST)
        if form.is_valid():
            message = form.save(commit=False)
            message.sender = request.user
            message.sent = datetime.now()
            message.save()
            #After this message has been assigned an id and saved, fetch its id and assign it to the thread id
            same_message = Message.objects.filter().order_by('-id')[0]
            same_message.thread = same_message.id
            same_message.save()
            return redirect('inbox')
    else:
        form = MessageForm()
    return render(request, 'Chat/message.html', {'form': form})

def inbox(request):
    #If a message is replied to in the inbox
    if request.method == "POST":
        form = MessageForm(request.POST)
        if form.is_valid():
            message = form.save(commit=False)
            message.sender = request.user
            message.recipient = request.POST.get('response-recipient', None)
            message.thread = request.POST.get('response-thread', None)
            message.sent = datetime.now()
            message.save()
        return redirect('inbox')
    #If inbox is loaded
    else:
        messages = Message.objects.filter(Q(sender=request.user) | Q(recipient=request.user)).order_by('-thread')
        form = MessageForm()
        return render(request, 'chat/inbox.html', {'messages': messages, 'form': form})
