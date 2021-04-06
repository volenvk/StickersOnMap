import {Message, MessageService} from 'primeng/api';

export abstract class ComponentBase {
  loading = false;
  readonly = false;
  messages: Message[] = [];

  protected constructor(protected messageService: MessageService) {
  }

  clearMessages() {
    this.messages = [];
  }

  showToast(severity: string, detail: string) {
    this.messageService.add({severity, detail});
  }

  showSuccessToast(detail: string) {
    this.showToast('success', detail);
  }

  showSingleError(detail: string) {
    this.showSingleMessage(detail, 'error');
  }

  showSingleWarning(detail: string) {
    this.showSingleMessage(detail, 'warn');
  }

  showSingleMessage(detail: string, severity: string) {
    this.messages = [{severity, detail}];
  }

  setLoadingState() {
    this.loading = true;
  }

  setDefaultState() {
    this.loading = this.readonly = false;
  }

  setErrorState(error: string) {
    this.loading = false;
    this.showSingleError(error);
  }

  setSuccessState(message: string) {
    this.loading = false;
    this.showSuccessToast(message);
  }
}
