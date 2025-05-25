import {
  Component,
  ViewChild,
  ElementRef,
  AfterViewChecked,
  ViewEncapsulation,
  OnInit
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import {environment} from '../../envinronment';
import { SpeechToTextService } from '../../services/speech-to-text-service/speech-to-text.service';
import { TextToSpeechServiceService, ConversationPair } from '../../services/text-to-speech-service/text-to-speech-service.service';


interface ChatMessage {
  sender: 'user' | 'assistant';
  text: string;
  timestamp: string; // Add the timestamp field
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
  imports: [CommonModule, FormsModule],
  standalone: true,
  encapsulation: ViewEncapsulation.None
})
export class ChatComponent implements AfterViewChecked, OnInit {
  private baseUrl = environment.apiUrl; // Use the API URL from environment
  messages: ChatMessage[] = []; // Updated to use the ChatMessage interface
  userInput: string = '';
  isRecording: boolean = false;
  lastAssistantMessage: string = '';

  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;

  constructor(
    private http: HttpClient,
    private speechToTextService: SpeechToTextService,
    private ttsService: TextToSpeechServiceService
  ) {}

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    // Fetch previous messages from the backend
    this.http.get<{ $values: any[] }>(`${this.baseUrl}/chat/messages`)
      .pipe(catchError(error => {
        console.error('Failed to fetch messages:', error);
        return of({ $values: [] }); // Return an empty array on error
      }))
      .subscribe(response => {
        const fetchedMessages = response.$values.map(message => ({
          sender: message.sender as 'user' | 'assistant', // Explicitly cast sender type
          text: message.content,
          timestamp: message.timestamp // Include timestamp
        }));
        // Order messages by timestamp (if not already ordered by backend)
        this.messages = fetchedMessages.sort((a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());
        this.scrollToBottom();
      });
  }

  sendMessage() {
    if (!this.userInput.trim()) return;

    const userMessage: ChatMessage = { sender: 'user', text: this.userInput, timestamp: new Date().toISOString() };
    this.messages.push(userMessage);
    const userInputCopy = this.userInput; // Preserve the input for the POST request
    this.userInput = '';

    // Send the message to the backend
    this.http.post<{ reply: string }>(`${this.baseUrl}/chat/message`, { content: userInputCopy, inputMode: 'user' })
      .pipe(catchError(error => {
        console.error('Failed to send message:', error);
        return of({ reply: 'Sorry, there was an error processing your message.' }); // Handle gracefully
      }))
      .subscribe(response => {
        // Add the assistant's reply to the messages
        const assistantMessage: ChatMessage = {
          sender: 'assistant',
          text: response.reply,
          timestamp: new Date().toISOString()
        };
        this.messages.push(assistantMessage);
        this.lastAssistantMessage = response.reply;
        this.scrollToBottom();
      });
  }

  speakAnswer() {
    // Build conversation history as a list of ConversationPair
    const conversation: ConversationPair[] = [];
    for (let i = 0; i < this.messages.length; i += 2) {
      const question = this.messages[i]?.text || '';
      const answer = this.messages[i + 1]?.text || '';
      conversation.push({ Question: question, Answer: answer });
    }
    const conversationRequest = { Conversation: conversation };
    this.ttsService.speakSecurely(conversationRequest);
  }

  clearChat() {
    const confirmation = confirm('Are you sure you want to erase all chat history and start over?');
    if (confirmation) {
      // Clear the messages array
      this.messages = [];

      // Send a POST request to reset the chat on the backend
      this.http.post(`${this.baseUrl}/chat/reset`, {})
        .pipe(catchError(error => {
          console.error('Failed to reset chat:', error);
          return of(null); // Handle errors gracefully
        }))
        .subscribe(() => {
          console.log('Chat reset on the backend successfully.');
        });
    }
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.messages.push({ sender: 'user', text: `📎 Uploaded: ${file.name}`, timestamp: new Date().toISOString() });
      setTimeout(() => {
        this.messages.push({ sender: 'assistant', text: `Received file: ${file.name}`, timestamp: new Date().toISOString() });
        this.scrollToBottom();
      }, 500);
    }
  }

  toggleRecording(): void {
    if (this.isRecording) {
      this.isRecording = false;
      // No explicit stop needed, as the service stops automatically after result
    } else {
      this.isRecording = true;
      this.speechToTextService.startListening((transcript: string) => {
        this.userInput += transcript + ' ';
        this.isRecording = false;
      });
    }
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.scrollContainer.nativeElement.scrollTop =
        this.scrollContainer.nativeElement.scrollHeight;
    } catch (err) {}
  }
}
