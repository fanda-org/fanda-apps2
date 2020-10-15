import { AppResource } from './app-resource';

export interface Application {
  id: string;
  code: string;
  name: string;
  description: string;
  edition: string;
  version: string;
  active: boolean;

  appResources: AppResource[];
}
