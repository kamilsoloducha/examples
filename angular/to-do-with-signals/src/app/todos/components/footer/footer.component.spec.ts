import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FooterComponent } from './footer.component';
import { TodosService } from '../../services/todo.service';
import { By } from '@angular/platform-browser';
import { FilterEnum } from '../../types/filter.enum';

describe('FooterComponent', () => {
  let fixture: ComponentFixture<FooterComponent>;
  let todosService: jasmine.SpyObj<TodosService>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FooterComponent],
      providers: [
        {
          provide: TodosService,
          useValue: jasmine.createSpyObj<TodosService>('todos', [
            'todosSig',
            'filterSig',
            'changeFilter',
          ]),
        },
      ],
    }).compileComponents();

    todosService = TestBed.inject(TodosService) as jasmine.SpyObj<TodosService>;
    fixture = TestBed.createComponent(FooterComponent);
  });

  it('should be created', () => {
    expect(fixture).toBeTruthy();
  });

  it('should add hidden class when no todos', () => {
    todosService.todosSig.and.returnValue([]);

    fixture.detectChanges();

    const footer = fixture.debugElement.query(By.css('footer.hidden'));

    expect(footer).toBeTruthy();
  });

  it('should not add hidden class when any toods', () => {
    todosService.todosSig.and.returnValue([
      { id: 'test', isCompleted: true, text: 'test' },
    ]);

    fixture.detectChanges();

    const footer = fixture.debugElement.query(By.css('footer.hidden'));

    expect(footer).toBeFalsy();
  });

  it('should dispaly active count', () => {
    todosService.todosSig.and.returnValue([
      { id: 'test', isCompleted: false, text: 'test' },
    ]);

    fixture.detectChanges();

    const countEl = fixture.debugElement.query(By.css('strong'));

    expect(countEl.nativeElement.innerHTML).toContain('1');
  });

  it('should add selected class when a is selected', () => {
    todosService.todosSig.and.returnValue([
      { id: 'test', isCompleted: false, text: 'test' },
    ]);
    todosService.filterSig.and.returnValue(FilterEnum.active);
    fixture.detectChanges();

    const activeEl = fixture.debugElement.query(By.css('a.selected'));
    expect(activeEl.nativeElement.innerHTML).toContain('Active');
  });

  it('should display text depending on active count', () => {
    todosService.todosSig.and.returnValue([
      { id: 'test1', isCompleted: false, text: 'test' },
      { id: 'test2', isCompleted: false, text: 'test' },
    ]);
    fixture.detectChanges();

    const countEl = fixture.debugElement.query(By.css('span'));
    expect(countEl.nativeElement.innerHTML).toContain('items left');
  });

  it('should react on a click', () => {
    todosService.todosSig.and.returnValue([
      { id: 'test1', isCompleted: false, text: 'test' },
    ]);
    todosService.filterSig.and.returnValue(FilterEnum.active);

    fixture.detectChanges();

    const firstEl = fixture.debugElement.queryAll(By.css('a'))[0];
    firstEl.nativeElement.click();

    expect(todosService.changeFilter).toHaveBeenCalledOnceWith(FilterEnum.all);
  });
});
