import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SantaApiDeleteService, SantaApiGetService, SantaApiPostService, SantaApiPutService } from 'src/app/services/santa-api.service';
import { Note } from 'src/classes/note';
import { NewNoteResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-note-control',
  templateUrl: './note-control.component.html',
  styleUrls: ['./note-control.component.css']
})
export class NoteControlComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService) { }

  @Input() clientID: string;
  @Input() notes: Array<Note> = [];

  @Output() postedNewNoteSuccessEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() postedNewNoteFailureEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() editedNoteSuccessEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() editedNoteFailureEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public newNoteFormGroup: FormGroup;
  public get newNoteControls()
  {
    return this.newNoteFormGroup.controls
  }
  get newNoteSubject()
  {
    var control = this.newNoteFormGroup.get('noteSubject') as FormControl
    return control.value;
  }
  get newNoteContents()
  {
    var control = this.newNoteFormGroup.get('noteContents') as FormControl
    return control.value;
  }

  public noteFormGroup: FormGroup;
  public get noteControls()
  {
    return this.noteFormGroup.controls
  }

  public showAll: boolean = false;
  public createNewNoteOpen: boolean = false;

  public postingNewNote: boolean = false;
  public puttingEditedNote: boolean = false;

  public selectedNote: Note = new Note();

  ngOnInit(): void {
    this.noteFormGroup = this.formBuilder.group({});
    this.newNoteFormGroup = this.formBuilder.group({});

    this.addFields();
  }
  addFields()
  {
    this.newNoteFormGroup = this.formBuilder.group({
      noteSubject: ['', [Validators.required, Validators.maxLength(100)]],
      noteContents: ['', [Validators.required, Validators.maxLength(2000)]]
    });
    this.notes.forEach((note: Note) => {
      this.noteFormGroup.addControl(note.noteID, new FormControl('', [Validators.required, Validators.maxLength(2000)]))
    });
  }
  public setSelectedNote(note: Note)
  {
    this.selectedNote = note;
  }
  public closeCreateNewNote()
  {
    this.newNoteFormGroup.reset();
    this.createNewNoteOpen = false;
  }
  getFormControlNameFromNote(note: Note)
  {
    return note.noteID
  }
  submitUpdatedNote()
  {

  }
  submitNewNote()
  {
    this.postingNewNote = true;

    let response: NewNoteResponse =
    {
      clientID: this.clientID,
      noteSubject: this.newNoteSubject,
      noteContents: this.newNoteContents
    };
    console.log(response);

    this.SantaApiPost.postNewClientNote(response).subscribe(() => {
      this.postingNewNote = false;
      this.postedNewNoteSuccessEvent.emit(true);
    }, err => {
      this.postingNewNote = false;
      this.postedNewNoteFailureEvent.emit(true);
      console.group();
      console.log("Something went wrong submitting a new note");
      console.log(err);
      console.groupEnd();
    });
  }
}
