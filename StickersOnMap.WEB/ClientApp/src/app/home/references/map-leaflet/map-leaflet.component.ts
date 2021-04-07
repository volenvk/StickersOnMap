import {AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, Output} from '@angular/core';
import {icon, LatLng, latLng, LatLngBoundsExpression, Layer, Map, marker, tileLayer} from 'leaflet';
import {ConfirmationService, MessageService} from "primeng/api";
import {Title} from "@angular/platform-browser";
import {GeoDataService} from "../../../services/geo-data.service";
import {getBaseUrl} from "../../../infrastructure/helpers";
import {SettingsService} from "../../../services/settings.service";
import {interval} from "rxjs";
import {GeoData} from "../../../infrastructure/models/geo-data.model";

@Component({
  selector: 'app-map-leaflet',
  templateUrl: './map-leaflet.component.html',
  styleUrls: ['./map-leaflet.component.scss']
})
export class MapLeafletComponent implements AfterViewInit {

  constructor(
      private geoDataService: GeoDataService,
      private settingsService: SettingsService,
      protected messageService: MessageService,
      protected confirmationService: ConfirmationService,
      protected cdRef: ChangeDetectorRef,
      title: Title
  ) {
    title.setTitle('Карта');
  }

  map: Map;
  activeLayers: Layer[] = [];
  center: LatLng = latLng(56.8384, 60.6033);
  defaultZoom = 15;
  baseHref: string = getBaseUrl();

  options = {
    layers: [
      tileLayer(this.settingsService.uriMap,
        {
          maxZoom: 18
        })
    ],
    zoom: this.defaultZoom,
    center: this.center
  };

  @Input() readonly = true;
  @Output() point: EventEmitter<{ lat: number, lon: number }> =
    new EventEmitter<{ lat: number, lon: number }>();

  onMapReady(map: Map) {
    this.map = map;
  }

  ngAfterViewInit(): void {
    this.subscribeData();
    interval(60000).subscribe(() => this.subscribeData());
    this.cdRef.detectChanges();
  }

  private subscribeData() : void {
    this.geoDataService.fetchAll().subscribe((data: GeoData[]) => {
      this.mergeMarker(data);
    });
  }

  private mergeMarker(geoData: GeoData[]) : void {
    if (geoData) {
      geoData.forEach(geoData => {
        this.addMarkerAt(geoData.name, geoData.latitude, geoData.longitude);
      })}
    }

  resetView() {
    this.map.setView(this.center, this.defaultZoom);
  }

  private addMarkerAt(title: string, lat: number, lon: number): Layer {
    const layer = this.createLayerWithMarkerAt(lat, lon);
    layer.bindPopup(title);
    this.activeLayers.push(layer);
    this.map.addLayer(layer);
    this.map.setView(latLng(lat, lon), this.map.getZoom());
    return layer;
  }

  private deleteMarker(marker: Layer){
    this.map.removeLayer(marker)
  }

  private clearMarkers() {
    this.activeLayers.forEach(l => this.map.removeLayer(l));
    this.activeLayers = [];
  }

  onMapClick(event): void {
    const lat = event.latlng.lat;
    const lon = event.latlng.lng;
    const name = `широта:${lat.toFixed(3)}, долгота:${lon.toFixed(3)}`;
    this.addMarkerAt(name, lat, lon);
    this.point.emit({lat, lon});
    this.geoDataService.create(new GeoData(name, lat, lon)).subscribe();
  }

  private createLayerWithMarkerAt(lat: number, lon: number, xSize: number = 25, ySize: number = 41): Layer {
    return marker([lat, lon],
      {
        title: '',
        icon: icon({
          iconSize: [xSize, ySize],
          iconAnchor: [xSize / 2, ySize],
          popupAnchor: [1, -34],
          shadowSize: [xSize, ySize],
          iconUrl: this.baseHref + `assets/truck.png`
          //shadowUrl: this.baseHref + 'assets/truck-shadow.png'
        })
      });
  }

}
