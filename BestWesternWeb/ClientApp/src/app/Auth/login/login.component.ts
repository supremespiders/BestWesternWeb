import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../Services/Auth/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  error:string;

  constructor(private auth:AuthService) {
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      'username': new FormControl('', Validators.required),
      'password': new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    this.submitted = true;
    this.auth.Login({username:this.loginForm.value.username,password:this.loginForm.value.password}).subscribe(resp => {
      console.log(`login ${JSON.stringify(resp)}`);
      this.auth.saveToken(resp.token);
      console.log(this.auth.isAuthenticated());
    },error => {
      this.error=error.error.errorMessage;
      console.log("Error "+JSON.stringify(error.error.errorMessage));
    })
  }
}
