import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageComponent } from './manage/manage.component';

const routes: Routes = [
  { path: 'advisors', component: ManageComponent },
  { path: '', redirectTo: 'advisors', pathMatch: 'full' },
  { path: '**', redirectTo: 'advisors', pathMatch: 'full' }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
