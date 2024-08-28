import { Injectable, signal } from '@angular/core';
import { Todo } from '../types/todo.model';
import { FilterEnum } from '../types/filter.enum';

@Injectable({
  providedIn: 'root',
})
export class TodosService {
  todosSig = signal<Todo[]>([]);
  filterSig = signal<FilterEnum>(FilterEnum.all);

  changeFilter(filter: FilterEnum): void {
    this.filterSig.set(filter);
  }

  addTodo(text: string): void {
    const newTodo: Todo = {
      text,
      isCompleted: false,
      id: Math.random().toString(16),
    };

    this.todosSig.update((current) => [...current, newTodo]);
  }

  changeTodo(id: string, text: string): void {
    this.todosSig.update((todos) => {
      return todos.map((todo) => (todo.id !== id ? todo : { ...todo, text }));
    });
  }

  removeTodo(id: string): void {
    this.todosSig.update((todos) => todos.filter((todo) => todo.id !== id));
  }

  toggleTodo(id: string): void {
    this.todosSig.update((todos) =>
      todos.map((todo) =>
        todo.id !== id ? todo : { ...todo, isCompleted: !todo.isCompleted }
      )
    );
  }

  toggleAll(isCompleted: boolean): void {
    this.todosSig.update((todos) => {
      return todos.map((todo) => {
        return { ...todo, isCompleted };
      });
    });
  }
}
