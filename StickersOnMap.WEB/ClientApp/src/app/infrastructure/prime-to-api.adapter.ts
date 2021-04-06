import {LazyLoadEvent} from 'primeng/api';
import {TableFilter} from "./models/table-filter.model";
import {Filter} from "./models/filter.model";

export class PrimeToApiAdapter implements TableFilter {
  page?: {
    from?: number;
    to?: number;
    size?: number;
    current?: number;
  };
  sort?: {
    by: string;
    reverse: boolean;
  };
  filters?: Filter[];

  constructor(private event: LazyLoadEvent) {
    this.adapt();
  }

  private adapt() {
    this.setPage();
    this.setSort();
    this.setFilters();
  }

  private setPage() {
    const size = this.event.rows ?? 10;
    const current = Math.trunc((this.event.first ?? 0) / size) + 1;

    this.page = {size, current};
  }

  private setSort() {
    if (this.event.sortField && this.event.sortOrder) {
      this.sort = {
        by: this.event.sortField,
        reverse: this.event.sortOrder === -1
      };
    }
  }

  private setFilters() {
    if (this.event.filters) {
      this.filters = Object.keys(this.event.filters).map(key => {
        const filter = this.event.filters[key];

        if (filter.matchMode !== 'range') {
          return this.createValueFilter(key, filter.value);
        }

        const values = filter.value.split(';');

        if (values.length !== 2) {
          throw new Error('not enough parameters for filter in range mode');
        }

        return this.createMinMaxFilter(key, values[0], values[1]);
      });
    }
  }

  createValueFilter(property: string, value: string): Filter {
    return {property, value};
  }

  createMinMaxFilter(property: string, min: string, max: string): Filter {
    return {property, min, max};
  }
}
