import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { NgxSpinnerService } from 'ngx-spinner';
import { PlayerStatus } from '../models/playerStatus.model';
import { UserService } from 'src/app/core/services/user.service';
import { Language } from '../models/language.enum';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  public stats$ = new BehaviorSubject<PlayerStatus[]>([]);
  public isStarted$ = new BehaviorSubject<boolean>(false);
  public roomLanguage$ = new BehaviorSubject<Language | undefined>(undefined);
  public roomId: number | undefined = undefined;
  private hubConnection = this.createHubConnection();

  constructor(
    private spinner: NgxSpinnerService,
    private userService: UserService
  ) {}

  createHubConnection(): HubConnection {
    this.spinner.show();
    const hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .withAutomaticReconnect()
      .build();

    hubConnection.start().catch((err) => console.log(err));

    hubConnection.on('SetRoomId', (roomId) => {
      this.roomId = roomId;
    });

    hubConnection.on('SetLanguage', (lang) => {
      this.roomLanguage$.next(lang);
    });

    hubConnection.on('UpdateStats', (stats) => {
      this.stats$.next(stats);
    });

    hubConnection.on('StartRoom', (value) => {
      this.isStarted$.next(value);
    });

    this.spinner.hide();

    return hubConnection;
  }

  public async joinRoom(): Promise<void> {
    const nickname = this.userService.getUsername();
    await this.hubConnection.invoke('JoinRoom', nickname);
  }

  public async leaveRoom(): Promise<void> {
    if (this.roomId != undefined) {
      await this.hubConnection.invoke('LeaveRoom', this.roomId);
      this.isStarted$.next(false);
      this.roomLanguage$.next(undefined);
      this.roomId = undefined;
    }
  }

  public async sendStats(stats: PlayerStatus): Promise<void> {
    if (this.roomId != undefined) {
      this.hubConnection.invoke('SendStats', this.roomId, stats);
    }
  }
}
