import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  CookedRecipesClient,
  CookedRecipeDto,
  RecipeDto,
  SizeTypeDto,
  RecipesOptionVm,
  CreateCookedRecipeCommand
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
  newCookedRecipeEditor: any = {};
  cookedRecipeOptionsEditor: any = {};
  newCookedRecipeModalRef: BsModalRef;
  cookedRecipeOptionsModalRef: BsModalRef;
  deleteCookedRecipeModalRef: BsModalRef;

  constructor(
    private cookedRecipesClient: CookedRecipesClient,
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
}
