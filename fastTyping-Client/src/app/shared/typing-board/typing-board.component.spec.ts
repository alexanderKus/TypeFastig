import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypingBoardComponent } from './typing-board.component';

describe('TypingBoardComponent', () => {
  let component: TypingBoardComponent;
  let fixture: ComponentFixture<TypingBoardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TypingBoardComponent]
    });
    fixture = TestBed.createComponent(TypingBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
