import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'https://localhost:7138/api/orders';

  constructor(private http: HttpClient) { }

  placeOrder(orderProductDto: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/place-order`, orderProductDto);
  }

  getOrders(orderIds: string[]): Observable<any> {
    return this.http.get(`${this.apiUrl}`, {
      params: {
        orderIds: orderIds.join(',')
      }
    });
  }
  rollReceivedOrder(orderSupplierId: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/roll-received-order`, null, {
      params: {
        orderSupplierId: orderSupplierId
      }
    });
  }
}
