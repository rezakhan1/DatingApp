import { TestBed } from '@angular/core/testing';

import { MemberdetailresolverResolver } from './memberdetailresolver.resolver';

describe('MemberdetailresolverResolver', () => {
  let resolver: MemberdetailresolverResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(MemberdetailresolverResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
