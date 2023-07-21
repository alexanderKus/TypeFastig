import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Model } from '../models/model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy {
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
  textModel: Model = new Model(this.text);
  isGameStarted: boolean = true;
  writtenText: string = '';
  writtenTextModel: Model | undefined;
  currentWordIndex: number = 0;
  errorCount: number = 0;
  allLetterCount = this.textModel.length;
  interval: any;
  startedAt: number = 0;
  speed: number = 0;

  @HostListener('document:keypress', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (this.isGameStarted) {
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

  ngOnInit(): void {
    this.startedAt = new Date().getTime();
    this.interval = setInterval(() => {
      this.calculateSpeed();
    }, 1000);
  }

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }
  private calculateSpeed() {
    const now = new Date().getTime();
    const numerator = Math.abs(this.currentWordIndex / 5 - this.errorCount);
    const denominator = (now - this.startedAt) / 1000 / 60;
    this.speed = Math.ceil(numerator / denominator);
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
