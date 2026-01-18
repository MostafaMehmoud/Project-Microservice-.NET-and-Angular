import { HttpInterceptorFn } from '@angular/common/http';
import { Inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const errorinterceptor: HttpInterceptorFn = (req, next) => {
  const router=Inject(Router);
  return next(req).pipe(
    // Add your error handling logic here
    catchError((error) => {
      console.error('HTTP Error:', error);
      if (error.status === 401) {
        // Handle unauthorized error
        console.warn('Unauthorized access - perhaps redirect to login?');
        router.navigateByUrl('/not-authebenticated');  
      }
      if (error.status === 403) {
        // Handle forbidden error
        console.warn('Forbidden access - perhaps redirect to forbidden page?');
        router.navigateByUrl('/forbidden');  
      }
      if (error.status === 404) {
        // Handle not found error
        console.warn('Resource not found - perhaps redirect to not found page?');
        router.navigateByUrl('/not-found');  
      }
      if (error.status === 500) {
        // Handle server error
        console.warn('Server error - perhaps redirect to server error page?');
        router.navigateByUrl('/server-error');  
      }

      return throwError(() => new Error(error));
    })
  );
};
