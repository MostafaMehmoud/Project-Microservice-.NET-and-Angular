import { Component } from '@angular/core';
import { BreadcrumbComponent,

  BreadcrumbItemDirective,
  BreadcrumbService
 } from 'xng-breadcrumb';
 import { AsyncPipe, CommonModule, NgIf, TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [BreadcrumbComponent, NgIf,AsyncPipe,TitleCasePipe,CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
constructor(public bcService:BreadcrumbService){

}
}