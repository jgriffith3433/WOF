import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  CookedRecipesClient,
  CookedRecipeCalledIngredientsClient,
  CookedRecipeDto,
  CookedRecipeCalledIngredientDetailsVm,
  RecipeDto,
  SizeTypeDto,
  RecipesOptionVm,
  CreateCookedRecipeCommand,
  CreateCookedRecipeCalledIngredientCommand,
  UpdateCookedRecipeCalledIngredientDetailsCommand
} from '../web-api-client';

@Component({
  selector: 'app-cooked-recipes-component',
  templateUrl: './cooked-recipes.component.html',
  styleUrls: ['./cooked-recipes.component.scss']
})
export class CookedRecipesComponent implements OnInit {
  debug = false;
  recipesOptions: RecipesOptionVm[];
  cookedRecipes: CookedRecipeDto[];
  sizeTypes: SizeTypeDto[];
  selectedCookedRecipe: CookedRecipeDto;
  selectedCookedRecipeCalledIngredient: CookedRecipeCalledIngredientDetailsVm;
  selectedCookedRecipeCalledIngredientDetails: CookedRecipeCalledIngredientDetailsVm;
  newCookedRecipeEditor: any = {};
  cookedRecipeOptionsEditor: any = {};
  cookedRecipeCalledIngredientDetailsEditor: any = {};
  newCookedRecipeModalRef: BsModalRef;
  cookedRecipeOptionsModalRef: BsModalRef;
  deleteCookedRecipeModalRef: BsModalRef;
  cookedRecipeCalledIngredientDetailsModalRef: BsModalRef;


  constructor(
    private cookedRecipesClient: CookedRecipesClient,
    private cookedRecipesCalledIngredientsClient: CookedRecipeCalledIngredientsClient,
    private modalService: BsModalService
  ) { }

  ngOnInit(): void {
    this.cookedRecipesClient.get().subscribe(
      result => {
        this.cookedRecipes = result.cookedRecipes;
        this.sizeTypes = result.sizeTypes;
        this.recipesOptions = result.recipesOptions;
        if (this.cookedRecipes.length) {
          this.selectedCookedRecipe = this.cookedRecipes[0];
        }
      },
      error => console.error(error)
    );
  }

  // CookedRecipes
  //remainingCalledIngredients(recipe: RecipeDto): number {
  //  return recipe.calledIngredients.filter(t => !t.name).length;
  //}

