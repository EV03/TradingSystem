import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/OrderService';
import { Order} from '../models/order';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-orders',
  templateUrl: './order.component.html',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  orders: Order[] = [];
  orderSupplierId: string = '';


  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.showOrders();
  }

  showOrders(): void {
    this.orderService.getOrders().subscribe({
      next: (data) => {
        this.orders = data;
        console.log("Orders fetched successfully:", data);
      },
      error: (error) => {
        console.error("Error fetching orders:", error);
      }
    });
  }

  rollReceivedOrder() {
    if (this.orderSupplierId) {
      this.orderService.rollReceivedOrder(this.orderSupplierId).subscribe(() => {
        alert('Order checked in successfully');
        this.showOrders();
        this.orderSupplierId = '';
      }, error => {
        alert('Error checking in order');
      });
    } else {
      alert('Please enter a valid Order Supplier ID');
    }
  }
}
