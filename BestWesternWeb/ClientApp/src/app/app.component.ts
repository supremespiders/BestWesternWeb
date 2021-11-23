import {Component, OnDestroy, OnInit} from '@angular/core';
import {MenuItem, PrimeNGConfig} from "primeng/api";
import {HubService} from "./Services/hub.service";
import {AuthService} from "./Services/Auth/auth.service";
import {Subscription} from "rxjs";
import {EventBusService} from "./Services/Shared/event-bus.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit,OnDestroy {
  isLoggedIn:boolean;
  eventsSub:Subscription;
  constructor(private primengConfig: PrimeNGConfig, private hubService: HubService
              ,private events:EventBusService,private auth:AuthService) {
  }

  ngOnInit() {
    this.primengConfig.ripple = true;
    this.eventsSub=this.events.on('login',(logged:boolean)=>{
      this.isLoggedIn=logged;
    });
    this.isLoggedIn=this.auth.isAuthenticated();
    let x=this.hubService.connection;
  }

  ngOnDestroy(): void {
    this.eventsSub.unsubscribe();
  }
}
