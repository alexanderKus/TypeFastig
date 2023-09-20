import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { NgxSpinnerService } from 'ngx-spinner';
import { PlayerStatus } from '../models/playerStatus.model';
import { UserService } from 'src/app/core/services/user.service';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  public stats$ = new BehaviorSubject<PlayerStatus[]>([]);
  public isStarted$ = new BehaviorSubject<boolean>(false);
  private roomId: number | undefined = undefined;
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
      // ONly for testing
      this.isStarted$.next(true);
      this.roomId = roomId;
    });

    hubConnection.on('UpdateStats', (stats) => {
      this.stats$.next(stats);
    });

    hubConnection.on('StartRoom', () => {
      this.isStarted$.next(true);
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
      this.hubConnection.stop();
      this.isStarted$.next(false);
      this.roomId = undefined;
    }
  }

  public async sendStats(stats: PlayerStatus): Promise<void> {
    if (this.roomId != undefined) {
      this.hubConnection.invoke('SendStats', this.roomId, stats);
    }
  }
}
