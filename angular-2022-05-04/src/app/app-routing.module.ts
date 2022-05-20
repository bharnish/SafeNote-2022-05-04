import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { CodeViewComponent } from './code-view/code-view.component';
import { HomeComponent } from './home/home.component';
import { LockerViewComponent } from './locker-view/locker-view.component';
import { LockerComponent } from './locker/locker.component';
import { NoteViewComponent } from './note-view/note-view.component';
import { ShortViewComponent } from './short-view/short-view.component';

const routes: Routes = [
  {path:'', redirectTo:'home', pathMatch:'full'},
  { path:'home', component:HomeComponent},
  { path:'about', component:AboutComponent},
  { path:'code', component:CodeViewComponent},
  { path:'locker', component:LockerComponent},
  { path:'s/:key', component:ShortViewComponent},
  { path:'l/:key', component:LockerViewComponent},
  { path:':key', component:NoteViewComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