  showNewCookedRecipeModal(template: TemplateRef<any>): void {
    this.newCookedRecipeModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('inputCookedRecipeName').focus(), 250);
  }

  newCookedRecipeCancelled(): void {
    this.newCookedRecipeModalRef.hide();
    this.newCookedRecipeEditor = {};
  }

  addCookedRecipe(): void {
    let recipeName: string;
    for (var recipesOption of this.recipesOptions) {
      if (recipesOption.value == this.newCookedRecipeEditor.recipeId) {
        recipeName = recipesOption.name;
        break;
      }
    }
    const cookedRecipe = {
      id: 0,
      recipeId: this.newCookedRecipeEditor.recipeId,
      recipe: new RecipeDto({ name: recipeName })
    } as CookedRecipeDto;

    this.cookedRecipesClient.create(cookedRecipe as CreateCookedRecipeCommand).subscribe(
      result => {
        this.selectedCookedRecipe = result;
        this.cookedRecipes.push(cookedRecipe);
        this.newCookedRecipeModalRef.hide();
        this.newCookedRecipeEditor = {};
      },
      error => {
        this.newCookedRecipeEditor.errorResponse = JSON.parse(error.response);
      }
    );
  }

  showCookedRecipeOptionsModal(template: TemplateRef<any>) {
    this.cookedRecipeOptionsEditor = {
      id: this.selectedCookedRecipe.id,
      recipeId: this.selectedCookedRecipe.recipeId
    };

    this.cookedRecipeOptionsModalRef = this.modalService.show(template);
  }

  confirmDeleteCookedRecipe(template: TemplateRef<any>) {
    this.cookedRecipeOptionsModalRef.hide();
    this.deleteCookedRecipeModalRef = this.modalService.show(template);
  }

  deleteCookedRecipeConfirmed(): void {
    this.cookedRecipesClient.delete(this.selectedCookedRecipe.id).subscribe(
      () => {
        this.deleteCookedRecipeModalRef.hide();
        this.cookedRecipes = this.cookedRecipes.filter(t => t.id !== this.selectedCookedRecipe.id);
        this.selectedCookedRecipe = this.cookedRecipes.length ? this.cookedRecipes[0] : null;
      },
      error => console.error(error)
    );
  }

  //CookedRecipeCalledIngredients

  showCookedRecipeCalledIngredientDetailsModal(template: TemplateRef<any>, cookedRecipeCalledIngredient: CookedRecipeCalledIngredientDetailsVm): void {
    this.cookedRecipesCalledIngredientsClient.getCookedRecipeCalledIngredientDetails(cookedRecipeCalledIngredient.id).subscribe(
      result => {
        this.selectedCookedRecipeCalledIngredientDetails = result;
        for (var i = this.selectedCookedRecipe.cookedRecipeCalledIngredients.length - 1; i >= 0; i--) {
          if (this.selectedCookedRecipe.cookedRecipeCalledIngredients[i].id == this.selectedCookedRecipeCalledIngredientDetails.id) {
            this.selectedCookedRecipe.cookedRecipeCalledIngredients[i] = this.selectedCookedRecipeCalledIngredientDetails;
            break;
          }
        }
        this.cookedRecipeCalledIngredientDetailsEditor = {
          ...this.selectedCookedRecipeCalledIngredientDetails,
          search: this.selectedCookedRecipeCalledIngredientDetails.name
        };

        this.cookedRecipeCalledIngredientDetailsModalRef = this.modalService.show(template);
      },
      error => console.error(error)
    );
  }

  searchIngredientName(): void {
    this.cookedRecipesCalledIngredientsClient.searchProductStockName(this.cookedRecipeCalledIngredientDetailsEditor.id, this.cookedRecipeCalledIngredientDetailsEditor.search).subscribe(
      result => {
        this.cookedRecipeCalledIngredientDetailsEditor.productStockSearchItems = result.productStockSearchItems;
      },
      error => {
        this.cookedRecipeCalledIngredientDetailsEditor.errorResponse = JSON.parse(error.response);
      }
    );
  }

  updateCookedRecipeCalledIngredientDetails(): void {
    const cookedRecipeCalledIngredient = this.cookedRecipeCalledIngredientDetailsEditor as UpdateCookedRecipeCalledIngredientDetailsCommand;
    this.cookedRecipesCalledIngredientsClient.updateCookedRecipeCalledIngredientDetails(this.selectedCookedRecipeCalledIngredientDetails.id, cookedRecipeCalledIngredient).subscribe(
      result => {
        this.selectedCookedRecipeCalledIngredientDetails = result;

        for (var i = this.selectedCookedRecipe.cookedRecipeCalledIngredients.length - 1; i >= 0; i--) {
          if (this.selectedCookedRecipe.cookedRecipeCalledIngredients[i].id == this.selectedCookedRecipeCalledIngredientDetails.id) {
            this.selectedCookedRecipe.cookedRecipeCalledIngredients[i] = this.selectedCookedRecipeCalledIngredientDetails;
            break;
          }
        }

        this.cookedRecipeCalledIngredientDetailsModalRef.hide();
        this.cookedRecipeCalledIngredientDetailsEditor = {};
      },
      error => console.error(error)
    );
  }

  addCookedRecipeCalledIngredient() {
    const cookedRecipeCalledIngredient = {
      id: 0,
      name: '',
      sizeType: this.sizeTypes[0].value,
      units: 0,
      cookedRecipeId: this.selectedCookedRecipe.id,
    } as CookedRecipeCalledIngredientDetailsVm;

    this.selectedCookedRecipe.cookedRecipeCalledIngredients.push(cookedRecipeCalledIngredient);
    const index = this.selectedCookedRecipe.cookedRecipeCalledIngredients.length - 1;
    this.editCookedRecipeCalledIngredient(cookedRecipeCalledIngredient, 'cookedRecipeCalledIngredientName' + index);
  }

  editCookedRecipeCalledIngredient(cookedRecipeCalledIngredient: CookedRecipeCalledIngredientDetailsVm, inputId: string): void {
    this.selectedCookedRecipeCalledIngredient = cookedRecipeCalledIngredient;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateCookedRecipeCalledIngredient(cookedRecipeCalledIngredient: CookedRecipeCalledIngredientDetailsVm, pressedEnter: boolean = false): void {
    const isNewCalledIngredient = cookedRecipeCalledIngredient.id === 0;

    if (!cookedRecipeCalledIngredient.name.trim()) {
      this.deleteCookedRecipeCalledIngredient(cookedRecipeCalledIngredient);
      return;
    }

    if (cookedRecipeCalledIngredient.id === 0) {
      this.cookedRecipesCalledIngredientsClient
        .create({
          ...cookedRecipeCalledIngredient, cookedRecipeId: this.selectedCookedRecipe.id
        } as CreateCookedRecipeCalledIngredientCommand)
        .subscribe(
          result => {
            cookedRecipeCalledIngredient.id = result;
          },
          error => console.error(error)
        );
    } else {
      this.cookedRecipesCalledIngredientsClient.update(cookedRecipeCalledIngredient.id, cookedRecipeCalledIngredient).subscribe(
        () => console.log('Update succeeded.'),
        error => console.error(error)
      );
    }

    this.selectedCookedRecipeCalledIngredient = null;
    this.selectedCookedRecipeCalledIngredientDetails = null;

    if (isNewCalledIngredient && pressedEnter) {
      setTimeout(() => this.addCookedRecipeCalledIngredient(), 250);
    }
  }

  deleteCookedRecipeCalledIngredient(cookedRecipeCalledIngredient: CookedRecipeCalledIngredientDetailsVm) {
    if (this.cookedRecipeCalledIngredientDetailsModalRef) {
      this.cookedRecipeCalledIngredientDetailsModalRef.hide();
    }

    if (cookedRecipeCalledIngredient.id === 0) {
      const cookedRecipeCalledIngredientIndex = this.selectedCookedRecipe.cookedRecipeCalledIngredients.indexOf(this.selectedCookedRecipeCalledIngredient);
      this.selectedCookedRecipe.cookedRecipeCalledIngredients.splice(cookedRecipeCalledIngredientIndex, 1);
    } else {
      this.cookedRecipesCalledIngredientsClient.delete(cookedRecipeCalledIngredient.id).subscribe(
        () =>
        (this.selectedCookedRecipe.cookedRecipeCalledIngredients = this.selectedCookedRecipe.cookedRecipeCalledIngredients.filter(
          t => t.id !== cookedRecipeCalledIngredient.id
        )),
        error => console.error(error)
      );
    }
  }
}
