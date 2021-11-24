import {AfterViewInit, Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {Log} from "../../Models/Log";
import {ScraperService} from "../../Services/scraper.service";
import {Table} from "primeng/table";
import {FilterService} from "primeng/api";
import {HubService} from "../../Services/hub.service";

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit,AfterViewInit {
  @ViewChild('myTable') private _table: Table;
  @ViewChildren("logDiv") logDivs: QueryList<ElementRef>;
  logs:Log[];
  statuses: any;
  dateFilters: any;

  constructor(private scraper: ScraperService,private filterService: FilterService,private hub: HubService) {
    var _self=this;
    this.logs=[{
      id:1,
      timeStamp:new Date(),
      message:"m1",
      logLevel:"l1",
      callSite:"s1"
,    }];
    this.statuses = [
      {label: 'DEBUG', value: 'DEBUG'},
      {label: 'INFO', value: 'INFO'},
      {label: 'ERROR', value: 'ERROR'},
    ]
    // @ts-ignore
    this.filterService.register('dateRangeFilter', (value, filter): boolean => {
      // get the from/start value
      var s = _self.dateFilters[0].getTime();
      var e;
      if (_self.dateFilters[1]) {
        e = _self.dateFilters[1].getTime() + 86400000;
      } else {
        e = s + 86400000;
      }
      return value.getTime() >= s && value.getTime() <= e;
    });
  }

  ngOnInit(): void {
    this.scraper.GetLogs().subscribe(value => {
      this.logs=value;
      this.logs.forEach(x=>x.timeStamp=new Date(x.timeStamp));
    })

    this.hub.connection.on("Log", (x) => {
      this.logs.push(x);
    });
  }

  ngAfterViewInit() :void{
    this.logDivs.changes.subscribe(() => {
      if (this.logDivs && this.logDivs.last) {
        this.logDivs.last.nativeElement.focus();
      }
    });
  }

}
