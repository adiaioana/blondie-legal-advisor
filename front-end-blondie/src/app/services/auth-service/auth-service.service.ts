import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import {environment} from '../../envinronment';

interface LoginResponse {
  token: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.apiUrl;
  private jwtTokenKey = 'some';

  private loggedIn = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient) {
    this.loggedIn.next(!!this.getToken());
  }
  register(username: string, email: string, password: string): Observable<any> {
    const body = { username, email, password };
    return this.http.post(`${this.baseUrl}/register`, body, { withCredentials: true });
  }


  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, { username, password }, { withCredentials: true }).pipe(
      tap(response => {
        this.setToken(response.token);
        this.loggedIn.next(true);
      })
    );
  }

  logout() {
    localStorage.removeItem(this.jwtTokenKey);
    this.loggedIn.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.jwtTokenKey);
  }

  private setToken(token: string) {
    localStorage.setItem(this.jwtTokenKey, token);
  }

  getCurrentUser(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/user/current`);
  }

  updateUserProfile(userData: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/user/update`, userData);
  }
}
