import { NgModule } from '@angular/core';
import {ConfigComponent} from "./Components/config/config.component";
import {LogsComponent} from "./Components/logs/logs.component";
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./Auth/login/login.component";
import {AuthGuardService} from "./Services/Auth/auth-guard.service";
import {LoggedInAuthGuardService} from "./Services/Auth/logged-in-auth-guard.service";

const routes: Routes = [
  { path: '', component: ConfigComponent ,canActivate: [AuthGuardService] },
  { path: 'logs', component: LogsComponent,canActivate: [AuthGuardService]  },
  { path: 'login', component: LoginComponent,canActivate:[LoggedInAuthGuardService] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
