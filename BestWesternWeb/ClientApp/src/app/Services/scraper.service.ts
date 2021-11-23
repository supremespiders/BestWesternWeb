import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {ScraperConfig} from "../Models/ScraperConfig";
import {Log} from "../Models/Log";

@Injectable({
  providedIn: 'root'
})
export class ScraperService {

  constructor(private http: HttpClient) {
  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'my-auth-token'
    })
  };

  url: string=environment.apiUrl;

  // GetFollowsStatus(): Observable<FollowStatus> {
  //   return this.http.get<FollowStatus>(`${this.url}/FollowersStatus`, this.httpOptions);
  // }

  Start(): Promise<void> {
    return this.http.get <void>(`${this.url}/scraper/start`, this.httpOptions).toPromise();
  }
  Cancel(): Promise<void> {
    return this.http.get <void>(`${this.url}/scraper/cancel`, this.httpOptions).toPromise();
  }
  GetConfig(): Observable<ScraperConfig> {
    return this.http.get <ScraperConfig>(`${this.url}/user/getConfig`, this.httpOptions);
  }
  SaveConfig(config:ScraperConfig): Observable<void> {
    return this.http.post<void>(`${this.url}/user/saveConfig`, config, this.httpOptions);
  }
  GetLogs(): Observable<Log[]> {
    return this.http.get <Log[]>(`${this.url}/scraper/getLogs`, this.httpOptions);
  }
  uploadFile(file :File) {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post(`${this.url}/api/uploader`, formData);
  }
}
