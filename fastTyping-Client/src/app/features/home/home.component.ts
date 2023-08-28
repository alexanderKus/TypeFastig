import {
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Model } from '../models/model';
import { UserService } from 'src/app/core/services/user.service';
import { Score } from 'src/app/core/models/score.model';
import { TokenService } from 'src/app/core/services/token.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnDestroy {
  /*
  private text: string = `static void *proc_keys_start(struct seq_file *p, loff_t *_pos)
    __acquires(key_serial_lock) 
{
  key_serial_t pos = *_pos;
	struct key *key;
  spin_lock(&key_serial_lock);
  if (*_pos > INT_MAX)
    return NULL;
  key = find_ge_key(p, pos);
  if (!key)
    return NULL;
  *_pos = key->serial;
  return &key->serial_node;
}`;
*/
  isGameStarted: boolean = false;
  isEndDialogShown: boolean = false;

  text: string = 'siema';
  textModel: Model = new Model(this.text);

  writtenText: string = '';
  writtenTextModel: Model | undefined;

  currentWordIndex: number = 0;
  interval: any;
  startedAt: number = 0;

  errorCount: number = 0;
  speed: number = 0;
  bestSpeed: number = 0;
  accuracy: number = 0;

  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>;

  @HostListener('document:keypress', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (this.isEndDialogShown) {
      return;
    } else if (!this.isGameStarted) {
      this.init();
    }
    let letter = this.textModel.getLetterFromIndex(this.currentWordIndex);
    let isLetter = this.textModel.isLetterFromIndex(this.currentWordIndex);
    if (event.key === ' ' && isLetter) {
      this.writtenText += '.';
    } else if (event.key === 'Enter') {
      this.writtenText += '\n';
    } else {
      this.writtenText += event.key;
    }
    if (event.key !== letter) {
      this.errorCount++;
    }
    this.writtenTextModel = new Model(this.writtenText);
    this.currentWordIndex++;
    this.accuracy =
      ((this.writtenText.length - this.errorCount) / this.writtenText.length) *
      100;
    if (this.currentWordIndex == this.text.length) {
      this.isGameStarted = false;
      this.isEndDialogShown = true;
      this.dialog.nativeElement.showModal();
      if (this.tokenService.isTokenValid) {
        const score: Score = {
          UserId: this.tokenService.userId!,
          Speed: this.bestSpeed,
          Accuracy: this.accuracy,
        };
        //TODO!: uncomment this
        //this.userService.addScore(score).subscribe();
      }
      // FIXME: reset colors
    }
  }

  @HostListener('document:keydown.backspace', ['$event'])
  handleBackSpaceEvent(event: KeyboardEvent) {
    if (this.isGameStarted && this.writtenText.length > 0) {
      this.writtenText = this.writtenText.slice(0, this.writtenText.length - 1);
      this.writtenTextModel = new Model(this.writtenText);
    }
    if (this.currentWordIndex > 0) {
      this.currentWordIndex--;
    }
  }

  constructor(
    private userService: UserService,
    private tokenService: TokenService
  ) {}

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  closeDialog(): void {
    this.dialog.nativeElement.close();
    this.reset();
  }

  private init(): void {
    console.log('Set `isGameStarted` to true');
    this.isGameStarted = true;
    this.startedAt = new Date().getTime();
    this.interval = setInterval(() => {
      this.calculateSpeed();
    }, 100);
  }

  private reset(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
    this.isGameStarted = false;
    this.errorCount = 0;
    this.currentWordIndex = 0;
    this.bestSpeed = 0;
    this.speed = 0;
  }

  private calculateSpeed(): void {
    if (!this.isEndDialogShown) {
      const now = new Date().getTime();
      const numerator = Math.abs(this.currentWordIndex / 5 - this.errorCount);
      const denominator = (now - this.startedAt) / 1000 / 60;
      let currentSpeed = Math.ceil(numerator / denominator);
      this.bestSpeed = Math.max(this.bestSpeed, currentSpeed);
      this.speed = currentSpeed;
    }
  }

  public getClass(
    letter: string,
    lineIndex: number,
    wordIndex: number,
    letterIndex: number
  ): string {
    if (!this.exists(lineIndex, wordIndex, letterIndex)) {
      return 'neutral-letter';
    }
    if (this.isCorrect(letter, lineIndex, wordIndex, letterIndex)) {
      return 'correct-letter';
    } else {
      return 'wrong-letter';
    }
  }

  public exists(
    lineIndex: number,
    wordIndex: number,
    letterIndex: number
  ): boolean {
    let letter =
      this.writtenTextModel?.lines[lineIndex]?.line[wordIndex]?.letter[
        letterIndex
      ];
    return letter !== undefined;
  }

  public isCorrect(
    letter: string,
    lineIndex: number,
    wordIndex: number,
    letterIndex: number
  ): boolean {
    return (
      this.writtenTextModel?.lines[lineIndex]?.line[wordIndex]?.letter[
        letterIndex
      ] == letter
    );
  }
}
