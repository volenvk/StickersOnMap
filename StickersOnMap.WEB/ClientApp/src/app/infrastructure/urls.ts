import {getBaseUrl} from "./helpers";
import {environment} from "../../environments/environment";

export interface Urls {
  stickers: string;
  geoData: string;
  settings: string;
}

const jsonServerBase = 'https://localhost:44310/';
const apiBase = environment.useFakeApi ? jsonServerBase : getBaseUrl();

export const urls: Urls = {
    stickers: apiBase + 'api/stickers',
    geoData: apiBase + 'api/map',
    settings: apiBase + 'api/settings'
};

