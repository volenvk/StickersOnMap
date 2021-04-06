export interface ColumnDefinition {
  name: string;
  property: string;
}

export interface HittableColumnDefinition extends ColumnDefinition {
  visible: boolean;
}
