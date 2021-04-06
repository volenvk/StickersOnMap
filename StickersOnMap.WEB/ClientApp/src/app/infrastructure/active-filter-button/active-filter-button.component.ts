import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-active-filter-button',
  template: `
    <button pButton
            class="btn-filter p-button-outlined p-button-rounded p-button-sm"
            label="{{ displayName | uppercase }}"
            icon="pi pi-times"
    >
    </button>
  `,
  styles: [
      `
      button {
        font-size: 0.8rem !important;
      }

      .btn-filter {
        animation-name: fadein;
        animation-duration: 0.3s;
        padding-top: 0.25rem;
        padding-bottom: 0.25rem;
      }

      @keyframes fadein {
        from {
          opacity: 0;
        }
        to {
          opacity: 1;
        }
      }
    `
  ]
})
export class ActiveFilterButtonComponent {
  @Input() displayName: string;
}
