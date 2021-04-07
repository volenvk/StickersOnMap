import {Entity} from "./entity.model";

export class Sticker implements Entity {
  constructor(
    public id: number,
    public name: string,
    public active: boolean,
    public createDate: string
  ) {
  }
}
