import { getAllMethods } from '../test-utils/mocking';

class Class {
  methodA(): void {}
  methodB(): void {}
  methodC(param: number): number {
    return param;
  }
}

describe('getAllMethods', () => {
  fit('should return all methods', () => {
    const methods = getAllMethods(Class);
    expect(methods).toEqual(['methodA', 'methodB', 'methodC']);
  });
});
