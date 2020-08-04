import { Component, OnInit, Input, EventEmitter, Output, ViewChild, ElementRef } from '@angular/core';
import { Message, MessageHistory, ClientMeta } from 'src/classes/message';
import { AuthService } from 'src/app/auth/auth.service';
import { ProfileService } from 'src/app/services/Profile.service';
import { MessageApiReadResponse } from 'src/classes/responseTypes';
import { SantaApiPutService } from 'src/app/services/santaApiService.service';
import { MapResponse } from 'src/app/services/mapService.service';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit{

  constructor(
    public SantaApiPut: SantaApiPutService,
    public responseMapper: MapResponse,
    public auth: AuthService) { }
  
  // Boolean value for passing whether or not the emit is a soft update or not
  @Output() messageUpdatedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input() selectedHistory: MessageHistory = new MessageHistory();
  @Input() sendingClientMeta: ClientMeta = new ClientMeta();
  @Input() showLoading: boolean = false;
  @Input() showActionProgressBar: boolean = false;
  
  @ViewChild('chatFrame', {static: false}) chatFrame: ElementRef;

  public isAdmin: boolean;
  public markingRead: boolean = false;

  ngOnInit(): void {
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
    
  }
  public totalHistory() : Array<Message>
  {
    let allMessages: Array<Message> = []
    this.selectedHistory.recieverMessages.forEach((message: Message) => {
      allMessages.push(message);
    });
    this.selectedHistory.subjectMessages.forEach((message: Message) => {
      allMessages.push(message);
    });
    return allMessages.sort((a: Message, b: Message) => {
      return a.dateTimeSent.getTime() - b.dateTimeSent.getTime();
    }); 
  }
  public scrollToBottom(): void {
    try {
        this.chatFrame.nativeElement.scrollTop = this.chatFrame.nativeElement.scrollHeight;
    } catch(err) { }
  }
  public async markRead(message: Message)
  {
    this.markingRead = true;

    let putMessage = new MessageApiReadResponse();

    putMessage.isMessageRead = true;
    await this.SantaApiPut.putMessageReadStatus(message.chatMessageID, putMessage).toPromise().catch((err) => {console.log(err);});

    this.markingRead = false;
    this.messageUpdatedEvent.emit(true);
  }
}
