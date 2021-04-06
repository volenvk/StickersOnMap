import {ColumnDefinition} from './column-definition.model';

export interface ColumnFilter extends ColumnDefinition {
  active: boolean;
}
