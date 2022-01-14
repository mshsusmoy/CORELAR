import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Status } from '../_models/status';
import { User } from '../_models/user';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private statusThreadSource = new BehaviorSubject<Status[]>([]);
  statusThread$ = this.statusThreadSource.asObservable();
  user: User;
  
  constructor(private http: HttpClient,private busyService: BusyService) { 
  }

  createHubConnection(user: User){
    this.hubConnection  = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'status', {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection
    .start()
    .catch(error => console.log(error));

    this.hubConnection.on('StatusAll', statuses => {
      this.statusThreadSource.next(statuses);
    })

    this.hubConnection.on('NewStatus', status => {
      this.statusThread$.pipe(take(1)).subscribe(statuses => {
        this.statusThreadSource.next([...statuses, status]);
      })
    })

    this.hubConnection.on('NewStatusPosted', ({userName, knownAs}) => {
      this.getStatuses();
    })
  }

  stopHubConnection(){
    if(this.hubConnection){
      this.statusThreadSource.next([]);
      this.hubConnection.stop();
    }
  }

  getStatuses(){
    return this.http.get<Status[]>(this.baseUrl + 'status/get-statuses');
  }

  async postStatus(content: string){
    return this.hubConnection.invoke('CreateStatus', {content})
    .catch(error => console.log(error));
  }
}
