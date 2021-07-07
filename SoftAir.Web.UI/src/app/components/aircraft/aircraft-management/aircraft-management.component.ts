import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AircraftModel } from 'src/app/model/aircraft.model';
import { AircraftService } from 'src/app/service/aircraft.service';
import { ToastService } from 'src/app/service/toast.service';

@Component({
  selector: 'app-aircraft-management',
  templateUrl: './aircraft-management.component.html',
  styleUrls: ['./aircraft-management.component.scss']
})
export class AircraftManagementComponent implements OnInit {

  public aircraft: AircraftModel = new AircraftModel();
  public aircrafts?: AircraftModel[];

  constructor(private _aircraftService: AircraftService,
    private _toastService: ToastService,
    private _router: Router,) { }

  ngOnInit(): void {
  }

  createAircraft(): void {
      this._aircraftService.createAircraft(this.aircraft).subscribe((respData: AircraftModel) => {
        this.aircrafts?.push(respData);
        this._toastService.showSuccess('Changed');
        this._router.navigate(['/']);
      })
  }

  cancel() {
    this._toastService.showSuccess('Changed');
    this._router.navigate(['/']);
  }
}

