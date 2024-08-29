import { Provider } from '@angular/core';

export function getAllMethods(type$: any): string[] {
  return Object.getOwnPropertyNames(type$.prototype)
    .filter((item) => item !== 'constructor')
    .filter((item) => {
      return typeof type$.prototype[item] === 'function';
    });
}

export function createProvider(type: any): Provider {
  return {
    provide: type,
    useValue: jasmine.createSpyObj(getAllMethods(type)),
  };
}
