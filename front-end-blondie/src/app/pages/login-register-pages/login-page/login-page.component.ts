import { Component } from '@angular/core';
import {FormsModule, NgForm} from '@angular/forms';
import { AuthService } from '../../../services/auth-service/auth-service.service';
import {Router, RouterLink} from '@angular/router';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    RouterLink
  ]
})
export class LoginPageComponent {
  user = { username: '', password: '' };
  errorMessage: string | null = null;

  // Define a regex pattern for password validation
  passwordPattern = '(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*]).{8,}'; // At least 8 chars, 1 uppercase, 1 number, 1 special char

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(form: NgForm) {
    if (form.invalid) {
      return;
    }

    const { username,password } = this.user;

    this.authService.login(username,password).subscribe(
      (response) => {
        console.log('User logged in:', response);
        this.router.navigate(['/my-account']); // Navigate to login page after successful registration
      },
      (error) => {
        // Handle errors like username already exists or other errors from the backend
        if (error.status === 403 ) {
          this.errorMessage = 'Username or password not right.';
        } else {
          this.errorMessage = error.error || 'Log in failed. Please try again.';
        }
        console.error('Error during registration:', error);
      }
    );
  }
}
