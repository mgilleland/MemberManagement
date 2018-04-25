import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/RX';
import { environment } from '../../../environments/environment';
import { IMember } from "./member.model";
import 'rxjs/add/operator/catch';

@Injectable()
export class MemberService {

  public members: IMember[];

  constructor(private http: HttpClient) { }

  getMembers(): Observable<IMember[]> {
    return this.http.get<IMember[]>(`${environment.memberApi}api/Member`);
  }

  getMember(id: number) {
    return this.http.get<IMember>(`${environment.memberApi}api/Member/${id}`);
  }

  addMember(member: IMember) {
    let options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post(`${environment.memberApi}api/Member`, member, options);
  }

  updateMember(member: IMember) {
    let options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put(`${environment.memberApi}api/Member/${member.id}`, member, options);
  }

  deleteMember(id: number) {
    let options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.delete(`${environment.memberApi}api/Member/${id}`, options);
  }

  isUserNameUnique(userName: string) {
    return this.http.get<boolean>(`${environment.memberApi}api/Member/isUserNameUnique/${userName}`);
  }

  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.statusText);
  }
}
