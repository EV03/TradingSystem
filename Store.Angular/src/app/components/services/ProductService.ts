import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://localhost:7138/api/products';

  constructor(private http: HttpClient) {
  }

  updateProductPrice(productId: string | undefined, newPrice: number | undefined): Observable<void> {
    if (productId === undefined || newPrice === undefined) {
      throw new Error("ProductId and newPrice must be defined");
    }
    return this.http.patch<void>(
      `${this.apiUrl}/change-price?productId=${encodeURIComponent(productId)}`,
      newPrice
    );
  }

  showAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }
}
