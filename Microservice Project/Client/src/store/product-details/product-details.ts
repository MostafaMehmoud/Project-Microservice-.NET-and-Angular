import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IProduct } from '../../shared/models/Product';
import { StoreService } from '../service/store.service';
import { ActivatedRoute } from '@angular/router';
import { pipe } from 'rxjs';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  imports: [CurrencyPipe,CommonModule,FormsModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.scss',
})
export class ProductDetails implements OnInit {
  product?:IProduct;

  constructor(private StoreService:StoreService,
    private route:ActivatedRoute,
private cd:ChangeDetectorRef,
private bcService:BreadcrumbService 
  ) {}
  ngOnInit(): void {
    this.loadProduct();
  }
  loadProduct(){
    const id = this.route.snapshot.paramMap.get('id');
    if(id){
      this.StoreService.getProductById(id).subscribe({
        next:(product)=>{
          this.bcService.set('@ProductDetails',product.name);
          this.product=product;
          console.log(this.product);

          this.cd.detectChanges();
        },
        error:(error)=>{
          console.log(error);
        }

      })
    }
}quantity=1;


  dec() { if (this.quantity > 1) this.quantity--; }
  inc() { this.quantity++; }

  addToCart() {
    // purely demo; no API
    alert(`Added to cart:\n${this.product?.name}\nQty: ${this.quantity}\nTotal: EGP ${this.product!.price * this.quantity}`);
  }

}