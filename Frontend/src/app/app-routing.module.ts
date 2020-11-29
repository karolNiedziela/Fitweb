import { UserExercisesComponent } from './admin-panel/user-exercises/user-exercises.component';
import { BmrCalculatorComponent } from './bmr-calculator/bmr-calculator.component';
import { AddExerciseComponent } from './admin-panel/exercises/add-exercise/add-exercise.component';
import { AdminEditProductComponent } from './admin-panel/products/admin-edit-product/admin-edit-product.component';
import { AddProductComponent } from './admin-panel/products/add-product/add-product.component';
import { UserProductsComponent } from './admin-panel/user-products/user-products.component';
import { ProductsComponent } from './admin-panel/products/products.component';
import { UsersComponent } from './admin-panel/users/users.component';

import { EditExerciseComponent } from './account/account-exercises/edit-exercise/edit-exercise.component';
import { EditProductComponent } from './account/account-products/edit-product/edit-product.component';
import { DietgoalComponent } from './account/dietgoal/dietgoal.component';


import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { CaloriesComponent } from './account/calories/calories.component';
import { EditPasswordComponent } from './account/edit-password/edit-password.component';
import { EditProfileComponent } from './account/edit-profile/edit-profile.component';
import { AccountProductsComponent } from './account/account-products/account-products.component';
import { AccountDetailComponent } from './account/account-detail/account-detail.component';
import { AccountExerciseComponent } from './account/account-exercises/account-exercise/account-exercise.component';
import { ProductComponent } from './product/product.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { UserService } from './shared/user.service';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { AuthGuard } from './auth/auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { AccountComponent } from './account/account.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { ExerciseComponent } from './exercise/exercise.component';
import { AccountExercisesComponent } from './account/account-exercises/account-exercises.component';
import { ExercisesComponent } from './admin-panel/exercises/exercises.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'home', component: HomeComponent},
  {
    path: 'user',
    component: UserComponent,
    children: [
      { path: 'registration', component: RegistrationComponent },
      { path: 'login', component: LoginComponent },
    ],
  },
  { path: 'admin-panel', component: AdminPanelComponent, canActivate: [AuthGuard], data: { permittedRoles: ['Admin']}, children: [
    { path: 'users', component: UsersComponent },
    { path: 'products', component: ProductsComponent},
    { path: 'add-product', component: AddProductComponent },
    { path: 'edit-product', component: AdminEditProductComponent },
    { path: 'add-exercise', component: AddExerciseComponent },
    { path: 'exercises', component: ExercisesComponent },
    { path: 'user-products', component: UserProductsComponent },
    { path: 'user-exercises', component: UserExercisesComponent }
  ]},
  { path: 'account', component: AccountComponent, canActivate: [AuthGuard], children: [
    { path: 'account-detail', component: AccountDetailComponent },
    { path: 'account-exercises', component: AccountExercisesComponent },
    { path: 'edit-exercise', component: EditExerciseComponent },
    { path: 'account-products', component: AccountProductsComponent },
    { path: 'edit-product', component: EditProductComponent },
    { path: 'edit-profile', component: EditProfileComponent },
    { path: 'edit-password', component: EditPasswordComponent },
    { path: 'calories', component: CaloriesComponent },
    { path: 'dietgoal', component: DietgoalComponent }
    ]
  },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'products', component: ProductComponent },
  { path: 'exercises', component: ExerciseComponent },
  { path: 'bmr-calculator', component: BmrCalculatorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation: `reload`, paramsInheritanceStrategy: 'always'}, )],
  exports: [RouterModule],
})
export class AppRoutingModule {}
