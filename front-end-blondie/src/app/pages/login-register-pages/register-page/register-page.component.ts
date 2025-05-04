import { Component } from '@angular/core';
import {FormsModule, NgForm} from '@angular/forms';
import { AuthService } from '../../../services/auth-service/auth-service.service';
import {Router, RouterLink} from '@angular/router';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-register',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css'],
  standalone: true,
    imports: [
        FormsModule,
        CommonModule,
        RouterLink
    ]
})
export class RegisterPageComponent {
  user = { username: '', email: '', password: '' };
  errorMessage: string | null = null;

  // Define a regex pattern for password validation
  passwordPattern = '(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*]).{8,}'; // At least 8 chars, 1 uppercase, 1 number, 1 special char

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(form: NgForm) {
    if (form.invalid) {
      return;
    }

    const { username, email, password } = this.user;

    // Call the registration service to register the user with email
    this.authService.register(username, email, password).subscribe(
      (response) => {
        console.log('User registered:', response);
        this.router.navigate(['/login']); // Navigate to login page after successful registration
      },
      (error) => {
        // Handle errors like username already exists or other errors from the backend
        if (error.status === 400 && error.error === 'Username already exists') {
          this.errorMessage = 'Username already exists. Please choose a different one.';
        } else {
          this.errorMessage = error.error || 'Registration failed. Please try again.';
        }
        console.error('Error during registration:', error);
      }
    );
  }
}
