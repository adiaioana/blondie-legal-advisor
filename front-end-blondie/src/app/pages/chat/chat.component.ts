import { Component, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
  imports: [CommonModule, FormsModule],
  standalone: true
})
export class ChatComponent implements AfterViewChecked {
  messages: { sender: 'user' | 'assistant', text: string }[] = [];
  userInput: string = '';

  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;

  sendMessage() {
    if (!this.userInput.trim()) return;

    // Add user message
    this.messages.push({ sender: 'user', text: this.userInput });

    // Clear input
    this.userInput = '';

    // Add mock assistant response (optional)
    setTimeout(() => {
      this.messages.push({ sender: 'assistant', text: 'This is a mock response.' });
    }, 500);
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    } catch(err) {}
  }
}
