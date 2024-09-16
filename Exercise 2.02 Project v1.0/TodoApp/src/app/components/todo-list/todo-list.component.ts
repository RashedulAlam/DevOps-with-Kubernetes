import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { TodoService } from '../../services/todo.service';
import { ITodo } from '../../models/todo';
import { BehaviorSubject, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrl: './todo-list.component.css',
})
export class TodoListComponent implements OnInit, OnDestroy {
  @Input() refresh$: BehaviorSubject<boolean> | undefined;
  todos: ITodo[] = [];
  private subscription: Subscription | undefined;

  constructor(private todoService: TodoService) {}

  ngOnInit(): void {
    if (this.refresh$) {
      this.subscription = this.refresh$
        .pipe(switchMap(() => this.todoService.getTodos()))
        .subscribe((items) => {
          this.todos = items;
        });
    }
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
