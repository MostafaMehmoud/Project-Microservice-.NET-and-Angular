import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { errorinterceptor } from '../core/components/nav-bar/interceptors/errorinterceptor-interceptor';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { loadingInterceptor } from '../core/components/nav-bar/interceptors/loading-interceptor';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
 providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
importProvidersFrom(CarouselModule),provideAnimations(),
    // مهم جدًا لتفعيل HttpClient + Interceptors
     provideHttpClient(
      withInterceptors([errorinterceptor,loadingInterceptor])
    )
  ]
};
