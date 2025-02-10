import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {StockComponent} from './components/stock/stock.component';
import {ProductComponent} from './components/product/product.component';
import {OrderComponent} from './components/order/order.component';

const routes: Routes = [
  { path: 'stocks', component: StockComponent },
  { path: 'products', component: ProductComponent },
  { path: 'orders', component: OrderComponent }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
