import { Component, OnInit } from '@angular/core';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Client } from 'src/classes/client';
import { Status } from 'src/classes/status';
import { Tag } from 'src/classes/tag';
import { GathererService } from '../services/gatherer.service';
import { EventType } from 'src/classes/eventType';
import { SearchQueryModelResponse } from 'src/classes/responseTypes';
import { SantaApiGetService, SantaApiPostService } from '../services/santaApiService.service';
import { MapService } from '../services/mapService.service';
import { trigger, state, style, transition, animate } from '@angular/animations';

export class SearchQueryObjectContainer
{
  tags: Array<Tag> = [];
  events: Array<EventType> = [];
  statuses: Array<Status> = [];
  names: Array<string> = [];
  nicknames: Array<string> = [];

  get tagQueryIDs() : Array<string>
  {
    let tagIDList: Array<string> = [];

    this.tags.forEach((tag: Tag) => {
      tagIDList.push(tag.tagID);
    });

    return tagIDList;
  }
  get eventQueryIDs() : Array<string>
  {
    let eventIDList: Array<string> = [];

    this.events.forEach((event: EventType) => {
      eventIDList.push(event.eventTypeID);
    });

    return eventIDList;
  }
  get statusesQueryIDs() : Array<string>
  {
    let statusIDList: Array<string> = [];

    this.statuses.forEach((status: Status) => {
      statusIDList.push(status.statusID);
    });

    return statusIDList;
  }

}
@Component({
  selector: 'app-agent-catalogue',
  templateUrl: './agent-catalogue.component.html',
  styleUrls: ['./agent-catalogue.component.css'],
  animations: [
    // the fade-in/fade-out animation.
    trigger('simpleFadeAnimation', [

      // the "in" style determines the "resting" state of the element when it is visible.
      state('in', style({opacity: 1})),

      // fade in when created. this could also be written as transition('void => *')
      transition(':enter', [
        style({opacity: 0}),
        animate(200)
      ]),

      // fade out when destroyed. this could also be written as transition('void => *')
      transition(':leave',
        animate(200, style({opacity: 0})))
    ])
  ]
})
export class AgentCatalogueComponent implements OnInit {

  constructor(private gatherer: GathererService,
    private formBuilder: FormBuilder,
    private santaApiPost: SantaApiPostService,
    private mapper: MapService) { }

  public allTags: Array<Tag> = [];
  public allEvents: Array<EventType> = [];
  public allClientStatuses: Array<Status> = [];

  public foundClients: Array<Client> = [];
  public selectedClient: Client = new Client();
  public selectedSearchType: string = "soft";

  public gatheringAllTags: boolean;
  public gatheringAllClientStatuses: boolean;
  public gatheringAllEvents: boolean;
  public searchingClients: boolean;
  public clickLocked: boolean;

  public showHelper: boolean;
  public showClientCard: boolean;

  public infoMessage: string = "Soft searches find any values that might match any values within respective groups of tags, events, statuses, names, and nicknames. Hard searches will show exact matches"

  public searchQueryString: string = '';
  public searchQueryObjectHolder: SearchQueryObjectContainer = new SearchQueryObjectContainer();

