﻿import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/ProductService';
import { Observer } from 'rxjs';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product } from '../models/product';

@Component({
  selector: 'app-products',
  templateUrl: './product.component.html',
  standalone: true,
  styleUrls: ['./product.component.css'],
  imports: [
    FormsModule,
    CommonModule,
    CurrencyPipe
  ]
})
export class ProductComponent implements OnInit {
  products: Product[] = [];
  productObserver: Observer<Product[]>;

  constructor(private productService: ProductService) {
    this.productObserver = {
      next: (data) => {
        this.products = data;
        console.log(data);
        console.log("show products");
      },
      error: (error) => {
        console.error("Error fetching products data:", error);
      },
      complete: () => {
        console.log("product data fetch complete");
      }
    };
  }

  ngOnInit(): void {
    this.productService.showAllProducts().subscribe(this.productObserver);
  }

  updatePrice(productId: number | undefined, newPrice: number | undefined): void {
    // @ts-ignore
    if (newPrice < 0) {
      console.error('Price cannot be negative');
      return;
    }
    this.productService.updateProductPrice(productId, newPrice).subscribe({
      next: () => {
        console.log(`Price for product with ID ${productId} updated successfully.`);
        alert(`Price updated successfully.`);
        this.productService.showAllProducts().subscribe(this.productObserver);
      },
      error: (error) => {
        console.error("Error updating product price:", error);
      }
    });
  }
}
