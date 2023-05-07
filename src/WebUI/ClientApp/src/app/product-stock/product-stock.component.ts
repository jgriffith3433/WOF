import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  ProductStockClient,
  ProductStockDto,
  ProductStockDetailsVm,
  CreateProductStockCommand,
  UpdateProductStockCommand,
  UpdateProductStockDetailsCommand
} from '../web-api-client';

@Component({
  selector: 'app-product-stock',
  templateUrl: './product-stock.component.html'
})
export class ProductStockComponent implements OnInit {
  public productStocks: ProductStockDto[];
  debug = false;
  selectedProductStockUnits: ProductStockDto;
  productStockEditor: any = {};
  newProductStockEditor: any = {};
  productStockModalRef: BsModalRef;
  newProductStockModalRef: BsModalRef;

  constructor(
    private productStockClient: ProductStockClient,
    private modalService: BsModalService
  ) { }

  ngOnInit(): void {
    this.productStockClient.getProductStocks().subscribe(
      result => {
        this.productStocks = result.productStocks;
      },
      error => console.error(error)
    );
  }

  showNewProductStockModal(template: TemplateRef<any>): void {
    this.newProductStockModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('name').focus(), 250);
  }

  newProductStockCancelled(): void {
    this.newProductStockModalRef.hide();
    this.newProductStockEditor = {};
  }

  addProductStock(): void {
    const productStock = {
      id: 0,
      name: this.newProductStockEditor.name,
      productSearchItems: []
    } as ProductStockDetailsVm;

    this.productStockClient.create(productStock as CreateProductStockCommand).subscribe(
      result => {
        productStock.id = result;
        this.productStocks.push(productStock);
        this.newProductStockModalRef.hide();
        this.newProductStockEditor = {};
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newProductStockEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('userImport').focus(), 250);
      }
    );
  }

  confirmDeleteProductStock(template: TemplateRef<any>) {
    //this.recipeOptionsModalRef.hide();
    //this.deleteRecipeModalRef = this.modalService.show(template);
  }

  deleteProductStockConfirmed(): void {
    //this.productStockClient.delete(this.selectedRecipe.id).subscribe(
    //  () => {
    //    this.deleteRecipeModalRef.hide();
    //    this.recipes = this.recipes.filter(t => t.id !== this.selectedRecipe.id);
    //    this.selectedRecipe = this.recipes.length ? this.recipes[0] : null;
    //  },
    //  error => console.error(error)
    //);
  }

  editProductStockUnits(productStock: ProductStockDto, inputId: string): void {
    this.selectedProductStockUnits = productStock;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateProductStockUnits(productStock: ProductStockDto, pressedEnter: boolean = false): void {
    const updateProductStockCommand = productStock as UpdateProductStockCommand;
    this.productStockClient.update(productStock.id, updateProductStockCommand).subscribe(
      result => {
        for (var i = this.productStocks.length - 1; i >= 0; i--) {
          if (this.productStocks[i].id == result.id) {
            this.productStocks[i] = result;
            break;
          }
        }
        this.selectedProductStockUnits = null;
      },
      error => console.error(error)
    );
  }

  updateProductStockDetails(): void {
    const updateProductStockDetailsCommand = this.productStockEditor as UpdateProductStockDetailsCommand;
    this.productStockClient.updateProductStockDetails(this.productStockEditor.id, updateProductStockDetailsCommand).subscribe(
      result => {
        for (var i = this.productStocks.length - 1; i >= 0; i--) {
          if (this.productStocks[i].id == result.id) {
            this.productStocks[i] = result;
            break;
          }
        }
        if (this.productStockEditor.id != result.id) {
          //product stock was merged into another product stock. remove the old one
          for (var i = this.productStocks.length - 1; i >= 0; i--) {
            if (this.productStocks[i].id == this.productStockEditor.id) {
              this.productStocks.splice(i, 1);
              break;
            }
          }
        }
        this.productStockModalRef.hide();
        this.productStockEditor = {};
      },
      error => console.error(error)
    );
  }


  showProductStockDetailsModal(template: TemplateRef<any>, productStock: ProductStockDto): void {
    this.productStockClient.getProductStockDetails(productStock.id, productStock.name).subscribe(
      result => {
        for (var i = this.productStocks.length - 1; i >= 0; i--) {
          if (this.productStocks[i].id == result.id) {
            this.productStocks[i] = result;
            break;
          }
        }

        this.productStockEditor = {
          ...result,
          search: result.name
        };

        this.productStockModalRef = this.modalService.show(template);
      },
      error => console.error(error)
    );
  }

  searchProductName(): void {
    this.productStockClient.getProductStockDetails(this.productStockEditor.id, this.productStockEditor.search).subscribe(
      result => {
        for (var i = this.productStocks.length - 1; i >= 0; i--) {
          if (this.productStocks[i].id == result.id) {
            this.productStocks[i] = result;
            break;
          }
        }
        var oldSearch = this.productStockEditor.search;
        this.productStockEditor = {
          ...result,
          search: oldSearch
        };
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.productStockEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('search').focus(), 250);
      }
    );
  }


}
