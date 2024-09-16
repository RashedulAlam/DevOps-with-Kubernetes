import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'TodoApp';
  refresh$ = new BehaviorSubject(true);

  refreshTodos(): void {
    this.refresh$.next(true);
  }
}
