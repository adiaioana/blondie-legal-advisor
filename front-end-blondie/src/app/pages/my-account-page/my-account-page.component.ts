import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service/auth-service.service';

@Component({
  selector: 'app-my-account-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-account-page.component.html',
  styleUrl: './my-account-page.component.css'
})
export class MyAccountPageComponent {
  userData = {
    username: "",
    email: "",
    firstName: "John",
    lastName: "Doe",
    phoneNumber: "+40 767 676 767",
    address: "Bulevardul Independentei 21, Iasi, IS 700001",
    birthdate: "2000-01-01",
    memberSince: "May 2025"
  };

  isEditing = false;
  loading = true;
  saveError = "";

  constructor(private authService: AuthService) { }

  ngOnInit() {
    console.log("Loading user data...");
    this.loadUserData();
  }

  loadUserData(): void {
    this.loading = true;
    this.authService.getCurrentUser().subscribe({
      next: (user) => {
        console.log("User data received:", user);
        this.userData = {
          username: user.username || '',
          email: user.email || '',
          // Keep default values for fields not in the backend
          firstName: "John",
          lastName: "Doe",
          phoneNumber: "+40 767 676 767",
          address: "Bulevardul Independentei 21, Iasi, IS 700001",
          birthdate: "2000-01-01",
          memberSince: "May 2025"
        };
        this.loading = false;
      },
      error: (error) => {
        console.error('Failed to load user data', error);
        this.loading = false;
      }
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
  }

  saveChanges(): void {
    // Only send username and email to the backend
    const updateData = {
      username: this.userData.username,
      email: this.userData.email
    };

    this.authService.updateUserProfile(updateData).subscribe({
      next: () => {
        console.log("Profile updated successfully");
        this.isEditing = false;
        this.loadUserData();
      },
      error: (error) => {
        console.error("Error updating profile", error);
        // Add this line to display error message to user
        this.saveError = error.error || "Failed to update profile. Please try again.";
      }
    });
  }
}
