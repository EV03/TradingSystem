import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SupplierDeliveryTime } from '../models/SupplierDeliveryTime';
import {KeycloakService} from '../security/KeycloakService';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl : string;

    constructor(private http: HttpClient, private keycloakService: KeycloakService) {
      this.apiUrl = `${this.keycloakService.getApiUri()}/reports`;
  }

  showAllReports(): Observable<SupplierDeliveryTime[]> {
    return this.http.get<SupplierDeliveryTime[]>(`${this.apiUrl}/supplier-delivery-times`)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }
}
