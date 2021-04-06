import { Component, OnInit } from '@angular/core';
import {getBaseUrl} from "../helpers";


declare const SwaggerUIBundle: any;

@Component({
    selector: 'app-swagger-ui',
    templateUrl: './swagger-ui.component.html',
    styleUrls: ['./swagger-ui.component.css']
})
export class SwaggerUiComponent implements OnInit {

    baseHref: string = getBaseUrl();
    
    ngOnInit(): void {
        const ui = SwaggerUIBundle({
            dom_id: '#swagger-ui',
            layout: 'BaseLayout',
            presets: [
                SwaggerUIBundle.presets.apis,
                SwaggerUIBundle.SwaggerUIStandalonePreset
            ],
            url: this.baseHref + 'swagger/v1/swagger.json',
            docExpansion: 'none',
            operationsSorter: 'alpha'
        });
    }
}