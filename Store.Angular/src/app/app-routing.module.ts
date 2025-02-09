import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {StockComponent} from './components/stock/stock.component';
import {ProductComponent} from './components/product/product.component';

const routes: Routes = [
  { path: 'stocks', component: StockComponent },
  { path: 'products', component: ProductComponent }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
