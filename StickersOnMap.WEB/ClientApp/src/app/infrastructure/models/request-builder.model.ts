import {TableFilter} from './table-filter.model';

export class RequestBuilder {
  private _filters: any[] = [];
  private _page: any = null;
  private _sort: any = null;

  addFilter(property: string, value: string): RequestBuilder {
    const filter = {property, value};
    this._filters.push(filter);
    return this;
  }

  orderBy(by: string, reverse: boolean = false): RequestBuilder {
    this._sort = {by, reverse};
    return this;
  }

  takePage(current: number, size: number): RequestBuilder {
    this._page = {current, size};
    return this;
  }

  onlyActive(): RequestBuilder {
    return this.addFilter('Active', 'true');
  }

  build(): TableFilter {
    return {
      filters: this._filters,
      page: this._page,
      sort: this._sort
    };
  }
}
