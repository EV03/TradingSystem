import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  private apiUrl = 'https://localhost:7138/api/stocks';

  stockItems: any[] = [];
  constructor(private http: HttpClient) { }
  showStock(): Observable<any> {
    return this.http.get(`${this.apiUrl}`);
  }
}
