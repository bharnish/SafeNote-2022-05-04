import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { ApiModule } from './api/api.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NoteViewComponent } from './note-view/note-view.component';
import { AboutComponent } from './about/about.component';
import { ShortViewComponent } from './short-view/short-view.component';
import { CodeViewComponent } from './code-view/code-view.component';
import { LockerComponent } from './locker/locker.component';
import { LockerViewComponent } from './locker-view/locker-view.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NoteViewComponent,
    AboutComponent,
    ShortViewComponent,
    CodeViewComponent,
    LockerComponent,
    LockerViewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ApiModule.forRoot({rootUrl: environment.rootUrl}),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
