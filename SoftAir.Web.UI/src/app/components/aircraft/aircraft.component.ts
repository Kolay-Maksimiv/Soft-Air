import { Component, OnInit } from '@angular/core';
import { AircraftModel } from 'src/app/model/aircraft.model';
import { AircraftService } from 'src/app/service/aircraft.service';

@Component({
  selector: 'app-aircraft',
  templateUrl: './aircraft.component.html',
  styleUrls: ['./aircraft.component.scss']
})
export class AircraftComponent implements OnInit {

  constructor(private _aircraftService: AircraftService) { }

  public _aircrafts?: AircraftModel[];

  ngOnInit() {
    this.getAircrafts();
  }

  getAircrafts() {
    this._aircraftService.getAircraft().subscribe((respData: AircraftModel[]) => {
      this._aircrafts = respData;
      
      console.log(this._aircrafts)
    })
  }
  
}
