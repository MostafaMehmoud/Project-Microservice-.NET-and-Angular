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
  getAllProducts(SpecParam:SpecParam){
    let params=new HttpParams()
    if(SpecParam.brandId){
      params=params.set("BrandId",SpecParam.brandId)

    }
    if(SpecParam.typeId){
      params=params.set("TypeId",SpecParam.typeId)
    }// إضافة Pagination
  params = params
    .set('pageIndex', SpecParam.pageIndex?.toString() || '1')
    .set('pageSize', SpecParam.pageSize?.toString() || '10');

    console.log(params)
    return this.http.get<IResponseDto<IProduct[]>>(`${this.baseUrl}Catalog/GetAllProducts`,{params})
  }
  getAllTypes(){
    return this.http.get<ITypes[]>(`${this.baseUrl}Catalog/GetAllTypes`)
  }getAllBrands(){
    return this.http.get<IBrands[]>(`${this.baseUrl}Catalog/GetAllBrands`)
  }
}
