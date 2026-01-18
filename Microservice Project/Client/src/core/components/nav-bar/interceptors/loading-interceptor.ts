import { HttpInterceptorFn } from '@angular/common/http';
import { Laoding } from '../../../services/laoding';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService=inject(Laoding);
  loadingService.loading();
  return next(req).pipe(
    delay(1000),
    finalize(() => {
      loadingService.idle();
    })
  );
};
