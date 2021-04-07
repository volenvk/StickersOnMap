import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ServiceBase} from './base.service';
import {urls} from "../infrastructure/urls";
import {Sticker} from "../infrastructure/models/stickers-model";


@Injectable({
    providedIn: 'root'
})

export class StickersService extends ServiceBase<Sticker> {
    constructor(httpClient: HttpClient) {
        super(httpClient, urls.stickers);
    }
}
