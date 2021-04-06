import {Component, Input} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-message-page',
  templateUrl: './message-page.component.html',
  styleUrls: ['./message-page.component.scss']
})
export class MessagePageComponent {
  @Input() header: string;
  @Input() body: string;
  @Input() redirectBtn: boolean;

  constructor(private router: Router) {
  }

  navigate() {
    this.router.navigate(['/']);
  }
}
