import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  ProductsClient,
  ProductDto,
  CreateProductCommand,
  UpdateProductCommand,
  UnitTypeDto
} from '../web-api-client';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {
  debug: boolean = false;
  public products: ProductDto[];
  unitTypes: UnitTypeDto[];
  selectedProductName: ProductDto;
  selectedProductSize: ProductDto;
  selectedProductUnitType: ProductDto;
  productEditor: any = {};
  newProductEditor: any = {};
  productModalRef: BsModalRef;
  newProductModalRef: BsModalRef;
  deleteProductModalRef: BsModalRef;

  constructor(
    private productsClient: ProductsClient,
    private modalService: BsModalService,
    private router: Router
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }


  ngOnInit(): void {
    this.productsClient.getProducts().subscribe(
      result => {
        this.products = result.products;
        this.unitTypes = result.unitTypes;
      },
      error => console.error(error)
    );
  }


  showNewProductModal(template: TemplateRef<any>): void {
    this.newProductModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('name').focus(), 250);
  }

  newProductCancelled(): void {
    this.newProductModalRef.hide();
    this.newProductEditor = {};
  }

  getUnitTypeNameFromUnitTypeValue(unitTypeValue: number): string {
    for (var unitType of this.unitTypes) {
      if (unitType.value == unitTypeValue) {
        return unitType.name;
      }
    }
    return "Unknown";
  }

  addProduct(): void {
    const product = {
      id: 0,
      name: this.newProductEditor.name
    } as ProductDto;

    this.productsClient.create(product as CreateProductCommand).subscribe(
      result => {
        this.products.push(result);
        this.newProductModalRef.hide();
        this.newProductEditor = {};
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newProductEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('name').focus(), 250);
      }
    );
  }

  editProductName(product: ProductDto, inputId: string): void {
    this.selectedProductName = product;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  editProductUnitType(product: ProductDto, inputId: string): void {
    this.selectedProductUnitType = product;
    setTimeout(() => {
      document.getElementById(inputId).focus();
      (<HTMLSelectElement>document.getElementById(inputId)).size = (<HTMLSelectElement>document.getElementById(inputId)).length;
    }, 100);
  }

  editProductSize(product: ProductDto, inputId: string): void {
    this.selectedProductSize = product;
    setTimeout(() => { document.getElementById(inputId).focus(); }, 100);
  }

  updateProductName(product: ProductDto, pressedEnter: boolean = false): void {
    const updateProductCommand = product as UpdateProductCommand;
    this.productsClient.updateName(product.id, updateProductCommand).subscribe(
      result => {
        for (var i = this.products.length - 1; i >= 0; i--) {
          if (this.products[i].id == result.id) {
            this.products[i] = result;
            break;
          }
        }
        this.selectedProductName = null;
      },
      error => console.error(error)
    );
  }

  updateProductUnitType(product: ProductDto, pressedEnter: boolean = false): void {
    const updateProductCommand = product as UpdateProductCommand;
    this.productsClient.updateUnitType(product.id, updateProductCommand).subscribe(
      result => {
        for (var i = this.products.length - 1; i >= 0; i--) {
          if (this.products[i].id == result.id) {
            this.products[i] = result;
            break;
          }
        }
        this.selectedProductUnitType = null;
      },
      error => console.error(error)
    );
  }

  updateProductSize(product: ProductDto, pressedEnter: boolean = false): void {
    const updateProductCommand = product as UpdateProductCommand;
    this.productsClient.updateSize(product.id, updateProductCommand).subscribe(
      result => {
        for (var i = this.products.length - 1; i >= 0; i--) {
          if (this.products[i].id == result.id) {
            this.products[i] = result;
            break;
          }
        }
        this.selectedProductSize = null;
      },
      error => console.error(error)
    );
  }

  searchProductName(): void {
    const updateProductNameCommand = this.productEditor as UpdateProductCommand;
    this.productsClient.updateName(this.productEditor.id, updateProductNameCommand).subscribe(
      result => {
        this.productEditor = result;
        for (var i = this.products.length - 1; i >= 0; i--) {
          if (this.products[i].id == this.productEditor.id) {
            this.products[i] = this.productEditor;
            break;
          }
        }
        if (this.productEditor.walmartSearchResponse) {
          this.productEditor.walmartSearchItems = JSON.parse(this.productEditor.walmartSearchResponse).items;
        }
      },
      error => console.error(error)
    );
  }

  updateProductDetails(): void {
    const updateProductCommand = this.productEditor as UpdateProductCommand;
    this.productsClient.update(this.productEditor.id, updateProductCommand).subscribe(
      result => {
        this.productEditor = result;
        for (var i = this.products.length - 1; i >= 0; i--) {
          if (this.products[i].id == this.productEditor.id) {
            this.products[i] = this.productEditor;
            break;
          }
        }
        if (this.productEditor.walmartSearchResponse) {
          this.productEditor.walmartSearchItems = JSON.parse(this.productEditor.walmartSearchResponse).items;
        }
        this.productModalRef.hide();
        this.productEditor = null;
      },
      error => console.error(error)
    );
  }

  showProductDetailsModal(template: TemplateRef<any>, product: ProductDto): void {
    this.productsClient.getProductDetails(product.id).subscribe(
      result => {
        for (var i = this.products.length - 1; i >= 0; i--) {
          if (this.products[i].id == result.id) {
            this.products[i] = result;
            break;
          }
        }

        this.productEditor = {
          ...result
        };
        if (this.productEditor.walmartSearchResponse) {
          this.productEditor.walmartSearchItems = JSON.parse(this.productEditor.walmartSearchResponse).items;
        }

        this.productModalRef = this.modalService.show(template);
      },
      error => console.error(error)
    );
  }

  deleteProduct(product: ProductDto) {
    if (this.productModalRef) {
      this.productModalRef.hide();
    }

    if (product.id === 0) {
      const completedOrderProductIndex = this.products.indexOf(this.selectedProductName);
      this.products.splice(completedOrderProductIndex, 1);
    } else {
      this.productsClient.delete(product.id).subscribe(
        () =>
        (this.products = this.products.filter(
          t => t.id !== product.id
        )),
        error => console.error(error)
      );
    }
  }


}
