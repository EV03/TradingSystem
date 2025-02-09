import { Component, OnInit } from '@angular/core';
import { StockService } from '../services/StockService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observer } from 'rxjs';
import { Stock } from '../models/Stock';
import {OrderService} from '../services/OrderService';

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

  stockItems: Stock[] = [];
  lowStockItems: Stock[] = [];
  constructor(private stockService: StockService, private orderService: OrderService) { }

  ngOnInit(): void {
    const stockObserver: Observer<Stock[]> = {
      next: (data) => {
        this.stockItems = data;
        this.lowStockItems = data.filter(item =>
          item.availableQuantity !== undefined ? item.availableQuantity < 10 : false
        );
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
    console.log("showstock");
  }

  orderAllStockItems(): void {
    const orders = this.stockItems.concat(this.lowStockItems)
      .filter(item => item.orderQuantity && item.orderQuantity > 0)
      .map(item => ({ productId: item.productId, quantity: item.orderQuantity }));

    if (orders.length > 0) {
      console.log('Placing orders:', orders);
      this.orderService.placeOrder(orders).subscribe(() => {
        alert('Orders placed successfully');
      });
    } else {
      console.error('No valid orders to place');
    }
  }
}
