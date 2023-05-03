import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  CompletedOrdersClient,
  IngredientsClient,
  CompletedOrderDto,
  IngredientDto,
  CreateCompletedOrderCommand,
  UpdateCompletedOrderCommand,
  CreateIngredientCommand,
  UpdateIngredientCommand
} from '../web-api-client';

@Component({
  selector: 'app-completed-orders-component',
  templateUrl: './completed-orders.component.html',
  styleUrls: ['./completed-orders.component.scss']
})
export class CompletedOrdersComponent implements OnInit {
  debug = false;
  completedOrders: CompletedOrderDto[];
  selectedCompletedOrder: CompletedOrderDto;
  selectedIngredient: IngredientDto;
  newCompletedOrderEditor: any = {};
  completedOrderOptionsEditor: any = {};
  ingredientDetailsEditor: any = {};
  newCompletedOrderModalRef: BsModalRef;
  completedOrderOptionsModalRef: BsModalRef;
  deleteCompletedOrderModalRef: BsModalRef;
  ingredientDetailsModalRef: BsModalRef;

  constructor(
    private completedOrdersClient: CompletedOrdersClient,
    private ingredientsClient: IngredientsClient,
    private modalService: BsModalService
  ) { }

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
  remainingIngredients(completedOrder: CompletedOrderDto): number {
    return completedOrder.ingredients.filter(t => !t.walmartId).length;
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
      ingredients: []
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
            //if (this.completedOrders.length) {
            //  this.selectedCompletedOrder = this.completedOrders[0];
            //}
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
        (this.selectedCompletedOrder.userImport = this.completedOrderOptionsEditor.userImport),
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

  // Ingredients
  showIngredientDetailsModal(template: TemplateRef<any>, ingredient: IngredientDto): void {
    this.selectedIngredient = ingredient;
    this.ingredientDetailsEditor = {
      ...this.selectedIngredient
    };

    this.ingredientDetailsModalRef = this.modalService.show(template);
  }

  updateIngredientDetails(): void {
    const ingredient = this.ingredientDetailsEditor as UpdateIngredientCommand;
    this.ingredientsClient.updateIngredientDetails(this.selectedIngredient.id, ingredient).subscribe(
      () => {
        //TODO: Need to look at this
        //if (this.selectedIngredient.completedOrderId !== this.ingredientDetailsEditor.completedOrderId) {
        //  this.selectedCompletedOrder.ingredients = this.selectedCompletedOrder.ingredients.filter(
        //    i => i.id !== this.selectedIngredient.id
        //  );
        //  const completedOrderIndex = this.completedOrders.findIndex(
        //    c => c.id === this.ingredientDetailsEditor.completedOrderId
        //  );
        //  this.selectedIngredient.listId = this.ingredientDetailsEditor.listId;
        //  this.completedOrders[completedOrderIndex].ingredients.push(this.selectedIngredient);
        //}

        //this.selectedIngredient.walmartId = this.ingredientDetailsEditor.walmartId;
        //this.ingredientDetailsModalRef.hide();
        //this.ingredientDetailsEditor = {};
      },
      error => console.error(error)
    );
  }

  addIngredient() {
    const ingredient = {
      id: 0,
      name: '',
      walmartId: ''
    } as IngredientDto;

    this.selectedCompletedOrder.ingredients.push(ingredient);
    const index = this.selectedCompletedOrder.ingredients.length - 1;
    this.editIngredient(ingredient, 'ingredientName' + index);
  }

  editIngredient(ingredient: IngredientDto, inputId: string): void {
    this.selectedIngredient = ingredient;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateIngredient(ingredient: IngredientDto, pressedEnter: boolean = false): void {
    const isNewIngredient = ingredient.id === 0;

    if (!ingredient.name.trim()) {
      this.deleteIngredient(ingredient);
      return;
    }

    //TODO: need to look at this
    //if (ingredient.id === 0) {
    //  this.ingredientsClient
    //    .create({
    //      ...ingredient, listId: this.selectedCompletedOrder.id
    //    } as CreateIngredientCommand)
    //    .subscribe(
    //      result => {
    //        ingredient.id = result;
    //      },
    //      error => console.error(error)
    //    );
    //} else {
    //  this.ingredientsClient.update(ingredient.id, ingredient).subscribe(
    //    () => console.log('Update succeeded.'),
    //    error => console.error(error)
    //  );
    //}

    this.selectedIngredient = null;

    if (isNewIngredient && pressedEnter) {
      setTimeout(() => this.addIngredient(), 250);
    }
  }

  deleteIngredient(ingredient: IngredientDto) {
    if (this.ingredientDetailsModalRef) {
      this.ingredientDetailsModalRef.hide();
    }

    if (ingredient.id === 0) {
      const ingredientIndex = this.selectedCompletedOrder.ingredients.indexOf(this.selectedIngredient);
      this.selectedCompletedOrder.ingredients.splice(ingredientIndex, 1);
    } else {
      this.ingredientsClient.delete(ingredient.id).subscribe(
        () =>
        (this.selectedCompletedOrder.ingredients = this.selectedCompletedOrder.ingredients.filter(
          t => t.id !== ingredient.id
        )),
        error => console.error(error)
      );
    }
  }
}
