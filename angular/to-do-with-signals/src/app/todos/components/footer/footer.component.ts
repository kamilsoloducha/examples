import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterEnum } from '../../types/filter.enum';
import { TodosService } from '../../services/todo.service';

@Component({
  selector: 'app-todos-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.component.html',
})
export class FooterComponent {
  todosService = inject(TodosService);

  filterSig = this.todosService.filterSig;
  filterEnum = FilterEnum;

  activeCountSig = computed(() => {
    return this.todosService.todosSig().filter((x) => !x.isCompleted).length;
  });

  noTodosSig = computed(() => {
    return this.todosService.todosSig().length === 0;
  });

  itemsLeftText = computed(
    () => `item${this.activeCountSig() !== 1 ? 's' : ''} left`
  );

  changeFilter(event: Event, filterName: FilterEnum): void {
    event.preventDefault();
    this.todosService.changeFilter(filterName);
  }
}
