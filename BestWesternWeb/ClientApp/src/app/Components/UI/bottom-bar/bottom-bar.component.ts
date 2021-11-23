import { Component, OnInit } from '@angular/core';
import {HubService} from "../../../Services/hub.service";

@Component({
  selector: 'app-bottom-bar',
  templateUrl: './bottom-bar.component.html',
  styleUrls: ['./bottom-bar.component.css']
})
export class BottomBarComponent implements OnInit {
  isWorking: boolean=false;
  log: string="";
  progress:number=0;

  constructor(private hub: HubService) { }

  ngOnInit(): void {
    this.hub.connection.on("ScraperStatus", (x) => {
      this.isWorking = x;
    });
    this.hub.connection.on("Log", (x) => {
      this.log = x;
    });
    this.hub.connection.on("Progress", (x,total) => {
      this.progress = x*100/total;
    });
    this.hub.connection.on("Error", (x) => {
      this.log = x;
    });
    this.hub.connection.on("Display", (x) => {
      this.log = x;
    });
  }


}
