import { Component, Input } from '@angular/core';
import { IProduct } from '../../shared/models/Product';

@Component({
  selector: 'app-product-item',
  imports: [],
  templateUrl: './product-item.html',
  styleUrl: './product-item.scss',
})
export class ProductItem {
@Input() product? :IProduct
}
