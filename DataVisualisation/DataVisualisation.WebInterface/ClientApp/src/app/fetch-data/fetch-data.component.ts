import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public events: Event[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Event[]>(baseUrl + 'events').subscribe(result => {
      this.events = result;
    }, error => console.error(error));
  }
}

interface Event {
  id: string;
  name: string;
}
