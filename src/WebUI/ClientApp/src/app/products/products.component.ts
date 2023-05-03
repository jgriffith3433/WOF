import { Component } from '@angular/core';
import { ProductsClient, ProductBriefDto } from '../web-api-client';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent {
  public products: ProductBriefDto[];

  constructor(private client: ProductsClient) {
    client.getProducts().subscribe(result => {
      this.products = result.products;
    }, error => console.error(error));
  }
}
