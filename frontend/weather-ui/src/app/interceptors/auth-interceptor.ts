import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const token = localStorage.getItem('token');
  console.log('[Interceptor] Token from storage:', token ? token.substring(0, 20) + '...' : 'NO TOKEN');
  console.log('[Interceptor] Request URL:', req.url);

  if (token) {
    console.log('[Interceptor] Attaching Authorization header');
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  console.log('[Interceptor] No token, request sent without Authorization');
  return next(req);
};
