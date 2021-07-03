import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AircraftComponent } from './components/aircraft/aircraft.component';

const routes: Routes = [
  { path:'', component: AircraftComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
