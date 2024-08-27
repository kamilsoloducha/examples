import { Component, inject } from '@angular/core';
import { TodosService } from '../../serivces/todo.service';

@Component({
  selector: 'app-todos-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  todoService = inject(TodosService);

  text: string = '';

  changeText(event: Event): void {
    const target = event.target as HTMLInputElement;

    this.text = target.value;
  }

  addTodo(): void {
    this.todoService.addTodo(this.text);
    this.text = '';
  }
}
