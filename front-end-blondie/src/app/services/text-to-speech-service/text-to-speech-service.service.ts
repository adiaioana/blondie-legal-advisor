import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../envinronment';

export interface Response{
  id : number;
  $values: [AudioResponse]
}
export interface AudioResponse {
  Type: string; // "question" or "answer"
  audio: string; // base64-encoded mp3
}

export interface ConversationPair {
  Question: string;
  Answer: string;
}

export interface ConversationRequest {
  Conversation: ConversationPair[];
}

@Injectable({
  providedIn: 'root'
})
export class TextToSpeechServiceService {
  private baseUrl = environment.apiUrl;
  private ttsUrl = this.baseUrl + '/tts';

  constructor(private http: HttpClient) { }

  speakSecurely(conversationRequest: ConversationRequest): void {
    this.http.post<Response>(this.ttsUrl, conversationRequest).subscribe(responses => {
      (async () => {
        for (const response of responses["$values"]) {
          if (response.audio) {
        const audio = new Audio('data:audio/mp3;base64,' + response.audio);
        const playPromise = audio.play();
        console.log('playing');
        // Wait for the audio to finish playing
        await new Promise<void>((resolve) => {
          audio.onended = () => resolve();
          // In case play() returns a promise (modern browsers)
          if (playPromise !== undefined) {
            playPromise.catch(() => resolve());
          }
        });
        // Wait an additional 1 second
        await new Promise(resolve => setTimeout(resolve, 1000));
          }
        }
      })();
    });
  }
}
