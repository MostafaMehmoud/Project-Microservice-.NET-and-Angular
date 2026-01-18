import { Component, Input } from '@angular/core';
import { IProduct } from '../../shared/models/Product';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-product-item',
  imports: [RouterLink],
  templateUrl: './product-item.html',
  styleUrl: './product-item.scss',
})
export class ProductItem {
@Input() product? :IProduct
}
