import {Injectable} from '@angular/core';
import {JwtHelperService} from "@auth0/angular-jwt";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Observable, Subject} from "rxjs";
import {AuthModel} from "../../Models/AuthModel";
import {AuthResponseModel} from "../../Models/AuthResponseModel";
import {Router} from "@angular/router";
import {EventBusService} from "../Shared/event-bus.service";
import {EventData} from "../Shared/EventData";

@Injectable({
  providedIn: 'root'
})
@Injectable()
export class AuthService {

  constructor(public jwtHelper: JwtHelperService, private http: HttpClient, public router: Router, private events: EventBusService) {
  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'my-auth-token'
    })
  };

  url: string = environment.apiUrl;

  public Login(authModel: AuthModel): Observable<AuthResponseModel> {
    return this.http.post<AuthResponseModel>(`${this.url}/user/login`, authModel, this.httpOptions);
  }

  public isAuthenticated(): boolean {
    let token = localStorage.getItem('token') ?? "";
    return !this.jwtHelper.isTokenExpired(token);
  }

  public getToken(): string | null {
    return localStorage.getItem('token');
  }

  public saveToken(token: string) {
    localStorage.setItem('token', token);
    this.events.emit(new EventData('login', true));
    this.router.navigate(['']);
  }

  public logout() {
    localStorage.removeItem('token');
    this.events.emit(new EventData('login', false));
    this.router.navigate(['login']);
  }
}
