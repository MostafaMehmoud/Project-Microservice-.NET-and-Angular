import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IResponseDto } from '../../shared/models/ResponseDto';
import { IBrands, IProduct, ITypes } from '../../shared/models/Product';
import { SpecParam } from '../../shared/models/StoreParam';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  baseUrl:string="http://localhost:8010/"
  constructor(private http:HttpClient){

  }
  getAllProducts(specParam: SpecParam) {
    let params = new HttpParams();
    params = params.append('pageIndex', specParam.pageIndex.toString());
    params = params.append('pageSize', specParam.pageSize.toString());

    if (specParam.brandId) {
      params = params.append('brandId', specParam.brandId);
    } 
    if (specParam.typeId) {
      params = params.append('typeId', specParam.typeId);
    }
    if (specParam.search) {
      params = params.append('search', specParam.search);
    }
    params = params.append('sort', specParam.sort);
    
  return this.http.get<IResponseDto<IProduct[]>>(this.baseUrl + 'Catalog/GetAllProducts', { params });
}

  getAllTypes(){
    return this.http.get<ITypes[]>(`${this.baseUrl}Catalog/GetAllTypes`)
  }getAllBrands(){
    return this.http.get<IBrands[]>(`${this.baseUrl}Catalog/GetAllBrands`)
  }
  getProductById(id:string){
    return this.http.get<IProduct>(`${this.baseUrl}Catalog/GetProductById/${id}`)
  }
}
