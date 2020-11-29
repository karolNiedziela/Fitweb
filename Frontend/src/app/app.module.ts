import { DietgoalService } from './shared/dietgoal.service';

import { AccountService } from './shared/account.service';
import { AppFilterPipe } from './pipes/appFilter.pipe';

import { UserExerciseService } from './shared/user-exercise.service';
import { UserProductService } from './shared/user-product.service';
import { ExerciseService } from './shared/exercise.service';
import { ProductService } from './shared/product.service';
import { RouterModule } from '@angular/router';
import { UserService } from './shared/user.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxPaginationModule } from 'ngx-pagination';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { Ng2OrderModule } from 'ng2-order-pipe'; 

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AccountComponent } from './account/account.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { ProductComponent } from './product/product.component';
import { ExerciseComponent } from './exercise/exercise.component';
import { AccountExercisesComponent } from './account/account-exercises/account-exercises.component';
import { AccountExerciseComponent } from './account/account-exercises/account-exercise/account-exercise.component';
import { AccountExerciseListComponent } from './account/account-exercises/account-exercise-list/account-exercise-list.component';
import { AccountDetailComponent } from './account/account-detail/account-detail.component';
import { AccountProductsComponent } from './account/account-products/account-products.component';
import { AccountProductComponent } from './account/account-products/account-product/account-product.component';
import { AccountProductListComponent } from './account/account-products/account-product-list/account-product-list.component';
import { EditProfileComponent } from './account/edit-profile/edit-profile.component';
import { EditPasswordComponent } from './account/edit-password/edit-password.component';
import { CaloriesComponent } from './account/calories/calories.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { DietgoalComponent } from './account/dietgoal/dietgoal.component';
import { EditProductComponent } from './account/account-products/edit-product/edit-product.component';
import { EditExerciseComponent } from './account/account-exercises/edit-exercise/edit-exercise.component';
import { UsersComponent } from './admin-panel/users/users.component';
import { ProductsComponent } from './admin-panel/products/products.component';
import { ExercisesComponent } from './admin-panel/exercises/exercises.component';
import { UserProductsComponent } from './admin-panel/user-products/user-products.component';
import { DayService } from './shared/day.service';
import { AddProductComponent } from './admin-panel/products/add-product/add-product.component';
import { AdminEditProductComponent } from './admin-panel/products/admin-edit-product/admin-edit-product.component';
import { AddExerciseComponent } from './admin-panel/exercises/add-exercise/add-exercise.component';
import { BmrCalculatorComponent } from './bmr-calculator/bmr-calculator.component';
import { UserExercisesComponent } from './admin-panel/user-exercises/user-exercises.component';

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    UserComponent,
    LoginComponent,
    HomeComponent,
    ForbiddenComponent,
    HeaderComponent,
    FooterComponent,
    AccountComponent,
    ProductComponent,
    ExerciseComponent,
    AccountExercisesComponent,
    AccountExerciseComponent,
    AccountExerciseListComponent,
    AccountDetailComponent,
    AccountProductsComponent,
    AccountProductComponent,
    AccountProductListComponent,
    AppFilterPipe,
    EditProfileComponent,
    EditPasswordComponent,
    CaloriesComponent,
    AdminPanelComponent,
    DietgoalComponent,
    EditProductComponent,
    EditExerciseComponent,
    UsersComponent,
    ProductsComponent,
    ExercisesComponent,
    UserProductsComponent,
    AddProductComponent,
    AdminEditProductComponent,
    AddExerciseComponent,
    BmrCalculatorComponent,
    UserExercisesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot({
      progressBar: true
    }), // ToastrModule added
    FormsModule,
    FontAwesomeModule,
    NgxPaginationModule,
    Ng2SearchPipeModule,
    Ng2OrderModule,
    RouterModule
  ],
  providers: [UserService, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  },
  ProductService,
  ExerciseService,
  UserProductService,
  UserExerciseService,
  AccountService,
  HeaderComponent,
  DietgoalService,
  DayService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
