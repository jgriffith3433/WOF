import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  CompletedOrdersClient,
  ProductsClient,
  CompletedOrderDto,
  ProductDto,
  SizeTypeDto,
  CreateCompletedOrderCommand,
  UpdateCompletedOrderCommand,
  CreateProductCommand,
  UpdateProductCommand
} from '../web-api-client';

@Component({
  selector: 'app-completed-orders-component',
  templateUrl: './completed-orders.component.html',
  styleUrls: ['./completed-orders.component.scss']
})
export class CompletedOrdersComponent implements OnInit {
  debug = false;
  completedOrders: CompletedOrderDto[];
  sizeTypes: SizeTypeDto[];
  selectedCompletedOrder: CompletedOrderDto;
  selectedProduct: ProductDto;
  newCompletedOrderEditor: any = {};
  completedOrderOptionsEditor: any = {};
  productDetailsEditor: any = {};
  newCompletedOrderModalRef: BsModalRef;
  completedOrderOptionsModalRef: BsModalRef;
  deleteCompletedOrderModalRef: BsModalRef;
  productDetailsModalRef: BsModalRef;

  constructor(
    private completedOrdersClient: CompletedOrdersClient,
    private productsClient: ProductsClient,
    private modalService: BsModalService
  ) { }

  ngOnInit(): void {
    this.completedOrdersClient.get().subscribe(
      result => {
        this.completedOrders = result.completedOrders;
        this.sizeTypes = result.sizeTypes;
        if (this.completedOrders.length) {
          this.selectedCompletedOrder = this.completedOrders[0];
        }
      },
      error => console.error(error)
    );
  }

  // Completed Orders
  remainingProducts(completedOrder: CompletedOrderDto): number {
    return completedOrder.products.filter(t => !t.walmartId).length;
  }

  showNewCompletedOrderModal(template: TemplateRef<any>): void {
    this.newCompletedOrderModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('userImport').focus(), 250);
  }

  newCompletedOrderCancelled(): void {
    this.newCompletedOrderModalRef.hide();
    this.newCompletedOrderEditor = {};
  }

  addCompletedOrder(): void {
    const completedOrder = {
      id: 0,
      userImport: this.newCompletedOrderEditor.userImport,
      products: []
    } as CompletedOrderDto;

    this.completedOrdersClient.create(completedOrder as CreateCompletedOrderCommand).subscribe(
      result => {
        completedOrder.id = result;
        this.completedOrders.push(completedOrder);
        this.selectedCompletedOrder = completedOrder;
        this.newCompletedOrderModalRef.hide();
        this.newCompletedOrderEditor = {};

        this.completedOrdersClient.get2(completedOrder.id).subscribe(
          result => {
            this.selectedCompletedOrder = result;
          },
          error => console.error(error)
        );
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newCompletedOrderEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('userImport').focus(), 250);
      }
    );
  }

  showCompletedOrderOptionsModal(template: TemplateRef<any>) {
    this.completedOrderOptionsEditor = {
      id: this.selectedCompletedOrder.id,
      userImport: this.selectedCompletedOrder.userImport
    };

    this.completedOrderOptionsModalRef = this.modalService.show(template);
  }

  updateCompletedOrderOptions() {
    const updateCompletedOrderCommand = this.completedOrderOptionsEditor as UpdateCompletedOrderCommand;
    this.completedOrdersClient.update(this.selectedCompletedOrder.id, updateCompletedOrderCommand).subscribe(
      () => {
        this.selectedCompletedOrder.userImport = this.completedOrderOptionsEditor.userImport;
        this.completedOrderOptionsModalRef.hide();
        this.completedOrdersClient.get2(this.selectedCompletedOrder.id).subscribe(
          result => {
            this.selectedCompletedOrder = result;
          },
          error => console.error(error)
        );
        this.completedOrderOptionsEditor = {};
      },
      error => console.error(error)
    );
  }

  confirmDeleteCompletedOrder(template: TemplateRef<any>) {
    this.completedOrderOptionsModalRef.hide();
    this.deleteCompletedOrderModalRef = this.modalService.show(template);
  }

  deleteCompletedOrderConfirmed(): void {
    this.completedOrdersClient.delete(this.selectedCompletedOrder.id).subscribe(
      () => {
        this.deleteCompletedOrderModalRef.hide();
        this.completedOrders = this.completedOrders.filter(t => t.id !== this.selectedCompletedOrder.id);
        this.selectedCompletedOrder = this.completedOrders.length ? this.completedOrders[0] : null;
      },
      error => console.error(error)
    );
  }

  // Products
  showProductDetailsModal(template: TemplateRef<any>, product: ProductDto): void {
    this.selectedProduct = product;
    this.productDetailsEditor = {
      ...this.selectedProduct
    };

    this.productDetailsModalRef = this.modalService.show(template);
  }

  updateProductDetails(): void {
    const product = this.productDetailsEditor as UpdateProductCommand;
    this.productsClient.updateProductDetails(this.selectedProduct.id, product).subscribe(
      () => {
        this.selectedProduct.sizeType = this.productDetailsEditor.sizeType;
        this.selectedProduct.walmartId = this.productDetailsEditor.walmartId;
        this.productDetailsModalRef.hide();
        this.productDetailsEditor = {};
      },
      error => console.error(error)
    );
  }

  addProduct() {
    const product = {
      id: 0,
      name: '',
      sizeType: this.sizeTypes[0].value
    } as ProductDto;

    this.selectedCompletedOrder.products.push(product);
    const index = this.selectedCompletedOrder.products.length - 1;
    this.editProduct(product, 'productName' + index);
  }

  editProduct(product: ProductDto, inputId: string): void {
    this.selectedProduct = product;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateProduct(product: ProductDto, pressedEnter: boolean = false): void {
    const isNewProduct = product.id === 0;

    if (!product.name.trim()) {
      this.deleteProduct(product);
      return;
    }

    //TODO: need to look at this
    //if (product.id === 0) {
    //  this.productsClient
    //    .create({
    //      ...product, listId: this.selectedCompletedOrder.id
    //    } as CreateProductCommand)
    //    .subscribe(
    //      result => {
    //        product.id = result;
    //      },
    //      error => console.error(error)
    //    );
    //} else {
    //  this.productsClient.update(product.id, product).subscribe(
    //    () => console.log('Update succeeded.'),
    //    error => console.error(error)
    //  );
    //}

    this.selectedProduct = null;

    if (isNewProduct && pressedEnter) {
      setTimeout(() => this.addProduct(), 250);
    }
  }

  deleteProduct(product: ProductDto) {
    if (this.productDetailsModalRef) {
      this.productDetailsModalRef.hide();
    }

    if (product.id === 0) {
      const productIndex = this.selectedCompletedOrder.products.indexOf(this.selectedProduct);
      this.selectedCompletedOrder.products.splice(productIndex, 1);
    } else {
      this.productsClient.delete(product.id).subscribe(
        () =>
        (this.selectedCompletedOrder.products = this.selectedCompletedOrder.products.filter(
          t => t.id !== product.id
        )),
        error => console.error(error)
      );
    }
  }
}
