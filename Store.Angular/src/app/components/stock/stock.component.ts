import { Component, OnInit } from '@angular/core';
import { StockService } from '../services/StockService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observer } from 'rxjs';
import { Stock } from '../models/Stock';
import { OrderService } from '../services/OrderService';
import { Order } from '../models/order';

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
  orderDto: Order | null = null;
  stockObserver: Observer<Stock[]>;

  constructor(private stockService: StockService, private orderService: OrderService) {
    this.stockObserver = {
      next: (data) => {
        this.stockItems = data;
        this.lowStockItems = data.filter(item =>
          item.availableQuantity !== undefined ? item.availableQuantity <= 40 : false
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
  }

  ngOnInit(): void {
    this.stockService.showStock().subscribe(this.stockObserver);
    console.log("showstock");
  }

  orderAllStockItems(): void {
    const orders = this.stockItems
      .filter(item => item.orderQuantity && item.orderQuantity > 0)
      .map(item => ({ cachedProductId: item.cachedProductId, quantity: item.orderQuantity }));

    if (orders.length > 0) {
      console.log('Placing orders:', orders);
      this.orderService.placeOrder(orders).subscribe((orderDto) => {
        this.orderDto = orderDto;
        const orderDetails = this.formatOrderDetails(orderDto);
        alert(`Orders placed successfully:\n${orderDetails}`);
        // Fetch the updated stock list
        this.stockService.showStock().subscribe(this.stockObserver);
      });
    } else {
      console.error('No valid orders to place');
    }
  }

  private formatOrderDetails(orderDto: Order): string {
    let details = `Order ID: ${orderDto.id}\n`;
    orderDto.orderSupplier.forEach(supplier => {
      details += `Supplier ID: ${supplier.id}}\n`;
      supplier.orderSupplierProducts.forEach(product => {
        details += `  Product Name: ${product.cachedProductName}, Quantity: ${product.quantity}\n`;
      });
    });
    return details;
  }
}