  public quoteReg: RegExp = new RegExp(/"([^"]+)"/g);
  public tagReg: RegExp = new RegExp(/"tag:([^"]+)"/g);
  public eventReg: RegExp = new RegExp(/"event:([^"]+)"/g);
  public statusReg: RegExp = new RegExp(/"status:([^"]+)"/g);
  public nameReg: RegExp = new RegExp(/"name:([^"]+)"/g);
  public nicknamesReg: RegExp = new RegExp(/"nickname:([^"]+)"/g);

  public get allHelperTags() : Array<Tag>
  {
    let allowedTagArray: Array<Tag> = [];
    this.allTags.forEach((object: Tag) => {
      // If the query object does not already have that in the list
      if(!this.searchQueryObjectHolder.tags.some((tag: Tag) => {return tag.tagID == object.tagID}))
      {
        allowedTagArray.push(object);
      }
    });
    return allowedTagArray;
  }
  public get allHelperStatuses() : Array<Status>
  {
    let allowedStatusArray: Array<Status> = [];
    this.allClientStatuses.forEach((object: Status) => {
      // If the query object does not already have that in the list
      if(!this.searchQueryObjectHolder.statuses.some((status: Status) => {return status.statusID == object.statusID}))
      {
        // Push the object into the array
        allowedStatusArray.push(object);
      }
    });
    return allowedStatusArray;
  }
  public get allHelperEvents() : Array<EventType>
  {
    let allowedEventArray: Array<EventType> = [];
    this.allEvents.forEach((object: EventType) => {
      // If the query object does not already have that in the list
      if(!this.searchQueryObjectHolder.events.some((event: EventType) => {return event.eventTypeID == object.eventTypeID}))
      {
        // Push the object into the array
        allowedEventArray.push(object);
      }
    });
    return allowedEventArray;
  }

  async ngOnInit() {
    /* Gathering status subscribes */
    this.gatherer.gatheringAllTags.subscribe((status: boolean) => {
      this.gatheringAllTags = status;
    });
    this.gatherer.gatheringAllStatuses.subscribe((status: boolean) => {
      this.gatheringAllClientStatuses = status;
    });
    this.gatherer.gatheringAllEvents.subscribe((status: boolean) => {
      this.gatheringAllEvents = status;
    });

    /* Default set so they all "load" at the same time */
    this.gatheringAllTags = true;
    this.gatheringAllClientStatuses = true;
    this.gatheringAllEvents = true;

    /* Gathering data subscribes */
    this.gatherer.allTags.subscribe((objectArray: Array<Tag>) => {
      this.allTags = objectArray;
    });
    this.gatherer.allStatuses.subscribe((objectArray: Array<Status>) => {
      this.allClientStatuses = objectArray;
    });
    this.gatherer.allEvents.subscribe((objectArray: Array<EventType>) => {
      this.allEvents = objectArray;
    });

    await this.gatherer.allGather();
  }
  addQuery(helper: any)
  {

    if(helper.tagID != undefined)
    {
      this.searchQueryString += '"tag:' + (<Tag>helper).tagName + '" ';
    }
    else if(helper.statusID != undefined)
    {
      this.searchQueryString += '"status:' + (<Status>helper).statusDescription + '" ';
    }
    else if(helper.eventTypeID != undefined)
    {
      this.searchQueryString += '"event:' + (<EventType>helper).eventDescription + '" ';
    }
    this.constructQueryObject();
  }
  constructQueryObject()
  {
    this.clearHolderObjects();
    let tagMatches = Array.from(this.searchQueryString.matchAll(this.tagReg))
    let eventMatches = Array.from(this.searchQueryString.matchAll(this.eventReg))
    let statusMatches = Array.from(this.searchQueryString.matchAll(this.statusReg))
    let nameMatches = Array.from(this.searchQueryString.matchAll(this.nameReg))
    let nicknameMatches = Array.from(this.searchQueryString.matchAll(this.nicknamesReg))


    tagMatches.forEach((element: RegExpMatchArray) => {
      // If allTags has a tag that equals the regex element group, and the searchable query object holder doesnt yet have that tag if so
      if(this.allTags.some((tag: Tag) => {return tag.tagName == element[1]}) && !this.searchQueryObjectHolder.tags.some((tag: Tag) => {return tag.tagName == element[1]}))
      {
        // Find and add the tag to the object holder
        this.searchQueryObjectHolder.tags.push(this.allTags.find((tag: Tag) => {return tag.tagName == element[1]}));
      }
    });
    eventMatches.forEach((element: RegExpMatchArray) => {
      // If allEvents has a event that equals the regex element group, and the searchable query object holder doesnt yet have that event if so
      if(this.allEvents.some((event: EventType) => {return event.eventDescription == element[1]}) && !this.searchQueryObjectHolder.events.some((event: EventType) => {return event.eventDescription == element[1]}))
      {
        // Find and add the event to the object holder
        this.searchQueryObjectHolder.events.push(this.allEvents.find((event: EventType) => {return event.eventDescription == element[1]}));
      }
    });
    statusMatches.forEach((element: RegExpMatchArray) => {
      // If allEvents has a event that equals the regex element group, and the searchable query object holder doesnt yet have that event if so
      if(this.allClientStatuses.some((status: Status) => {return status.statusDescription == element[1]}) && !this.searchQueryObjectHolder.statuses.some((status: Status) => {return status.statusDescription == element[1]}))
      {
        // Find and add the event to the object holder
        this.searchQueryObjectHolder.statuses.push(this.allClientStatuses.find((status: Status) => {return status.statusDescription == element[1]}));
      }
    });
    // Same thing as above but with strings. These do not have helper objects so they dont need the first conditional to be pushed into the list
    nicknameMatches.forEach((element: RegExpMatchArray) => {
      if(!this.searchQueryObjectHolder.nicknames.some((nickname: string) => {return nickname == element[1]}))
      {
        this.searchQueryObjectHolder.nicknames.push(element[1]);
      }
    });
    nameMatches.forEach((element: RegExpMatchArray) => {
      if(!this.searchQueryObjectHolder.names.some((name: string) => {return name == element[1]}))
      {
        this.searchQueryObjectHolder.names.push(element[1]);
      }
    });
  }
  public removeTagQuery(tag: Tag)
  {
    this.searchQueryObjectHolder.tags.splice(this.searchQueryObjectHolder.tags.indexOf(tag), 1);
    this.searchQueryString = this.searchQueryString.replace('"tag:'+ tag.tagName + '" ', "")
  }
  public removeEventQuery(event: EventType)
  {
    this.searchQueryObjectHolder.events.splice(this.searchQueryObjectHolder.events.indexOf(event), 1);
    this.searchQueryString = this.searchQueryString.replace('"event:'+ event.eventDescription + '" ', "")
  }
  public removeStatusQuery(status: Status)
  {
    this.searchQueryObjectHolder.statuses.splice(this.searchQueryObjectHolder.statuses.indexOf(status), 1);
    this.searchQueryString = this.searchQueryString.replace('"status:'+ status.statusDescription + '" ', "")
  }
  public clearHolderObjects()
  {
    this.searchQueryObjectHolder.tags = [];
    this.searchQueryObjectHolder.events = [];
    this.searchQueryObjectHolder.statuses = [];
    this.searchQueryObjectHolder.names = [];
    this.searchQueryObjectHolder.nicknames = [];
  }
  public search()
  {
    this.searchingClients = true;
    this.foundClients = [];
    let response: SearchQueryModelResponse =
    {
      tags: this.searchQueryObjectHolder.tagQueryIDs,
      statuses: this.searchQueryObjectHolder.statusesQueryIDs,
      events: this.searchQueryObjectHolder.eventQueryIDs,
      names: this.searchQueryObjectHolder.names,
      nicknames: this.searchQueryObjectHolder.nicknames,
      isHardSearch: this.selectedSearchType == 'hard' ? true : false
    }
    this.santaApiPost.searchClients(response).subscribe((res) => {

      res.forEach(client => {
        this.foundClients.push(this.mapper.mapClient(client));
      });
      this.searchingClients = false;
    },err => {
      console.group();
      console.log("Something went wrong searching clients!");
      console.log(err);
      console.groupEnd();

      this.searchingClients = false;
    });
  }
  public updateSelectedClient()
  {

  }
  public showCardInfo(client: Client)
  {
    this.selectedClient = client;
    this.showClientCard = true;
  }
  public hideOpenWindow()
  {
    if(!this.clickLocked)
    {
      this.showClientCard = false;
      this.selectedClient = new Client();
    }
  }
  test()
  {
    console.log(this.selectedSearchType);
  }
}