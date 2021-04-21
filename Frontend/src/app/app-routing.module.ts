import { CalendarComponent } from './calendar/calendar.component';
import { AccountOverviewComponent } from './account/account-overview/account-overview.component';
import { AccountProductsComponent } from './account/account-products/account-products.component';
import { AccountComponent } from './account/account.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ExercisesComponent } from './exercises/exercises.component';
import { ProductsComponent } from './products/products.component';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { UsersComponent } from './users/users.component';
import { AuthGuard } from './_helpers/auth.guard';
import { SignInComponent } from './sign/sign-in/sign-in.component';
import { SignComponent } from './sign/sign.component';
import { HomepageComponent } from './homepage/homepage.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignUpComponent } from './sign/sign-up/sign-up.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';
import { AccountExercisesComponent } from './account/account-exercises/account-exercises.component';

const routes: Routes = [
  { path: '', component: HomepageComponent },
  { path: 'home', component: HomepageComponent },
  {
    path: 'sign',
    component: SignComponent,
    children: [
      { path: 'in', component: SignInComponent },
      { path: 'up', component: SignUpComponent },
    ],
  },
  { path: 'confirmemail', component: EmailConfirmationComponent },
  { path: 'users', component: UsersComponent, canActivate: [AuthGuard] },
  { path: 'products', component: ProductsComponent },
  { path: 'exercises', component: ExercisesComponent },
  { path: 'forgotpassword', component: ForgotPasswordComponent },
  { path: 'resetpassword', component: ResetPasswordComponent },
  { path: 'calendar', component: CalendarComponent },
  {
    path: 'account',
    component: AccountComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'overview', component: AccountOverviewComponent },
      { path: 'products', component: AccountProductsComponent },
      { path: 'exercises', component: AccountExercisesComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
