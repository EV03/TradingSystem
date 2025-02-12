import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {KeycloakService} from '../security/KeycloakService';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl : string;

  constructor(private http: HttpClient, private keycloakService: KeycloakService) {
    this.apiUrl = `${this.keycloakService.getApiUri()}/orders`;
  }

  placeOrder(orderProductDto: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/place-order`, orderProductDto)
      .pipe(
        catchError(this.handleError)
      );
  }

  getOrders(orderIds?: string[]): Observable<any> {
    let params = new HttpParams();
    if (orderIds && orderIds.length > 0) {
      params = params.set('orderIds', orderIds.join(','));
    }
    return this.http.get(`${this.apiUrl}`, { params })
      .pipe(
        catchError(this.handleError)
      );
  }

  rollReceivedOrder(orderSupplierId: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/roll-received-order`, null, {
      params: {
        orderSupplierId: orderSupplierId
      }
    }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError('Something went wrong; please try again later.');
  }
}
