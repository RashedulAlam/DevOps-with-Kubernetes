import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { TodoService } from '../../services/todo.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-create-todo',
  templateUrl: './create-todo.component.html',
  styleUrl: './create-todo.component.css',
})
export class CreateTodoComponent implements OnInit {
  todoForm: FormGroup = this.formBuilder.group({});
  @Output() onCreate = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
    public todoService: TodoService
  ) {}

  ngOnInit(): void {
    this.todoForm = this.formBuilder.group({
      title: new FormControl('', [
        Validators.required,
        Validators.maxLength(120),
      ]),
    });
  }

  get ImageUrl(): string {
    return `${environment.todoServiceUrl}find-image`
  }

  createTodo(): void {
    if (!this.todoForm.valid) {
      alert('Invalid Title Length');

      return;
    }

    this.todoService.createTodo(this.todoForm.value).subscribe(() => {
      alert('Successfully created');
      this.todoForm.reset();
      this.onCreate.emit();
    });
  }
}
