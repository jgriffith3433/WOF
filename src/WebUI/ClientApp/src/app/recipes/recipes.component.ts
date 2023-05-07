import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  RecipesClient,
  CalledIngredientsClient,
  RecipeDto,
  CalledIngredientDetailsVm,
  CalledIngredientDto,
  SizeTypeDto,
  CreateRecipeCommand,
  UpdateRecipeCommand,
  CreateCalledIngredientCommand,
  UpdateCalledIngredientCommand,
  UpdateCalledIngredientDetailsCommand
} from '../web-api-client';

@Component({
  selector: 'app-recipes-component',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.scss']
})
export class RecipesComponent implements OnInit {
  debug = false;
  recipes: RecipeDto[];
  sizeTypes: SizeTypeDto[];
  selectedRecipe: RecipeDto;
  selectedCalledIngredient: CalledIngredientDto;
  selectedCalledIngredientDetails: CalledIngredientDetailsVm;
  newRecipeEditor: any = {};
  recipeOptionsEditor: any = {};
  calledIngredientDetailsEditor: any = {};
  newRecipeModalRef: BsModalRef;
  recipeOptionsModalRef: BsModalRef;
  deleteRecipeModalRef: BsModalRef;
  calledIngredientDetailsModalRef: BsModalRef;

  constructor(
    private recipesClient: RecipesClient,
    private calledIngredientsClient: CalledIngredientsClient,
    private modalService: BsModalService
  ) { }

  ngOnInit(): void {
    this.recipesClient.get().subscribe(
      result => {
        this.recipes = result.recipes;
        this.sizeTypes = result.sizeTypes;
        if (this.recipes.length) {
          this.selectedRecipe = this.recipes[0];
        }
      },
      error => console.error(error)
    );
  }

  // Recipes
  remainingCalledIngredients(recipe: RecipeDto): number {
    return recipe.calledIngredients.filter(t => !t.name).length;
  }

