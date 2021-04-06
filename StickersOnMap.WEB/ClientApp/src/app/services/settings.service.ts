import {urls} from "../infrastructure/urls";
import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable, of} from "rxjs";
import {catchError, map, tap} from "rxjs/operators";
import {AppSettings} from "../infrastructure/models/app-settings.model";


@Injectable({
    providedIn: 'root'
})
export class SettingsService {

    private _settings: AppSettings;

    private _uriMap: string;

    get settings() {
        if (!this.initialized) {
            throw new Error('service has not been initialized');
        }
        return this._settings;
    }

    get uriMap() {
        if (!this.initialized) {
            throw new Error('service has not been initialized');
        }
        return this._uriMap;
    }

    get initialized(): boolean {
        return !!this._settings;
    }

    constructor(private httpClient: HttpClient) {
    }

    init(): Observable<boolean> {
        if (this.initialized) {
            return of(true);
        }

        return this.httpClient.get<AppSettings>(urls.settings)
            .pipe(tap(settings => {
                this._settings = settings;
                this._uriMap = this.normalizeUrl(settings.uriMap);
            }),
            catchError(_ => of(null)),
            map(s => !!s));
    }

    private normalizeUrl(url: string): string {
        if (!url || url.length < 1) {
            throw new Error('provide valid url for normalization');
        }
        return url.endsWith('/') ? url.slice(0, url.length - 1) : url;
    }
}
