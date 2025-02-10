import {OrderSupplier} from './OrderSupplier';

export class Order {
  id? : string;
  orderSupplier: OrderSupplier[] = [];
}
