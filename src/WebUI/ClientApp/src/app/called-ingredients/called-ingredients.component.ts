import { Component } from '@angular/core';
import { CalledIngredientsClient, CalledIngredientDto } from '../web-api-client';

@Component({
  selector: 'app-called-ingredients',
  templateUrl: './called-ingredients.component.html'
})
export class CalledIngredientsComponent {
  public calledIngredients: CalledIngredientDto[];

  constructor(private client: CalledIngredientsClient) {
    client.getCalledIngredients().subscribe(result => {
      this.calledIngredients = result.calledIngredients;
    }, error => console.error(error));
  }
}
