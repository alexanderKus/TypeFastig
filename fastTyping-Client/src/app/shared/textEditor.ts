import { texts } from './code-texts/codeTexts';

export class TextEditor {
  public text: string = '';

  constructor(text: string) {
    this.text = text;
  }

  public get lines() {
    return this.text.split('\n');
  }

  public parseIntoArrayOfChar(): string[] {
    return this.text.split('');
  }

  public getCharAtIndex(index: number): string {
    if (index < 0) {
      return '';
    } else if (index > this.text.length) {
      return this.text[this.text.length - 1];
    } else {
      return this.text[index];
    }
  }

  public static createNewRandomText(): TextEditor {
    let randomIndex = Math.floor(Math.random() * texts.size);
    return new TextEditor(texts.get(randomIndex)!);
  }
}
