import {Injectable} from "@angular/core";
import {
  HTTP_INTERCEPTORS, HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from "@angular/common/http";
import {Observable, throwError} from "rxjs";
import {AuthService} from "../Services/Auth/auth.service";
import {catchError} from "rxjs/operators";


@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<Object>> {

    let token = this.authService.getToken();
    let newReq = req.clone({headers: req.headers.set('Authorization', 'Bearer ' + token)});
    return next.handle(newReq).pipe(catchError(error => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        console.log("intercepted the 401");
        this.authService.logout();
      }
      return throwError(error);
    }));
  }
}

export const authInterceptorProviders = [
  {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}
];
