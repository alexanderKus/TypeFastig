import { Language } from '../features/models/language.enum';
import { CodeTexts } from '../shared/code-texts/codeTexts';

export class TextEditor {
  public text: string = '';
  public Language?: Language;

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

  public static createNewRandomText(lang: Language | undefined): TextEditor {
    if (lang === undefined) {
      if (Math.random() < 0.5) {
        lang = Language.C;
      } else {
        lang = Language.PYTHON;
      }
    }
    let te = new TextEditor(CodeTexts.getText(lang));
    te.Language = lang;
    return te;
  }
}
