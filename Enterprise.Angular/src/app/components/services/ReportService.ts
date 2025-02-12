import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SupplierDeliveryTime } from '../models/SupplierDeliveryTime';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'https://localhost:7058/api/reports';

  constructor(private http: HttpClient) { }

  showAllReports(enterpriseId: string): Observable<SupplierDeliveryTime[]> {
    const params = new HttpParams().set('enterpriseId', enterpriseId);
    return this.http.get<SupplierDeliveryTime[]>(`${this.apiUrl}/supplier-delivery-times`, { params })
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }
}