  showNewRecipeModal(template: TemplateRef<any>): void {
    this.newRecipeModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('inputRecipeName').focus(), 250);
  }

  newRecipeCancelled(): void {
    this.newRecipeModalRef.hide();
    this.newRecipeEditor = {};
  }

  addRecipe(): void {
    const recipe = {
      id: 0,
      userImport: this.newRecipeEditor.userImport,
      name: this.newRecipeEditor.name,
      calledIngredients: []
    } as RecipeDto;

    this.recipesClient.create(recipe as CreateRecipeCommand).subscribe(
      result => {
        this.selectedRecipe = result;
        this.recipes.push(recipe);
        this.newRecipeModalRef.hide();
        this.newRecipeEditor = {};
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newRecipeEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('inputRecipeName').focus(), 250);
      }
    );
  }

  showRecipeOptionsModal(template: TemplateRef<any>) {
    this.recipeOptionsEditor = {
      id: this.selectedRecipe.id,
      userImport: this.selectedRecipe.userImport,
      name: this.selectedRecipe.name
    };

    this.recipeOptionsModalRef = this.modalService.show(template);
  }

  updateRecipeOptions() {
    const updateRecipeCommand = this.recipeOptionsEditor as UpdateRecipeCommand;
    this.recipesClient.update(this.selectedRecipe.id, updateRecipeCommand).subscribe(
      result => {
        this.selectedRecipe = result;

        this.recipeOptionsModalRef.hide();
        this.recipeOptionsEditor = {};
      },
      error => console.error(error)
    );
  }

  confirmDeleteRecipe(template: TemplateRef<any>) {
    this.recipeOptionsModalRef.hide();
    this.deleteRecipeModalRef = this.modalService.show(template);
  }

  deleteRecipeConfirmed(): void {
    this.recipesClient.delete(this.selectedRecipe.id).subscribe(
      () => {
        this.deleteRecipeModalRef.hide();
        this.recipes = this.recipes.filter(t => t.id !== this.selectedRecipe.id);
        this.selectedRecipe = this.recipes.length ? this.recipes[0] : null;
      },
      error => console.error(error)
    );
  }

  // CalledIngredients
  showCalledIngredientDetailsModal(template: TemplateRef<any>, calledIngredient: CalledIngredientDto): void {
    this.calledIngredientsClient.getCalledIngredientDetails(calledIngredient.id).subscribe(
      result => {
        this.selectedCalledIngredientDetails = result;
        for (var i = this.selectedRecipe.calledIngredients.length - 1; i >= 0; i--) {
          if (this.selectedRecipe.calledIngredients[i].id == this.selectedCalledIngredientDetails.id) {
            this.selectedRecipe.calledIngredients[i] = this.selectedCalledIngredientDetails;
            break;
          }
        }
        this.calledIngredientDetailsEditor = {
          ...this.selectedCalledIngredientDetails,
          search: this.selectedCalledIngredientDetails.name
        };

        this.calledIngredientDetailsModalRef = this.modalService.show(template);
      },
      error => console.error(error)
    );
  }


  searchIngredientName(): void {
    this.calledIngredientsClient.searchProductStockName(this.calledIngredientDetailsEditor.id, this.calledIngredientDetailsEditor.search).subscribe(
      result => {
        this.selectedCalledIngredientDetails = result;
        for (var i = this.selectedRecipe.calledIngredients.length - 1; i >= 0; i--) {
          if (this.selectedRecipe.calledIngredients[i].id == this.selectedCalledIngredientDetails.id) {
            this.selectedRecipe.calledIngredients[i] = this.selectedCalledIngredientDetails;
            break;
          }
        }
        var oldSearch = this.calledIngredientDetailsEditor.search;
        this.calledIngredientDetailsEditor = {
          ...this.selectedCalledIngredientDetails,
          search: oldSearch
        };
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.calledIngredientDetailsEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('search').focus(), 250);
      }
    );
  }

  updateCalledIngredientDetails(): void {
    const calledIngredient = this.calledIngredientDetailsEditor as UpdateCalledIngredientDetailsCommand;
    this.calledIngredientsClient.updateCalledIngredientDetails(this.selectedCalledIngredientDetails.id, calledIngredient).subscribe(
      result => {
        this.selectedCalledIngredientDetails = result;

        for (var i = this.selectedRecipe.calledIngredients.length - 1; i >= 0; i--) {
          if (this.selectedRecipe.calledIngredients[i].id == this.selectedCalledIngredientDetails.id) {
            this.selectedRecipe.calledIngredients[i] = this.selectedCalledIngredientDetails;
            break;
          }
        }

        this.calledIngredientDetailsModalRef.hide();
        this.calledIngredientDetailsEditor = {};
      },
      error => console.error(error)
    );
  }

  addCalledIngredient() {
    const calledIngredient = {
      id: 0,
      name: '',
      sizeType: this.sizeTypes[0].value
    } as CalledIngredientDto;

    this.selectedRecipe.calledIngredients.push(calledIngredient);
    const index = this.selectedRecipe.calledIngredients.length - 1;
    this.editCalledIngredient(calledIngredient, 'calledIngredientName' + index);
  }

  editCalledIngredient(calledIngredient: CalledIngredientDto, inputId: string): void {
    this.selectedCalledIngredient = calledIngredient;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateCalledIngredient(calledIngredient: CalledIngredientDto, pressedEnter: boolean = false): void {
    const isNewCalledIngredient = calledIngredient.id === 0;

    if (!calledIngredient.name.trim()) {
      this.deleteCalledIngredient(calledIngredient);
      return;
    }

    if (calledIngredient.id === 0) {
      this.calledIngredientsClient
        .create({
          ...calledIngredient, recipeId: this.selectedRecipe.id
        } as CreateCalledIngredientCommand)
        .subscribe(
          result => {
            calledIngredient.id = result;
          },
          error => console.error(error)
        );
    } else {
      this.calledIngredientsClient.update(calledIngredient.id, calledIngredient).subscribe(
        () => console.log('Update succeeded.'),
        error => console.error(error)
      );
    }

    this.selectedCalledIngredient = null;
    this.selectedCalledIngredientDetails = null;

    if (isNewCalledIngredient && pressedEnter) {
      setTimeout(() => this.addCalledIngredient(), 250);
    }
  }

  deleteCalledIngredient(calledIngredient: CalledIngredientDto) {
    if (this.calledIngredientDetailsModalRef) {
      this.calledIngredientDetailsModalRef.hide();
    }

    if (calledIngredient.id === 0) {
      const calledIngredientIndex = this.selectedRecipe.calledIngredients.indexOf(this.selectedCalledIngredient);
      this.selectedRecipe.calledIngredients.splice(calledIngredientIndex, 1);
    } else {
      this.calledIngredientsClient.delete(calledIngredient.id).subscribe(
        () =>
        (this.selectedRecipe.calledIngredients = this.selectedRecipe.calledIngredients.filter(
          t => t.id !== calledIngredient.id
        )),
        error => console.error(error)
      );
    }
  }
}
