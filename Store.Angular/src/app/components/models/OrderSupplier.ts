import {OrderSupplierProduct} from './OrderSupplierProducts';

export class OrderSupplier {
  id?: string;
  orderDate?: Date;
  deliveryDate?: Date;
  orderSupplierProducts: OrderSupplierProduct[] = [];
}
