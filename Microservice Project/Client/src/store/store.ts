import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IBrands, IProduct, ITypes } from '../shared/models/Product';
import { StoreService } from './service/store.service';
import { ProductItem } from "./product-item/product-item";
import { SpecParam } from '../shared/models/StoreParam';


@Component({
  selector: 'app-store',
  imports: [ProductItem],
  templateUrl: './store.html',
  styleUrl: './store.scss',
})
export class Store implements OnInit{
products:IProduct[]=[]
brands:IBrands[]=[]
types:ITypes[]=[]
specPrams=new SpecParam();
constructor(private storeService:StoreService,private cd:ChangeDetectorRef){}
  ngOnInit(): void {
   this.loadProducts()
   this.loadTypes()
   this.loadBrands()
  }

  loadProducts(){
 this.storeService.getAllProducts(this.specPrams).subscribe({
      next:res=>{
        console.log(res.data)
        this.products=res.data
        this.cd.detectChanges()
      },
      error:err=>{
        console.log(err)
      }
    })
  }
   loadBrands(){
 this.storeService.getAllBrands().subscribe({
      next:res=>{
        this.brands=res
        this.cd.detectChanges()
      },
      error:err=>{
        console.log(err)
      }
    })
  }
   loadTypes(){
 this.storeService.getAllTypes().subscribe({
      next:res=>{
        this.types=res
        this.cd.detectChanges()
      },
      error:err=>{
        console.log(err)
      }
    })
  }
  onBrandSelected(brandId:string){
    this.specPrams.brandId=brandId;
    console.log(brandId);
    this.loadProducts();
  }
   onTypeSelected(typeId:string){
    this.specPrams.typeId=typeId;
    this.loadProducts();
  }
}
