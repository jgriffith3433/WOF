import { Component } from '@angular/core';
import { ProductStockClient, ProductStockDto } from '../web-api-client';

@Component({
  selector: 'app-product-stock',
  templateUrl: './product-stock.component.html'
})
export class ProductStockComponent {
  public productStocks: ProductStockDto[];

  constructor(private client: ProductStockClient) {
    client.getProductStock().subscribe(result => {
      this.productStocks = result.productStocks;
    }, error => console.error(error));
  }
}
