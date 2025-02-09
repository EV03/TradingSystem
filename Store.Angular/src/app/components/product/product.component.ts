import { Component, OnInit } from '@angular/core';
import {ProductService} from '../services/ProductService';
import { Observer } from 'rxjs';
import {CommonModule, CurrencyPipe} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {Product} from '../models/product';

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
  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    const productObserver: Observer<Product[]> = {
      next: (data) => {
        this.products = data;
        console.log(data);
        console.log("show proudcts");
      },
      error: (error) => {
        console.error("Error fetching products data:", error);
      },
      complete: () => {
        console.log("product data fetch complete");
      }
    };

    this.productService.showAllProducts().subscribe(productObserver);
  }

  updatePrice(productId: string | undefined, newPrice: number | undefined): void {
    this.productService.updateProductPrice(productId, newPrice).subscribe({
      next: () => {
        console.log(`Price for product with ID ${productId} updated successfully.`);
      },
      error: (error) => {
        console.error("Error updating product price:", error);
      }
    });
  }
}
