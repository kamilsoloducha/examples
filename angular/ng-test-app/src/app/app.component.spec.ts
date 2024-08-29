import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { getAllMethods } from '../test-utils/mocking';

class Class {
  methodA(): void {}
  methodB(): void {}
}

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
    }).compileComponents();
  });

  fit('should', () => {
    const methods = getAllMethods(Class);
    expect(methods).toEqual(['methodA', 'methodB']);
  });
});
