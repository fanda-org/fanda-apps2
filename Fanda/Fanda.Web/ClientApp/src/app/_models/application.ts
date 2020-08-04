import { AppResource } from './app-resource';

export class Application {
  id: string;
  code: string;
  name: string;
  description: string;
  edition: string;
  version: string;
  active: boolean;

  appResources: AppResource[];
}
