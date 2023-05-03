import { Component } from '@angular/core';
import { IngredientsClient, IngredientBriefDto } from '../web-api-client';

@Component({
  selector: 'app-ingredients',
  templateUrl: './ingredients.component.html'
})
export class IngredientsComponent {
  public ingredients: IngredientBriefDto[];

  constructor(private client: IngredientsClient) {
    client.getIngredients().subscribe(result => {
      this.ingredients = result.ingredients;
    }, error => console.error(error));
  }
}
