import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ITodo } from '../models/todo';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TodoService {
  constructor(private httpClient: HttpClient) {}

  getTodos(): Observable<ITodo[]> {
    return this.httpClient.get<ITodo[]>(`${environment.todoServiceUrl}todos`);
  }

  createTodo(todo: ITodo): Observable<ITodo> {
    return this.httpClient.post<ITodo>(`${environment.todoServiceUrl}todos`, todo);
  }
}
