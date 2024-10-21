import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { TopbarComponent } from './parts/topbar/topbar.component';
import { SidebarComponent } from './parts/sidebar/sidebar.component';

@NgModule({
  declarations: [
    LayoutComponent  

  ],
  imports: [
    CommonModule,
    RouterModule,
    TopbarComponent,
    SidebarComponent
  ],
  exports: [LayoutComponent]
})
export class LayoutModule { }
