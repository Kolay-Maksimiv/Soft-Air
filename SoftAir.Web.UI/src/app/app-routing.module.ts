import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AircraftManagementComponent } from './components/aircraft/aircraft-management/aircraft-management.component';
import { AircraftComponent } from './components/aircraft/aircraft.component';

const routes: Routes = [
  { path:'', component: AircraftComponent },
  { path:'add', component: AircraftManagementComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
