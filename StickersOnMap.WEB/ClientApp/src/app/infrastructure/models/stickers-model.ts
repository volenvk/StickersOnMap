import {Entity} from "./entity.model";

export class Stickers implements Entity {
  constructor(
    public id: number,
    public Name: string,
    public Active: boolean,
    public createdOn: string
  ) {
  }
}
