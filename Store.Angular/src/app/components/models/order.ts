import {OrderSupplier} from './orderSupplier';

export class Order {
  id? : number;
  orderSupplier: OrderSupplier[] = [];
}
