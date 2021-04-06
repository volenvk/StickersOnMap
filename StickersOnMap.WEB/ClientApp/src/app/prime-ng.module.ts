import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ButtonModule} from 'primeng/button';
import {MessagesModule} from 'primeng/messages';
import {ToastModule} from 'primeng/toast';
import {CalendarModule} from 'primeng/calendar';
import {TableModule} from "primeng/table";
import {MultiSelectModule} from "primeng/multiselect";
import {InputTextareaModule} from "primeng/inputtextarea";
import {InputMaskModule} from "primeng/inputmask";
import {OverlayPanelModule} from "primeng/overlaypanel";
import {AutoCompleteModule} from "primeng/autocomplete";
import {ProgressBarModule} from "primeng/progressbar";
import {RadioButtonModule} from "primeng/radiobutton";
import {TabViewModule} from "primeng/tabview";
import {InputNumberModule} from "primeng/inputnumber";
import {MessageModule} from "primeng/message";
import {ConfirmDialogModule} from "primeng/confirmdialog";
import {CheckboxModule} from "primeng/checkbox";
import {DialogModule} from "primeng/dialog";
import {ToolbarModule} from "primeng/toolbar";
import {DropdownModule} from "primeng/dropdown";
import {InputTextModule} from "primeng/inputtext";
import {FormsModule} from "@angular/forms";
import {MenuModule} from "primeng/menu";

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TableModule,
    InputTextModule,
    DropdownModule,
    ButtonModule,
    ToolbarModule,
    DialogModule,
    CheckboxModule,
    ConfirmDialogModule,
    MenuModule,
    MessagesModule,
    MessageModule,
    ToastModule,
    InputNumberModule,
    InputMaskModule,
    MultiSelectModule,
    CalendarModule,
    TabViewModule,
    RadioButtonModule,
    ProgressBarModule,
    AutoCompleteModule,
    InputTextareaModule,
    OverlayPanelModule,
    FormsModule
  ],
  exports: [
    TableModule,
    InputTextModule,
    DropdownModule,
    ButtonModule,
    ToolbarModule,
    DialogModule,
    CheckboxModule,
    ConfirmDialogModule,
    MenuModule,
    MessagesModule,
    MessageModule,
    ToastModule,
    InputNumberModule,
    InputMaskModule,
    MultiSelectModule,
    CalendarModule,
    TabViewModule,
    RadioButtonModule,
    ProgressBarModule,
    AutoCompleteModule,
    InputTextareaModule,
    OverlayPanelModule
  ]
})
export class PrimeNgModule {
}
