import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {urls} from "../infrastructure/urls";
import {Observable} from "rxjs";
import {GeoData} from "../infrastructure/models/geo-data.model";


@Injectable({
    providedIn: 'root'
})

export class GeoDataService {
    constructor(private httpClient: HttpClient) {
    }

  fetchAll(): Observable<GeoData[]> {
    return this.httpClient.get<GeoData[]>(`${urls.geoData}/`);
  }
}
