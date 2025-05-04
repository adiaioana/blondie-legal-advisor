import { Component, ViewEncapsulation  } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MenuComponent} from './general-page-components/menu/menu.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MenuComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  standalone: true,
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  title = 'front-end-blondie';
}
