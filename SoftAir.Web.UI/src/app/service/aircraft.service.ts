import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AircraftModel } from '../model/aircraft.model';
import { HttpClientService } from '../shared/http-client/http-client.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftService {

  constructor(private http: HttpClient, private router: Router) {
  }

  getAircraft() {
    return this.http.get<AircraftModel[]>(HttpClientService.GET_AIRCRAFT_CONTROLLER);
  }
  
  createAircraft(data: AircraftModel) {
    return this.http.post(HttpClientService.CREATE_AIRCRAFT_CONTROLLER, data);
  }

  updateAircraft(data: AircraftModel) {
    return this.http.put(HttpClientService.UPDATE_AIRCRAFT_CONTROLLER, data);
  }

}
