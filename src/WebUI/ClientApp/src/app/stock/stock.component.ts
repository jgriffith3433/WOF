import { Component } from '@angular/core';
import { StockClient, StockDto } from '../web-api-client';

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html'
})
export class StockComponent {
  public ingredients: StockDto[];

  constructor(private client: StockClient) {
    client.getStock().subscribe(result => {
      this.ingredients = result.ingredients;
    }, error => console.error(error));
  }
}
