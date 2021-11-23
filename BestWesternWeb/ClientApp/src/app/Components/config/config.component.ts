import {Component, OnInit} from '@angular/core';
import {ScraperService} from "../../Services/scraper.service";
import {ScraperConfig} from "../../Models/ScraperConfig";
import {MessageService} from 'primeng/api';
import {FileUpload} from "primeng/fileupload";

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.css']
})
export class ConfigComponent implements OnInit {
  uploadedFiles: any[] = [];
  uplo: File;
  config: ScraperConfig = new ScraperConfig();
  loaded: boolean = false;

  constructor(private scraper: ScraperService, private messageService: MessageService) {
  }

  ngOnInit(): void {
    this.scraper.GetConfig().subscribe(value => {
      this.config = value;
      this.loaded = true;
      console.log("config " + JSON.stringify(value));
    }, error => {
      console.log("err" + JSON.stringify(error));
      this.loaded = true;
    })
  }

  onUpload(event: FileUpload) {
    for (let file of event.files) {
      this.uplo = file;
    }
    this.uploadFileToActivity();
  }

  uploadFileToActivity() {
    this.scraper.uploadFile(this.uplo).subscribe(data => {
      this.messageService.add({
        severity: 'info',
        summary: 'File Uploaded',
        detail: ''
      });
    }, error => {
      this.messageService.add({
        severity: 'error',
        summary: 'File Uploaded',
        detail: 'Error uploading : '+error.error
      });
      console.log(error);
    });
  }


  async startWork() {
    let s = await this.scraper.Start();
    console.log(s);
  }

  async stopWork() {
    await this.scraper.Cancel();
  }

  async saveConfig() {
    this.scraper.SaveConfig(this.config).subscribe(value => {
      console.log("m " + JSON.stringify(value));
      this.messageService.add({severity: 'success', summary: 'Success', detail: 'Configuration saved'});
    }, error => {
      console.log("Error " + error);
    })
  }

}
