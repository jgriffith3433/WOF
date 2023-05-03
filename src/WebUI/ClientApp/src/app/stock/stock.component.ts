import { Component } from '@angular/core';
import { StockClient, StockDto } from '../web-api-client';

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html'
})
export class StockComponent {
  public products: StockDto[];

  constructor(private client: StockClient) {
    client.getStock().subscribe(result => {
      this.products = result.products;
    }, error => console.error(error));
  }
}
