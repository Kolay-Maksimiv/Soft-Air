import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientService } from './shared/http-client/http-client.service';

//Components
import { AppComponent } from './app.component';
import { AircraftComponent } from './components/aircraft/aircraft.component'
import { AircraftManagementComponent } from './components/aircraft/aircraft-management/aircraft-management.component'


@NgModule({
  declarations: [
    AppComponent,
    AircraftComponent,
    AircraftManagementComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [HttpClientService],
  bootstrap: [AppComponent]
})
export class AppModule { }
