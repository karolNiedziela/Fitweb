import { environment } from './../environments/environment';
import { ErrorInterceptor } from './_helpers/error.interceptor';
import { BasicAuthInterceptor } from './_helpers/basic-auth.interceptor';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomepageComponent } from './homepage/homepage.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { SignComponent } from './sign/sign.component';
import { SignInComponent } from './sign/sign-in/sign-in.component';
import { SignUpComponent } from './sign/sign-up/sign-up.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UsersComponent } from './users/users.component';
import {
  SocialLoginModule,
  SocialAuthServiceConfig,
  FacebookLoginProvider,
} from 'angularx-social-login';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { JwtModule } from '@auth0/angular-jwt';
import { AlertComponent } from './alert/alert.component';
import { ProductsComponent } from './products/products.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { ExercisesComponent } from './exercises/exercises.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';
import { AccountComponent } from './account/account.component';
import { AccountOverviewComponent } from './account/account-overview/account-overview.component';
import { AccountProductsComponent } from './account/account-products/account-products.component';
import { AccountExercisesComponent } from './account/account-exercises/account-exercises.component';
import { CalendarComponent } from './calendar/calendar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

export function tokenGetter() {
  return localStorage.getItem('jwt');
}
@NgModule({
  declarations: [
    AppComponent,
    HomepageComponent,
    HeaderComponent,
    FooterComponent,
    SignComponent,
    SignInComponent,
    SignUpComponent,
    UsersComponent,
    EmailConfirmationComponent,
    AlertComponent,
    ExercisesComponent,
    ProductsComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    AccountComponent,
    AccountOverviewComponent,
    AccountProductsComponent,
    AccountExercisesComponent,
    CalendarComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    SocialLoginModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
      },
    }),
    NgxPaginationModule,
    NgbModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    // {
    //   provide: 'SocialAuthServiceConfig',
    //   useValue: {
    //     autoLogin: false,
    //     providers: [
    //       {
    //         id: FacebookLoginProvider.PROVIDER_ID,
    //         provider: new FacebookLoginProvider(
    //           `${environment.FACEBOOK_APP_ID}`
    //         ),
    //       },
    //     ],
    //   } as SocialAuthServiceConfig,
    // },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
