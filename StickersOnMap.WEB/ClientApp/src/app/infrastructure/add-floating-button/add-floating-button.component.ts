import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-add-floating-button',
  template: `
    <div class="floating-button-wrapper">
      <button pButton type="button" icon="pi pi-plus"
              class="p-button-rounded floating-button"
              (click)="add.emit()" [disabled]="disabled"></button>
    </div>
  `,
  styles: [
      `
      .floating-button-wrapper {
        position: fixed;
        right: 30px;
        bottom: 30px;
        font-size: 2rem;
      }

      .floating-button {
        width: 4rem !important;
        height: 4rem !important;
        box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.25);
      }
    `
  ]
})
export class AddFloatingButtonComponent {
  @Input() disabled = false;
  @Output() add: EventEmitter<void> = new EventEmitter();
}
