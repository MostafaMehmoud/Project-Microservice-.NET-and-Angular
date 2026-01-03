import { Component, signal } from '@angular/core';
import { NavBar } from "../core/components/nav-bar/nav-bar";
import { Store } from "../store/store";


@Component({
  selector: 'app-root',
  imports: [NavBar, Store],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('Client');
}
