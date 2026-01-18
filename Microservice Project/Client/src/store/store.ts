import { ChangeDetectorRef, Component, ElementRef, NgModule, OnInit, viewChild } from "@angular/core";
import { IBrands, IProduct, ITypes } from "../shared/models/Product";
import { StoreService } from "./service/store.service";
import { SpecParam } from "../shared/models/StoreParam";
import { ProductItem } from "./product-item/product-item";
import { CommonModule } from "@angular/common";


import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-store',
  imports: [ProductItem,CommonModule,FormsModule ],
  templateUrl: './store.html',
  styleUrl: './store.scss',
})
export class Store implements OnInit {
sortOptions = [ 
  {name:'Alphabetical', value:'name'},
  {name:'Price: Low to High', value:'priceAsc'},
  {name:'Price: High to Low', value:'priceDesc'},
];
itemPerPageOptions=[{
  name:'3 / page', value:3
},{
  name:'6 / page', value:6
},{
  name:'9 / page', value:9
}]
  products: IProduct[] = [];
  brands: IBrands[] = [];
  types: ITypes[] = [];
 searchTerm = viewChild<ElementRef<HTMLInputElement>>('search');
  specPrams = new SpecParam();
 totalCount = 9;
pageSize = 3;
pageIndex = 1;

onPageChanged(page: number) {
  this.pageIndex = page;
  this.specPrams.pageIndex = this.pageIndex;
  this.specPrams.pageSize = this.pageSize;
  this.getAllProducts();
}

get totalPages() {
  return Math.ceil(this.totalCount / this.pageSize);
}

  constructor(private storeService: StoreService,
    private cdr: ChangeDetectorRef
  ) {}
  getAllProducts() {
    this.storeService.getAllProducts(this.specPrams).subscribe({
      next: (res) => {
        this.products = res.data;
        this.totalCount = res.count;
        this.specPrams.pageIndex = this.pageIndex;
        this.specPrams.pageSize = this.pageSize;
        this.cdr.detectChanges(); 
        console.log(this.products);
             },
      error: (error) => {
        console.log(error);
      },
    });
  }

  ngOnInit(): void {
    console.log("Store Component Initialized");
    this.specPrams.pageSize = this.pageSize;
    this.specPrams.pageIndex = this.pageIndex;

    this.getAllProducts();
    this.getAllBrands();
    this.getAllTypes();
    console.log("Store Component Initialized End"); 
  }
  getAllBrands() {
    this.storeService.getAllBrands().subscribe(res => {
      this.brands =[ {id:"", name:"All"},...res];
      this.cdr.detectChanges(); 

    });
  }

  getAllTypes() {
    this.storeService.getAllTypes().subscribe(res => {
      this.types = [ {id:"", name:"All"},...res];;
      this.cdr.detectChanges(); 
    });
  }

  onBrandSelected(brandId: string) {
    this.specPrams.brandId = brandId;
    this.getAllProducts();
  }

  onTypeSelected(typeId: string) {
    this.specPrams.typeId = typeId;
    this.getAllProducts();
  }
  onSortSelected(sort:any){
    this.specPrams.sort=sort.value;
    this.getAllProducts();
  }
  onSearch(){
    this.specPrams.search=this.searchTerm()?.nativeElement.value;
this.pageIndex=1;
this.specPrams.pageIndex=this.pageIndex;
    this.getAllProducts();
  }
   onResetFilters(){
   
    this.specPrams.brandId="";
    this.specPrams.typeId="";
    this.specPrams.search="";

    this.getAllProducts();
  }
 onItemsPerPageSelected(itemsPerPage:any

 ){
  this.pageSize=itemsPerPage.value;
  this.specPrams.pageSize=this.pageSize;
  this.pageIndex=1;
  this.specPrams.pageIndex=this.pageIndex;
  this.getAllProducts();
 }
}
