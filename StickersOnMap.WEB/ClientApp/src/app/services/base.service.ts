import {HttpClient} from '@angular/common/http';
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";
import {map} from 'rxjs/operators';
import {PagedList} from "../infrastructure/models/paged-list.model";
import {Entity} from "../infrastructure/models/entity.model";
import {TableFilter} from "../infrastructure/models/table-filter.model";


export abstract class ServiceBase<T extends Entity> {
  protected constructor(protected httpClient: HttpClient, private url: string) {
  }

  fetch<TModel extends Entity = T>(): Observable<TModel> {
    return this.httpClient.get<TModel>(`${this.url}/`);
  }

  fetchById<TModel extends Entity = T>(id: number): Observable<TModel> {
    return this.httpClient.get<TModel>(`${this.url}/${id}`);
  }

  fetchAll<TModel extends Entity = T>(): Observable<TModel[]> {
    return this.httpClient.get<TModel[]>(`${this.url}/`);
  }

  fetchFiltered<TModel extends Entity = T>(state: TableFilter): Observable<PagedList<TModel>> {
    if (environment.useFakeApi) {
      return this. httpClient.get<TModel[]>(this.url).pipe(map(data => {
        const list: PagedList<TModel> = {
          data,
          totalCount: data.length
        };
        return list;
      }));
    }

    return this.httpClient.post<PagedList<TModel>>(this.url, state);
  }

  create<TModel extends Entity = T>(item: TModel): Observable<number> {
    return this.httpClient.post<number>(`${this.url}/create`, item);
  }

  update<TModel extends Entity = T>(item: TModel): Observable<number> {
    return this.httpClient.put<number>(`${this.url}/${item.id}`, item);
  }

  delete(id: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.url}/${id}`);
  }
}
