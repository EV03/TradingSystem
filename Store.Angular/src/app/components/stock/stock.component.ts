import { Component, OnInit } from '@angular/core';
import { StockService } from '../services/StockService';
import {FormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observer } from 'rxjs';

@Component({
  selector: 'app-stocks',
  templateUrl: './stock.component.html',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule
  ],
  styleUrls: ['./stock.component.css']
})
export class StockComponent implements OnInit {

  stockItems: any[] = [];
  constructor(private stockService: StockService) { }

  ngOnInit(): void {
    const stockObserver: Observer<any> = {
      next: (data) => {
        console.log(data);
        console.log("showstock");
      },
      error: (error) => {
        console.error("Error fetching stock data:", error);
      },
      complete: () => {
        console.log("Stock data fetch complete");
      }
    };

    this.stockService.showStock().subscribe(stockObserver);
  }

  orderStockItem(item: any, amount: number): void {
    console.log(`Ordering ${amount} of ${item.name}`);
  }
}
