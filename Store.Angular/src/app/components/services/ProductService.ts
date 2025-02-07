import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://localhost:7138/api/products';

  constructor(private http: HttpClient) { }

  changePrice(productId: string, newPrice: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/change-price`, newPrice, {
      params: {
        productId: productId
      }
    });
  }
  showAllProducts(): Observable<any> {
    return this.http.get(`${this.apiUrl}`);
  }
}
