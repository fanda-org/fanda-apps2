import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class HiddenDataService {
  private idValue: string;

  set id(value: string) {
    this.idValue = value;
  }

  get id(): string {
    return this.idValue;
  }

  constructor() {}
}
