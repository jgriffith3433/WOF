import { Component, TemplateRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  CompletedOrdersClient,
  CompletedOrderDto,
  CompletedOrderProductDto,
  UnitTypeDto,
  CreateCompletedOrderCommand,
  UpdateCompletedOrderCommand,
  CreateCompletedOrderProductCommand,
  UpdateCompletedOrderProductCommand
} from '../web-api-client';

@Component({
  selector: 'app-completed-orders-component',
  templateUrl: './completed-orders.component.html',
  styleUrls: ['./completed-orders.component.scss']
})
export class CompletedOrdersComponent implements OnInit {
  debug = false;
  completedOrders: CompletedOrderDto[];
  unitTypes: UnitTypeDto[];
  selectedCompletedOrder: CompletedOrderDto;
  selectedCompletedOrderProduct: CompletedOrderProductDto;
  newCompletedOrderEditor: any = {};
  completedOrderOptionsEditor: any = {};
  completedOrderProductDetailsEditor: any = {};
  newCompletedOrderModalRef: BsModalRef;
  completedOrderOptionsModalRef: BsModalRef;
  deleteCompletedOrderModalRef: BsModalRef;
  completedOrderProductDetailsModalRef: BsModalRef;

  constructor(
    private completedOrdersClient: CompletedOrdersClient,
    private modalService: BsModalService,
    private router: Router
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  ngOnInit(): void {
    this.completedOrdersClient.get().subscribe(
      result => {
        this.completedOrders = result.completedOrders;
        if (this.completedOrders.length) {
          this.selectedCompletedOrder = this.completedOrders[0];
        }
      },
      error => console.error(error)
    );
  }

  // Completed Orders
  remainingCompletedOrderProducts(completedOrder: CompletedOrderDto): number {
    return completedOrder.completedOrderProducts.filter(t => !t.walmartId).length;
  }

  showNewCompletedOrderModal(template: TemplateRef<any>): void {
    this.newCompletedOrderModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('name').focus(), 250);
  }

  newCompletedOrderCancelled(): void {
    this.newCompletedOrderModalRef.hide();
    this.newCompletedOrderEditor = {};
  }

