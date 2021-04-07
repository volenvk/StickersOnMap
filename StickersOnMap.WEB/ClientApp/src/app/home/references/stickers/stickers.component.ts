import {ChangeDetectorRef, Component, Input, OnInit} from '@angular/core';
import {Title} from '@angular/platform-browser';
import {ConfirmationService, LazyLoadEvent, MessageService, SelectItem} from 'primeng/api';
import {Router} from '@angular/router';
import {Observable, of} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import * as moment from 'moment';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Sticker} from "../../../infrastructure/models/stickers-model";
import {DatagridComponentBaseDirective} from "../../../infrastructure/datagrid-component-base.directive";
import {HittableColumnDefinition} from "../../../infrastructure/models/column-definition.model";
import {StickersService} from "../../../services/stickers.service";
import {Entity} from "../../../infrastructure/models/entity.model";
import {ServiceBase} from "../../../services/base.service";
import {RequestBuilder} from "../../../infrastructure/models/request-builder.model";


const COLUMN_SIZE_FACTOR_IN_REMS = 10;

@Component({
  selector: 'app-stickers',
  templateUrl: './stickers.component.html',
  styleUrls: ['stickers.component.scss']
})
export class StickersComponent extends DatagridComponentBaseDirective<Sticker>
  implements OnInit {

  columnDefs: { [p: string]: HittableColumnDefinition } = {
    name: {property: 'name', name: 'Наименование', visible: true},
    createDate: {property: 'createDate', name: 'Дата и время добавления', visible: true},
    active: {property: 'active', name: 'Отображать на карте', visible: true},
  };

  cols: any[];
  createdOnStartDate: Date = null;
  createdOnFilterStartDate: Date = null;
  createdOnFilterStartDateProperty = this.columnDefs.createDate.property + '_gte';
  createdOnFilterEndDate: Date = null;
  createdOnFilterEndDateProperty = this.columnDefs.createDate.property + '_lte';
  minDate = moment('1997-01-01').toDate();
  maxDate = moment().endOf('year').toDate();

  constructor(
    protected stickersService: StickersService,
    protected fb: FormBuilder,
    protected router: Router,
    protected messageService: MessageService,
    protected confirmationService: ConfirmationService,
    protected cdRef: ChangeDetectorRef,
    title: Title
  ) {
    super(stickersService, messageService, confirmationService, cdRef);
    title.setTitle('Стикеры');
  }

  _selectedColumns: any[];

  @Input() get selectedColumns(): any[] {
    return this._selectedColumns;
  }

  buildForm(): FormGroup {
    return this.fb.group({
      id: [0],
      name: [null, [Validators.required, Validators.maxLength(200)]],
      createDate: [null],
      active: [true]
    });
  }

  getFormValue(model: Sticker | null): { [p: string]: any } {
    this.createdOnStartDate = moment(model?.createDate)?.toDate() ?? null;
    return {
      id: model?.id ?? 0,
      name: model?.name ?? null,
      createDate: model?.createDate ?? null,
      active: model?.active ?? true
    };
  }

  set selectedColumns(val: any[]) {
    const visibleColumns = val.map(c => c.field);
    Object.keys(this.columnDefs)
      .forEach(key => this.columnDefs[key].visible = visibleColumns.includes(key));
    this.onColumnsCountChange(visibleColumns.length + 1);
  }

  ngOnInit() {
    super.ngOnInit();
    this.bindVisibleColumnsToColumnsSelect();
  }

  ngAfterViewInit(): void {
  }

  bindVisibleColumnsToColumnsSelect() {
    this.cols = Object.keys(this.columnDefs)
      .map(key => ({field: key, header: this.columnDefs[key].name, visible: this.columnDefs[key].visible}))
      .sort((l, r) => l.header <= r.header ? -1 : 1);
    this._selectedColumns = this.cols.filter(c => c.visible);
  }

  fetchSelectItems<T extends Entity>(
    service: ServiceBase<T>,
    func: (item: T) => SelectItem
  ): Observable<SelectItem[]> {
    const request = new RequestBuilder().orderBy('createDate').takePage(1, 10).build();
    return service.fetchFiltered(request)
      .pipe(
        map(r => r.data.map(d => func(d))),
        catchError(e => {
          this.showSingleError(e.message);
          return [];
        })
      );
  }

  restoreFilters(state: LazyLoadEvent) {
    super.restoreFilters(state);
    this.restoreCreatedOnFilter(state);
  }

  restoreCreatedOnFilter(state: LazyLoadEvent) {
    const keys = Object.keys(this.activeTableFilters);

    if (keys.includes(this.createdOnFilterStartDateProperty)) {
      const startDate = state.filters[this.createdOnFilterStartDateProperty].value;
      this.createdOnFilterStartDate = moment(startDate).toDate();
    }

    if (keys.includes(this.createdOnFilterEndDateProperty)) {
      const endDate = state.filters[this.createdOnFilterEndDateProperty].value;
      this.createdOnFilterEndDate = moment(endDate).toDate();
    }
  }

  removeFilter(name: string) {
    const property = this.columnDefs.createDate.property;

    if (name === property) {
      this.removeCreatedOnFilter();
    }

    super.removeFilter(name);
  }

  removeCreatedOnFilter() {
    delete this.table.filters[this.createdOnFilterStartDateProperty];
    delete this.table.filters[this.createdOnFilterEndDateProperty];
    this.createdOnFilterStartDate = null;
    this.createdOnFilterEndDate = null;
  }

  filterByCreatedOn(date: Date | null, matchMode: 'gte' | 'lte') {
    const property = matchMode === 'gte' ?
      this.createdOnFilterStartDateProperty : this.createdOnFilterEndDateProperty;

    let value = '';

    if (date) {
      let asMoment = moment(date);
      asMoment = matchMode === 'gte' ? asMoment.startOf('day') : asMoment.endOf('day');
      value = this.toJsonDate(asMoment.toDate());
    }

    this.table.filter(value, property, matchMode);
  }

  private toSelectItem(
      item: Sticker,
      labelSelector: (item: Sticker) => string = (i) => i?.name,
      valueSelector: (item: Sticker) => any = (i) => i?.id
  ): SelectItem {
    return {
      label: labelSelector(item),
      value: valueSelector(item),
      disabled: !item?.active
    };
  }

  private onColumnsCountChange(count: number) {
    const tableBody = document.getElementsByClassName('p-datatable')[0] as HTMLDivElement;
    const value = `${count * COLUMN_SIZE_FACTOR_IN_REMS}rem;`;
    tableBody.setAttribute('style', `min-width: ${value};`);
  }

  toJsonDate(datetime: Date | string | null): string | null {
    if (!datetime) {
      return null;
    }
    const date = moment(datetime).format('YYYY-MM-DD');
    const time = moment(datetime).format('HH:mm:ss');
    return `${date}T${time}`;
  }
}
