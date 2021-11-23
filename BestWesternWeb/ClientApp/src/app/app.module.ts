import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {ButtonModule} from "primeng/button";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {RippleModule} from "primeng/ripple";
import {SidebarModule} from "primeng/sidebar";
import {PanelMenuModule} from "primeng/panelmenu";
import {TopBarComponent} from './Components/UI/top-bar/top-bar.component';
import {BottomBarComponent} from './Components/UI/bottom-bar/bottom-bar.component';
import {ProgressBarModule} from "primeng/progressbar";
import {ConfigComponent} from './Components/config/config.component';
import {LogsComponent} from './Components/logs/logs.component';
import {AppRoutingModule} from './app-routing.module';
import {CardModule} from "primeng/card";
import {InputTextareaModule} from "primeng/inputtextarea";
import {FileUploadModule} from "primeng/fileupload";
import {HttpClientModule} from "@angular/common/http";
import {InputTextModule} from "primeng/inputtext";
import {PasswordModule} from "primeng/password";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {InputNumberModule} from "primeng/inputnumber";
import {CheckboxModule} from "primeng/checkbox";
import {ToastModule} from "primeng/toast";
import {MessageService} from "primeng/api";
import {ProgressSpinnerModule} from "primeng/progressspinner";
import {SliderModule} from "primeng/slider";
import {MenuModule} from "primeng/menu";
import {TableModule} from "primeng/table";
import {DropdownModule} from "primeng/dropdown";
import {CalendarModule} from "primeng/calendar";
import { LoginComponent } from './Auth/login/login.component';
import {JWT_OPTIONS, JwtHelperService} from "@auth0/angular-jwt";
import {PanelModule} from "primeng/panel";
import {authInterceptorProviders} from "./Interceptors/AuthInterceptor";
import {MultiSelectModule} from "primeng/multiselect";

@NgModule({
  declarations: [
    AppComponent,
    TopBarComponent,
    BottomBarComponent,
    ConfigComponent,
    LogsComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ButtonModule,
    RippleModule,
    SidebarModule,
    PanelMenuModule,
    ProgressBarModule,
    AppRoutingModule,
    CardModule,
    InputTextareaModule,
    FileUploadModule,
    HttpClientModule,
    InputTextModule,
    PasswordModule,
    FormsModule,
    InputNumberModule,
    CheckboxModule,
    ToastModule,
    ProgressSpinnerModule,
    SliderModule,
    MenuModule,
    TableModule,
    DropdownModule,
    CalendarModule,
    PanelModule,
    ReactiveFormsModule,
    MultiSelectModule
  ],
  providers: [
    MessageService,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService,
    authInterceptorProviders
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
