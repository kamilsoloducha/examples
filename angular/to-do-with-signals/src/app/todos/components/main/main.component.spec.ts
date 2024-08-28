import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MainComponent } from './main.component';
import { TodosService } from '../../services/todo.service';
import { By } from '@angular/platform-browser';

describe('MainComponent', () => {
  let fixture: ComponentFixture<MainComponent>;
  let todosService: jasmine.SpyObj<TodosService>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainComponent],
      providers: [
        {
          provide: TodosService,
          useValue: jasmine.createSpyObj<TodosService>('todos', [
            'todosSig',
            'filterSig',
            'toggleAll',
          ]),
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MainComponent);
    todosService = TestBed.inject(TodosService) as jasmine.SpyObj<TodosService>;
  });

  it('should be created', () => {
    expect(fixture).toBeTruthy();
  });

  it('should display todos', () => {
    todosService.todosSig.and.returnValue([
      { id: 'test1', isCompleted: false, text: 'test' },
      { id: 'test2', isCompleted: false, text: 'test' },
    ]);
    fixture.detectChanges();

    const todos = fixture.debugElement.queryAll(By.css('app-todos-todo'));
    expect(todos.length).toBe(2);
  });

  it('should call toggleAll after checkbox click', () => {
    todosService.todosSig.and.returnValue([
      { id: 'test1', isCompleted: false, text: 'test' },
    ]);
    fixture.detectChanges();

    const checkbox = fixture.debugElement.query(By.css('input'));
    checkbox.nativeElement.click();

    expect(todosService.toggleAll).toHaveBeenCalledTimes(1);
  });
});
