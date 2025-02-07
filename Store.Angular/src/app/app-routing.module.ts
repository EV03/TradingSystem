import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {StockComponent} from './components/stock/stock.component';

const routes: Routes = [
  { path: 'stocks', component: StockComponent }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
