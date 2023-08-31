import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { NgxSpinnerService } from 'ngx-spinner';
import { PlayerStatus } from '../models/playerStatus.model';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  // Maybe observable is not needed.
  // TODO: give it correct type
  public stats$ = new BehaviorSubject<PlayerStatus[]>([]);
  public isStarted$ = new BehaviorSubject<boolean>(false); // TODO: add $ at the end
  private roomId: number | undefined = undefined;
  private hubConnection = this.createHubConnection();

  constructor(private spinner: NgxSpinnerService) {}

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
    // TODO: send correct username is logged in if not figure it out
    await this.hubConnection.invoke('JoinRoom', 'player1');
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
