import { Component, signal } from '@angular/core';
import { NavBar } from "../core/components/nav-bar/nav-bar";
import { Store } from "../store/store";
import { RouterOutlet } from "@angular/router";
import { Header } from "../core/components/header/header";
import {  NgxSpinnerModule } from 'ngx-spinner';


@Component({
  selector: 'app-root',
  imports: [NavBar, Store, RouterOutlet, Header,NgxSpinnerModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('Client');
}
