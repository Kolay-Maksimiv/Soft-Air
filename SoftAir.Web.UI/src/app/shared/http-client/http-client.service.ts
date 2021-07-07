import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable()
export class HttpClientService {
  private static readonly API: string = environment.apiUrl;
  public static readonly WEB: string = environment.webUrl;

  // Aircraft Controller
  public static readonly AIRCRAFT_CONTROLLER = HttpClientService.API + '/Aircraft';

  public static readonly GET_AIRCRAFT_CONTROLLER = HttpClientService.AIRCRAFT_CONTROLLER + "/get-aircraft";
  public static readonly CREATE_AIRCRAFT_CONTROLLER = HttpClientService.AIRCRAFT_CONTROLLER + "/add-aircraft";
  public static readonly UPDATE_AIRCRAFT_CONTROLLER = HttpClientService.AIRCRAFT_CONTROLLER + "/edit-aircraft";
}