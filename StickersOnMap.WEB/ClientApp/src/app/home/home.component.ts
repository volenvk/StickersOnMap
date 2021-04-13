import {ChangeDetectorRef, Component, ViewChild} from '@angular/core';
import {MenuItem} from "primeng/api";
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  title = 'Карта';

  references: MenuItem[] = this.CreateMenuItem();

  constructor(
      private cdRef: ChangeDetectorRef,
  ) {
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }


  private CreateMenuItem(): MenuItem[] {
    const menu = [];
    menu.push({label: 'Стикеры', routerLink: '/references/stickers'});
    if (!environment.production) {
      menu.push({label: 'API', routerLink: '/references/API'});
    }
    return menu;
  }
}
