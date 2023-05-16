import { Component, ElementRef, HostListener, Input, OnInit, ViewChild } from '@angular/core'
import { Subject } from 'rxjs'
import { fadeIn, fadeInOut } from '../animations'
import { NavigationEnd, NavigationStart, Router } from '@angular/router';
import {
  ChatClient,
  GetChatResponseQuery,
  ChatMessageVm
} from '../../app/web-api-client';

const rand = max => Math.floor(Math.random() * max)

@Component({
  selector: 'chat-widget',
  templateUrl: './chat-widget.component.html',
  styleUrls: ['./chat-widget.component.css'],
  animations: [fadeInOut, fadeIn],
})
export class ChatWidgetComponent implements OnInit {
  @ViewChild('bottom') bottom: ElementRef;
  @Input() public theme: 'blue' | 'grey' | 'red' = 'blue';
  public _visible = false;
  public _refreshing = false;
  _previousScrollPosition = 0;
  _chatConversationId = undefined;
  previousMessages: ChatMessageVm[] = [];

  constructor(
    private chatClient: ChatClient,
    private router: Router
  ) {
    this.router.events.forEach((event) => {
      // NavigationCancel
      // NavigationError
      // RoutesRecognized
      if (event instanceof NavigationStart) {
        if (!this._refreshing) {
          if (event.url.toLowerCase().indexOf('login') != -1) {
            this.addMessage(this.operator, 'Logging in.', 'received');
          }
          else {
            this.addMessage(this.operator, 'Navigating to the ' + (event.url.split('/').join(' ').trim() || 'home') + ' page.', 'received');
          }
        }
      }
      if (event instanceof NavigationEnd) {
        if (this._refreshing) {
          this._refreshing = false;
          //window.scrollTo({ top: this._previousScrollPosition, behavior: 'instant' });
          setTimeout(() => {
            window.scrollTo({ top: this._previousScrollPosition, behavior: 'auto' });
          }, 1000);
        }
        else {
          setTimeout(() => {
            this._chatConversationId = undefined;
            while (this.messages.length > 0) {
              this.messages.pop();
            }
            while (this.previousMessages.length > 0) {
              this.previousMessages.pop();
            }
            this.addMessage(this.operator, 'How can I help you manage your ' + this.getCurrentPageName(), 'received')
          }, 1500);
        }
      }
    });
  }

  public get visible() {
    return this._visible
  }

  @Input() public set visible(visible) {
    this._visible = visible
    if (this._visible) {
      setTimeout(() => {
        this.scrollToBottom()
        //this.focusMessage()
      }, 0)
    }
  }

  /*public focus = new Subject()*/

  public operator = {
    name: 'Operator',
    status: 'online',
    avatar: `https://randomuser.me/api/portraits/women/${rand(100)}.jpg`,
  }

  public client = {
    name: 'Guest User',
    status: 'online',
    avatar: `https://randomuser.me/api/portraits/men/${rand(100)}.jpg`,
  }

  public messages = []

  public addMessage(from, text, type: 'received' | 'sent') {
    this.messages.unshift({
      from,
      text,
      type,
      date: new Date().getTime(),
    })
    this.scrollToBottom()
  }

  public scrollToBottom() {
    if (this.bottom !== undefined) {
      this.bottom.nativeElement.scrollIntoView()
    }
  }

  //public focusMessage() {
  //  this.focus.next(true)
  //}

  ngOnInit() {
    setTimeout(() => this.visible = true, 2000)
  }

  public toggleChat() {
    this.visible = !this.visible
  }

  getCurrentPageName() {
    return this.router.url.split('/').join(' ').trim() || 'home';
  }

  public sendMessage({ message }) {
    if (message.trim() === '') {
      return
    }
    this.chatClient.create(new GetChatResponseQuery({
      chatMessage: new ChatMessageVm({ message: message, from: 2 }),
      previousMessages: this.previousMessages,
      chatConversationId: this._chatConversationId,
      currentUrl: this.getCurrentPageName()
    })).subscribe(
      result => {
        if (result.createNewChat) {
          this.addMessage(this.operator, 'System: Something went wrong, creating new chat instance', 'received');
          setTimeout(() => {
            this._chatConversationId = undefined;
            while (this.messages.length > 0) {
              this.messages.pop();
            }
            while (this.previousMessages.length > 0) {
              this.previousMessages.pop();
            }
            this.router.navigateByUrl(this.router.url);
            setTimeout(() => {
              this.addMessage(this.operator, 'How can I help you manage your ' + this.getCurrentPageName(), 'received')
            }, 500);
          }, 2000);
        }
        this._chatConversationId = result.chatConversationId;
        this.previousMessages = result.previousMessages;
        this.addMessage(this.operator, result.responseMessage.message, 'received');
        if (result.dirty) {
          this._refreshing = true;
          this._previousScrollPosition = window.scrollY || document.getElementsByTagName("html")[0].scrollTop;
          this.router.navigateByUrl(this.router.url);
        }
      },
      error => console.error(error)
    );
    this.addMessage(this.client, message, 'sent')
  }

  @HostListener('document:keypress', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === '/') {
      //this.focusMessage()
    }
    if (event.key === '?' && !this._visible) {
      this.toggleChat()
    }
  }

}
