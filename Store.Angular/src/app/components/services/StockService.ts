import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {KeycloakService} from '../security/KeycloakService';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  stockItems: any[] = [];
  private apiUrl : string;

  constructor(private http: HttpClient, private keycloakService: KeycloakService) {
    this.apiUrl = `${this.keycloakService.getApiUri()}/stocks`;
  }
  showStock(): Observable<any> {
    return this.http.get(`${this.apiUrl}`);
  }
}
