import { Routes } from '@angular/router';
import {HomePageComponent} from './pages/home-page/home-page.component';
import {DocumentAnalyzerPageComponent} from './pages/document-analyzer-page/document-analyzer-page.component';
import {RegisterPageComponent} from './pages/login-register-pages/register-page/register-page.component';
import {LoginPageComponent} from './pages/login-register-pages/login-page/login-page.component';
import {MyAccountPageComponent} from './pages/my-account-page/my-account-page.component';
import {AuthGuard} from './services/auth-service/auth-guard';

export const routes: Routes = [

  { path: '', component: HomePageComponent },
  { path: 'analyzer', component: DocumentAnalyzerPageComponent,
    canActivate: [AuthGuard] },
  {path:'register', component: RegisterPageComponent},
  {path:'login', component: LoginPageComponent},
  {path: 'my-account', component: MyAccountPageComponent,
    canActivate: [AuthGuard]}
];
