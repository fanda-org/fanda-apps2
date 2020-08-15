import { ResourceType } from '../_enums';

export interface AppResource {
  id: string;
  code: string;
  name: string;
  description: string;
  resourceType: ResourceType;

  //#region Action fields
  creatable: boolean;
  updatable: boolean;
  deletable: boolean;
  readable: boolean;
  printable: boolean;
  importable: boolean;
  exportable: boolean;
  //#endregion

  active: boolean;
}
