import {Component, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {AuthService} from '../../services/auth-service/auth-service.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  imports: [CommonModule, RouterLink, RouterLinkActive],
  standalone: true
})
export class MenuComponent implements OnInit{
  isExpanded = false;
  isLoggedIn = false;
  constructor(private authService: AuthService, protected router: Router) {}

  ngOnInit() {
    this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;
    });

    console.log('maybeee', this.isLoggedIn)
  }
  toggleMenu() {
    this.isExpanded = !this.isExpanded;
  }
  toggleTheme() {
    const body = document.body;
    body.classList.toggle('light-mode');
  }

  logout() {
    this.authService.logout();
  }

}
