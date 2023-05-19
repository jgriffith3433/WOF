import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ProductsComponent } from './products/products.component';
import { ProductStockComponent } from './product-stock/product-stock.component';
import { TodoComponent } from './todo/todo.component';
import { CompletedOrdersComponent } from './completed-orders/completed-orders.component';
import { RecipesComponent } from './recipes/recipes.component';
import { CookedRecipesComponent } from './cooked-recipes/cooked-recipes.component';
import { CalledIngredientsComponent } from './called-ingredients/called-ingredients.component';
import { TokenComponent } from './token/token.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'home', component: HomeComponent, redirectTo: '' },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'products', component: ProductsComponent, canActivate: [AuthorizeGuard] },
  { path: 'product-stock', component: ProductStockComponent, canActivate: [AuthorizeGuard] },
  { path: 'todo', component: TodoComponent, canActivate: [AuthorizeGuard] },
  { path: 'completed-orders', component: CompletedOrdersComponent, canActivate: [AuthorizeGuard] },
  { path: 'recipes', component: RecipesComponent, canActivate: [AuthorizeGuard] },
  { path: 'cooked-recipes', component: CookedRecipesComponent, canActivate: [AuthorizeGuard] },
  { path: 'called-ingredients', component: CalledIngredientsComponent, canActivate: [AuthorizeGuard] },
  { path: 'token', component: TokenComponent, canActivate: [AuthorizeGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
