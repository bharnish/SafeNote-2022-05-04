import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { NoteViewComponent } from './note-view/note-view.component';
import { ShortViewComponent } from './short-view/short-view.component';

const routes: Routes = [
  {path:'', redirectTo:'home', pathMatch:'full'},
  { path:'home', component:HomeComponent},
  { path:'about', component:AboutComponent},
  { path:':key', component:NoteViewComponent},
  { path:'s/:key', component:ShortViewComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
