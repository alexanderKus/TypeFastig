import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { Top100Component } from './features/top100/top100.component';
import { ProfileComponent } from './features/profile/profile.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'top100', component: Top100Component },
  { path: 'profile', component: ProfileComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
