import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './pages/layout/layout.component';
import { LoginComponent } from './pages/login/login.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '', 
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'home', 
    component: LayoutComponent, // Ruta principal con LayoutComponent
    // Puedes añadir más rutas protegidas aquí en el futuro
  },
  { path: '**', redirectTo: 'login' } // Redirige cualquier ruta desconocida a Login
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
