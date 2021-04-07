import {ConfirmationService, LazyLoadEvent, MessageService} from 'primeng/api';
import {FormGroup} from '@angular/forms';
import {AfterViewInit, ChangeDetectorRef, Directive, OnInit, ViewChild} from '@angular/core';
import {Table} from 'primeng/table';
import {PrimeToApiAdapter} from './prime-to-api.adapter';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
import {ServiceBase} from "../services/base.service";
import {ComponentBase} from "./component-base";
import {ColumnDefinition} from "./models/column-definition.model";
import {ColumnFilter} from "./models/column-filter.model";
import { yesNoAllSelectItems } from './constants';
import {Entity} from "./models/entity.model";

@Directive()
export abstract class DatagridComponentBaseDirective<T extends Entity> extends ComponentBase
  implements OnInit, AfterViewInit {

  items: T[] = [];
  totalItemsCount = 0;

  yesNoAllSelectItems = yesNoAllSelectItems;

  displayDeleteDialog = false;

  form: FormGroup;

  abstract columnDefs: { [key: string]: ColumnDefinition };

  get columnsCount(): number {
    if (!this.table) {
      return 0;
    }

    const table = this.table.tableViewChild.nativeElement as HTMLTableElement;
    const headerRow = table.tHead.firstChild as HTMLTableRowElement;
    return headerRow.children.length;
  }

  allTableFilters: ColumnFilter[];

  restored = false;
  activeTableFilters: { [key: string]: string } = {};

  @ViewChild('dt') table: Table;

  protected constructor(
    protected fetchService: ServiceBase<T>,
    protected messageService: MessageService,
    protected confirmationService: ConfirmationService,
    protected cdRef: ChangeDetectorRef) {
    super(messageService);
  }

  ngOnInit() {
    this.form = this.buildForm();
    this.setAllTableFilters();
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

  buildForm(): FormGroup {
    return new FormGroup({});
  }

  getFormValue(model: T | null): { [p: string]: any } {
    return {};
  }

  setAllTableFilters() {
    this.allTableFilters = Object.keys(this.columnDefs).map(key => {
      return {...this.columnDefs[key], active: false};
    });
  }

  loadData(event: LazyLoadEvent) {
    if (!this.restored) {
      this.restored = true;
      this.restoreFilters(event);
    }

    const clrRequest = new PrimeToApiAdapter(event);
    this.loading = true;

    this.checkActiveFilters(event);

    if (!environment.production) {
      console.log(JSON.stringify(clrRequest));
    }

    this.fetchService.fetchFiltered(clrRequest).subscribe(
        (response) => {
          this.loading = false;
          this.items = response.data;
          this.totalItemsCount = response.totalCount;
        }, (ex) => this.setErrorState(ex.error.message)
    );
  }

  checkActiveFilters(event: LazyLoadEvent) {
    const keys = Object.keys(event.filters)
        .map(key => this.removeFilterPropertyModifiers(key));
    for (const filter of this.allTableFilters) {
      const idx = keys.indexOf(filter.property);
      filter.active = idx >= 0;
    }
  }

  removeFilterPropertyModifiers(key: string) {
    return key
        .replace('_gte', '')
        .replace('_lte', '')
        .replace('_gt', '')
        .replace('_lt', '');
  }

  restoreFilters(state: LazyLoadEvent) {
    this.activeTableFilters = {};
    if (!state?.filters) {
      return;
    }

    Object.keys(state.filters).forEach(key => {
          this.activeTableFilters[key] = state.filters[key].value;
        }
    );
  }

  onSubmit() {
    this.readonly = false;
    this.toggleDialog();
    const model = this.buildSubmitModel();
    this.getUpdateObservableFor(model).subscribe(
        id => {
          this.showSuccessToast('Операция выполнена успешно');
          this.refreshTable();
        },
        (ex) => this.showSingleError(ex.error.message));
  }

  buildSubmitModel(): T {
    return {...this.form.value};
  }

  onCancel() {
    this.readonly = false;
    this.toggleDialog();
  }

  onCreate() {
    this.readonly = false;
    this.form.markAsUntouched();
    this.form.markAsPristine();
    const value = this.getFormValue(null);
    this.form.setValue(value);
    this.toggleDialog();
  }

  onEdit(id: number) {
    this.readonly = false;
    const item = this.items.find(u => u.id === id);
    const value = this.getFormValue(item);
    this.form.setValue(value);
    this.toggleDialog();
  }

  onDelete(id: number) {
    this.confirmationService.confirm({
      message: 'Вы уверены, что хотите удалить запись?',
      accept: () => this.deleteItem(id),
      acceptLabel: 'Да',
      acceptButtonStyleClass: 'p-button-text p-button-danger',
      rejectLabel: 'Нет',
      rejectButtonStyleClass: 'p-button-text',
      defaultFocus: 'reject'
    });
  }

  deleteItem(id: number) {
    this.fetchService.delete(id).subscribe(
        () => {
          this.showSuccessToast('Операция выполнена успешно');
          this.refreshTable();
        },
        (ex) => this.showSingleError(ex.error.message)
    );
  }

  toggleDialog() {
    this.displayDeleteDialog = !this.displayDeleteDialog;
  }

  getUpdateObservableFor(model: T): Observable<number> {
    return this.fetchService.update(model);
  }

  removeFilter(name: string) {
    delete this.table.filters[name];
    this.restoreFilters(this.table.createLazyLoadMetadata());
    this.refreshTable();
    this.table.saveState();
  }

  refreshTable() {
    const state = this.table.createLazyLoadMetadata();
    this.loadData(state);
  }
}
