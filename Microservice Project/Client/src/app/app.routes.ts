import { Routes } from '@angular/router';
import { Home } from '../home/home/home';
import { Store } from '../store/store';
import { ProductDetails } from '../store/product-details/product-details';

export const routes: Routes = [
    {path:'',component:Home,data:{breadcrumb:'Home'}},
    {path:'store',loadComponent() {
        return import('../store/store').then(m => m.Store);
    },data:{breadcrumb:'Store'}},{
        path:'store/:id',loadComponent() {
            return import('../store/product-details/product-details').then(m => m.ProductDetails);
    },data:{breadcrumb:{alias:'ProductDetails'}}
}, {path:'not-found',loadComponent() {
        return import('../core/pages/not-found/not-found').then(m => m.NotFound);
    }},
    {path:'server-error',loadComponent() {
        return import('../core/pages/server-error/server-error').then(m => m.ServerError);
    }},

    {path:'forbidden',loadComponent() {
        return import('../core/pages/forbidden/forbidden').then(m => m.Forbidden);
        }},
    {path:'not-authebenticated',loadComponent() {
        return import('../core/pages/not-authebenticated/not-authebenticated').then(m => m.NotAuthebenticated);
    }},
    // {path:'store/:id',component:ProductDetails},
    {path:'**',redirectTo:'',pathMatch:'full'},
   
];
