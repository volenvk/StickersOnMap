import {Component} from '@angular/core';

@Component({
  selector: 'app-error',
  template: `
    <app-message-page [header]="header" [body]="body" [redirectBtn]="true"></app-message-page>
  `
})
export class ErrorComponent {
  header = 'Ошибка сервера';
  body = `К сожалению, произошла техническая ошибка и ваш запрос не может быть обработан. Приносим извинения за доставленные неудобства.
   Пожалуйста, свяжитесь со службой технической поддержки для выяснения обстоятельств, либо повторите попытку позже.`;
}
