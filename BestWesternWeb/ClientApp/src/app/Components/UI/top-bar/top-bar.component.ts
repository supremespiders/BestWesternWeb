import { Component, OnInit } from '@angular/core';
import {MenuItem} from "primeng/api";
import {AuthService} from "../../../Services/Auth/auth.service";

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {
  display: boolean=false;
   items: MenuItem[];
  constructor(private auth:AuthService) {
   this.items = [{
      label: 'Config',
      icon: 'pi pi-search',
      routerLink:'/',
     command: (event) => {
       this.display=false;
     }
    },
     {
       label: 'Logs',
       icon: 'pi pi-user',
       routerLink:'/logs',
       command: (event) => {
         this.display=false;
       }
     }];
}

  ngOnInit(): void {
  }

  logout()
  {
    this.auth.logout();
  }

}
