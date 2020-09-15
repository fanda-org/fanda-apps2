import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData, CommonModule } from '@angular/common';
import en from '@angular/common/locales/en';

// import { NzLayoutModule } from 'ng-zorro-antd/layout';
// import { NzMenuModule } from 'ng-zorro-antd/menu';
// import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
// import { NzGridModule } from 'ng-zorro-antd/grid';
// import { NzFormModule } from 'ng-zorro-antd/form';
// import { NzInputModule } from 'ng-zorro-antd/input';
import { IconsProviderModule } from './icons-provider.module';
import { NgZorroAntdModule } from './ng-zorro-antd.module';

import { NZ_I18N, en_US } from 'ng-zorro-antd/i18n';
import { NzMessageService } from 'ng-zorro-antd/message';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DefaultLayoutComponent } from './layouts/default-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout.component';

registerLocaleData(en);

@NgModule({
  declarations: [AppComponent, DefaultLayoutComponent, AuthLayoutComponent],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    IconsProviderModule,
    // NzLayoutModule,
    // NzMenuModule,
    // NzBreadCrumbModule,
    // NzGridModule,
    // NzFormModule,
    // NzInputModule,
    NgZorroAntdModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
  ],
  exports: [NgZorroAntdModule],
  providers: [{ provide: NZ_I18N, useValue: en_US }, NzMessageService],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor() {
    console.log('appModule:constructor');
  }
}
