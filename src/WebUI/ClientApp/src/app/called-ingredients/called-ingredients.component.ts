import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CalledIngredientsClient, CalledIngredientDto } from '../web-api-client';

@Component({
  selector: 'app-called-ingredients',
  templateUrl: './called-ingredients.component.html'
})
export class CalledIngredientsComponent implements OnInit {
  public calledIngredients: CalledIngredientDto[];

  constructor(
    private client: CalledIngredientsClient,
    private router: Router
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  ngOnInit(): void {
    this.client.getCalledIngredients().subscribe(result => {
      this.calledIngredients = result.calledIngredients;
    }, error => console.error(error));
  }
}
