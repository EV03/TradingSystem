import {OrderSupplier} from './orderSupplier';

export class Order {
  id? : string;
  orderSupplier: OrderSupplier[] = [];
}
