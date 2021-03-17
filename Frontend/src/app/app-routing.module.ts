import { UsersComponent } from './users/users.component';
import { AuthGuard } from './_helpers/auth.guard';
import { SignInComponent } from './sign/sign-in/sign-in.component';
import { SignComponent } from './sign/sign.component';
import { HomepageComponent } from './homepage/homepage.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignUpComponent } from './sign/sign-up/sign-up.component';

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
  { path: 'users', component: UsersComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
