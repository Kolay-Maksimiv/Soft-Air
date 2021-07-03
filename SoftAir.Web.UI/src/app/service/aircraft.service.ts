import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClientService } from '../shared/http-client/http-client.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftService {

  constructor(private http: HttpClient, private router: Router) {
  }

  getAircraft() {
    return this.http.get<any>(HttpClientService.AIRCRAFT_CONTROLLER);
  }

}
