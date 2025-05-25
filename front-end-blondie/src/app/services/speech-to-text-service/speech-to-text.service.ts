import { Injectable, NgZone } from '@angular/core';

interface ISpeechRecognition extends EventTarget {
  lang: string;
  interimResults: boolean;
  maxAlternatives: number;
  start(): void;
  stop(): void;
  onresult: ((event: any) => void) | null;
  onerror: ((event: any) => void) | null;
}

@Injectable({
  providedIn: 'root'
})
export class SpeechToTextService {
  private recognition: ISpeechRecognition;

  constructor(private zone: NgZone) {
    const SpeechRecognition = (window as any).SpeechRecognition || (window as any).webkitSpeechRecognition;
    this.recognition = new SpeechRecognition();
    this.recognition.lang = 'en-US';
    this.recognition.interimResults = false;
    this.recognition.maxAlternatives = 1;
  }

  startListening(callback: (text: string) => void): void {
    this.recognition.start();
    this.recognition.onresult = (event: any) => {
      const transcript = event.results[0][0].transcript;
      this.zone.run(() => callback(transcript));
    };
    this.recognition.onerror = (event: any) => {
      console.error('Speech recognition error:', event.error);
    };
  }
}
