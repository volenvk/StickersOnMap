import {Component} from '@angular/core';

@Component({
  selector: 'app-page-not-found',
  template: `
    <app-message-page [header]="header" [body]="body" [redirectBtn]="true"></app-message-page>
  `
})
export class PageNotFoundComponent {
  header = 'Страница не найдена';
  body = `Запрошенная Вами страница не найдена. Если Вы перешли сюда по ссылке из приложения,
          пожалуйста, сообщите о некорректной ссылке службе технической поддержки.`;
}
