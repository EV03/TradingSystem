import {OrderSupplierProduct} from './OrderSupplierProducts';

export class OrderSupplier {
  id?: number;
  orderDate?: Date;
  deliveryDate?: Date;
  orderSupplierProducts: OrderSupplierProduct[] = [];
}
