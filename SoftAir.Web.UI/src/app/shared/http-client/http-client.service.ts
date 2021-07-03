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

}