  addCompletedOrder(): void {
    const completedOrder = {
      id: 0,
      name: this.newCompletedOrderEditor.name,
      userImport: this.newCompletedOrderEditor.userImport,
      completedOrderProducts: []
    } as CompletedOrderDto;

    this.completedOrdersClient.create(completedOrder as CreateCompletedOrderCommand).subscribe(
      result => {
        this.completedOrdersClient.get2(result).subscribe(
          result => {
            this.completedOrders.push(result);
            this.selectedCompletedOrder = result;
            this.newCompletedOrderModalRef.hide();
            this.newCompletedOrderEditor = {};
          },
          error => console.error(error)
        );
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newCompletedOrderEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('name').focus(), 250);
      }
    );
  }

  showCompletedOrderOptionsModal(template: TemplateRef<any>) {
    this.completedOrderOptionsEditor = {
      id: this.selectedCompletedOrder.id,
      name: this.selectedCompletedOrder.name,
      userImport: this.selectedCompletedOrder.userImport
    };

    this.completedOrderOptionsModalRef = this.modalService.show(template);
  }

  updateCompletedOrderOptions() {
    const updateCompletedOrderCommand = this.completedOrderOptionsEditor as UpdateCompletedOrderCommand;
    this.completedOrdersClient.update(this.selectedCompletedOrder.id, updateCompletedOrderCommand).subscribe(
      () => {
        this.selectedCompletedOrder.name = this.completedOrderOptionsEditor.name;
        this.selectedCompletedOrder.userImport = this.completedOrderOptionsEditor.userImport;
        this.completedOrderOptionsModalRef.hide();
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
  showCompletedOrderProductDetailsModal(template: TemplateRef<any>, completedOrderProduct: CompletedOrderProductDto): void {
    this.completedOrdersClient.getCompletedOrderProduct(completedOrderProduct.id).subscribe(
      result => {
        this.selectedCompletedOrderProduct = result;
        for (var i = this.selectedCompletedOrder.completedOrderProducts.length - 1; i >= 0; i--) {
          if (this.selectedCompletedOrder.completedOrderProducts[i].id == this.selectedCompletedOrderProduct.id) {
            this.selectedCompletedOrder.completedOrderProducts[i] = this.selectedCompletedOrderProduct;
            break;
          }
        }
        this.completedOrderProductDetailsEditor = {
          ...this.selectedCompletedOrderProduct,
          search: this.selectedCompletedOrderProduct.name
        };
        if (this.selectedCompletedOrderProduct.walmartSearchResponse) {
          this.completedOrderProductDetailsEditor.walmartSearchItems = JSON.parse(this.selectedCompletedOrderProduct.walmartSearchResponse).items;
        }

        this.completedOrderProductDetailsModalRef = this.modalService.show(template);
      },
      error => console.error(error)
    );
  }

  getWalmartLinkFromProductDetailsEditor(): string {
    if (this.completedOrderProductDetailsEditor.walmartSearchItems) {
      for (var walmartSearchItem of this.completedOrderProductDetailsEditor.walmartSearchItems) {
        if (walmartSearchItem.itemId == this.completedOrderProductDetailsEditor.walmartId) {
          return "https://www.walmart.com/ip/" + walmartSearchItem.name + "/" + walmartSearchItem.itemId;
        }
      }
    }
    return "#";
  }

  searchCompletedOrderProductName(): void {
    this.completedOrdersClient.searchCompletedOrderProductName(this.completedOrderProductDetailsEditor.id, this.completedOrderProductDetailsEditor.search).subscribe(
      result => {
        this.selectedCompletedOrderProduct = result;
        for (var i = this.selectedCompletedOrder.completedOrderProducts.length - 1; i >= 0; i--) {
          if (this.selectedCompletedOrder.completedOrderProducts[i].id == this.selectedCompletedOrderProduct.id) {
            this.selectedCompletedOrder.completedOrderProducts[i] = this.selectedCompletedOrderProduct;
            break;
          }
        }
        var oldSearch = this.completedOrderProductDetailsEditor.search;
        this.completedOrderProductDetailsEditor = {
          ...this.selectedCompletedOrderProduct,
          search: oldSearch
        };
        if (this.selectedCompletedOrderProduct.walmartSearchResponse) {
          this.completedOrderProductDetailsEditor.walmartSearchItems = JSON.parse(this.selectedCompletedOrderProduct.walmartSearchResponse).items;
        }
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.completedOrderProductDetailsEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('name').focus(), 250);
      }
    );
  }

  updateCompletedOrderProductDetails(): void {
    const completedOrderProduct = this.completedOrderProductDetailsEditor as UpdateCompletedOrderProductCommand;
    this.completedOrdersClient.updateCompletedOrderProduct(this.selectedCompletedOrderProduct.id, completedOrderProduct).subscribe(
      result => {
        this.selectedCompletedOrderProduct = result;
        for (var i = this.selectedCompletedOrder.completedOrderProducts.length - 1; i >= 0; i--) {
          if (this.selectedCompletedOrder.completedOrderProducts[i].id == this.selectedCompletedOrderProduct.id) {
            this.selectedCompletedOrder.completedOrderProducts[i] = this.selectedCompletedOrderProduct;
            break;
          }
        }
        this.selectedCompletedOrderProduct = null;
        this.completedOrderProductDetailsModalRef.hide();
        this.completedOrderProductDetailsEditor = {};
      },
      error => console.error(error)
    );
  }

  addCompletedOrderProduct() {
    const completedOrderProduct = {
      id: 0,
      name: '',
    } as CompletedOrderProductDto;

    this.selectedCompletedOrder.completedOrderProducts.push(completedOrderProduct);
    const index = this.selectedCompletedOrder.completedOrderProducts.length - 1;
    this.editCompletedOrderProduct(completedOrderProduct, 'completedOrderProductName' + index);
  }

  editCompletedOrderProduct(completedOrderProduct: CompletedOrderProductDto, inputId: string): void {
    this.selectedCompletedOrderProduct = completedOrderProduct;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateCompletedOrderProduct(completedOrderProduct: CompletedOrderProductDto, pressedEnter: boolean = false): void {
    const isNewCompletedOrderProduct = completedOrderProduct.id === 0;

    if (!completedOrderProduct.name.trim()) {
      this.deleteCompletedOrderProduct(completedOrderProduct);
      return;
    }

    if (completedOrderProduct.id === 0) {
      this.completedOrdersClient
        .createCompletedOrderProduct({
          ...completedOrderProduct, completedOrderId: this.selectedCompletedOrder.id
        } as CreateCompletedOrderProductCommand)
        .subscribe(
          result => {
            completedOrderProduct.id = result;
          },
          error => console.error(error)
        );
    } else {
      this.completedOrdersClient.updateCompletedOrderProduct(completedOrderProduct.id, completedOrderProduct).subscribe(
        () => console.log('Update succeeded.'),
        error => console.error(error)
      );
    }

    this.selectedCompletedOrderProduct = null;

    if (isNewCompletedOrderProduct && pressedEnter) {
      setTimeout(() => this.addCompletedOrderProduct(), 250);
    }
  }

  deleteCompletedOrderProduct(completedOrderProduct: CompletedOrderProductDto) {
    if (this.completedOrderProductDetailsModalRef) {
      this.completedOrderProductDetailsModalRef.hide();
    }

    if (completedOrderProduct.id === 0) {
      const completedOrderProductIndex = this.selectedCompletedOrder.completedOrderProducts.indexOf(this.selectedCompletedOrderProduct);
      this.selectedCompletedOrder.completedOrderProducts.splice(completedOrderProductIndex, 1);
    } else {
      this.completedOrdersClient.deleteCompletedOrderProduct(completedOrderProduct.id).subscribe(
        () =>
        (this.selectedCompletedOrder.completedOrderProducts = this.selectedCompletedOrder.completedOrderProducts.filter(
          t => t.id !== completedOrderProduct.id
        )),
        error => console.error(error)
      );
    }
  }
}
