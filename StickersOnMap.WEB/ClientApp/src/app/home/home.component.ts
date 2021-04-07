import {ChangeDetectorRef, Component, ViewChild} from '@angular/core';
import {MenuItem} from "primeng/api";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  title = 'Карта';

  references: MenuItem[] = [
    {label: 'Стикеры', routerLink: '/references/stickers'},
    {label: 'API', routerLink: '/references/API'}
  ];

  reports: MenuItem[];

  _linksSortFunc = (left: MenuItem, right: MenuItem): number => {
    return left.label < right.label ? -1 : 1;
  }

  constructor(
      private cdRef: ChangeDetectorRef,
  ) {
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }
}
