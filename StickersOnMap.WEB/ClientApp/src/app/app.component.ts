import {Component, OnInit} from '@angular/core';
import {PrimeNGConfig} from "primeng/api";
import {translation} from "./infrastructure/constants";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(private config: PrimeNGConfig) {
  }

  ngOnInit() {
    this.config.setTranslation(translation);
  }
}
