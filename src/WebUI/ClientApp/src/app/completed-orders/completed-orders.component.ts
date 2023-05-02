import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  CompletedOrdersClient,
  //TodoItemsClient,
  CompletedOrderDto,
  //TodoItemDto,
  CreateCompletedOrderCommand,
  UpdateCompletedOrderCommand,
  //CreateTodoItemCommand, UpdateTodoItemCommand
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
  //selectedItem: TodoItemDto;
  newCompletedOrderEditor: any = {};
  completedOrderOptionsEditor: any = {};
  //itemDetailsEditor: any = {};
  newCompletedOrderModalRef: BsModalRef;
  completedOrderOptionsModalRef: BsModalRef;
  deleteCompletedOrderModalRef: BsModalRef;
  //itemDetailsModalRef: BsModalRef;

  constructor(
    private completedOrdersClient: CompletedOrdersClient,
    //private itemsClient: TodoItemsClient,
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

  // Lists
  //remainingItems(list: CompletedOrderDto): number {
  //  return completedOrders.items.filter(t => !t.done).length;
  //}

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
      //items: []
    } as CompletedOrderDto;

    this.completedOrdersClient.create(completedOrder as CreateCompletedOrderCommand).subscribe(
      result => {
        completedOrder.id = result;
        this.completedOrders.push(completedOrder);
        this.selectedCompletedOrder = completedOrder;
        this.newCompletedOrderModalRef.hide();
        this.newCompletedOrderEditor = {};
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

  // Items
  //showItemDetailsModal(template: TemplateRef<any>, item: TodoItemDto): void {
  //  this.selectedItem = item;
  //  this.itemDetailsEditor = {
  //    ...this.selectedItem
  //  };

  //  this.itemDetailsModalRef = this.modalService.show(template);
  //}

  //updateItemDetails(): void {
  //  const item = this.itemDetailsEditor as UpdateTodoItemCommand;
  //  this.itemsClient.updateItemDetails(this.selectedItem.id, item).subscribe(
  //    () => {
  //      if (this.selectedItem.listId !== this.itemDetailsEditor.listId) {
  //        this.selectedList.items = this.selectedList.items.filter(
  //          i => i.id !== this.selectedItem.id
  //        );
  //        const listIndex = this.lists.findIndex(
  //          l => l.id === this.itemDetailsEditor.listId
  //        );
  //        this.selectedItem.listId = this.itemDetailsEditor.listId;
  //        this.lists[listIndex].items.push(this.selectedItem);
  //      }

  //      this.selectedItem.priority = this.itemDetailsEditor.priority;
  //      this.selectedItem.note = this.itemDetailsEditor.note;
  //      this.itemDetailsModalRef.hide();
  //      this.itemDetailsEditor = {};
  //    },
  //    error => console.error(error)
  //  );
  //}

  //addItem() {
  //  const item = {
  //    id: 0,
  //    listId: this.selectedList.id,
  //    priority: this.priorityLevels[0].value,
  //    title: '',
  //    done: false
  //  } as TodoItemDto;

  //  this.selectedList.items.push(item);
  //  const index = this.selectedList.items.length - 1;
  //  this.editItem(item, 'itemTitle' + index);
  //}

  //editItem(item: TodoItemDto, inputId: string): void {
  //  this.selectedItem = item;
  //  setTimeout(() => document.getElementById(inputId).focus(), 100);
  //}

  //updateItem(item: TodoItemDto, pressedEnter: boolean = false): void {
  //  const isNewItem = item.id === 0;

  //  if (!item.title.trim()) {
  //    this.deleteItem(item);
  //    return;
  //  }

  //  if (item.id === 0) {
  //    this.itemsClient
  //      .create({
  //        ...item, listId: this.selectedList.id
  //      } as CreateTodoItemCommand)
  //      .subscribe(
  //        result => {
  //          item.id = result;
  //        },
  //        error => console.error(error)
  //      );
  //  } else {
  //    this.itemsClient.update(item.id, item).subscribe(
  //      () => console.log('Update succeeded.'),
  //      error => console.error(error)
  //    );
  //  }

  //  this.selectedItem = null;

  //  if (isNewItem && pressedEnter) {
  //    setTimeout(() => this.addItem(), 250);
  //  }
  //}

  //deleteItem(item: TodoItemDto) {
  //  if (this.itemDetailsModalRef) {
  //    this.itemDetailsModalRef.hide();
  //  }

  //  if (item.id === 0) {
  //    const itemIndex = this.selectedList.items.indexOf(this.selectedItem);
  //    this.selectedList.items.splice(itemIndex, 1);
  //  } else {
  //    this.itemsClient.delete(item.id).subscribe(
  //      () =>
  //      (this.selectedList.items = this.selectedList.items.filter(
  //        t => t.id !== item.id
  //      )),
  //      error => console.error(error)
  //    );
  //  }
  //}

}
