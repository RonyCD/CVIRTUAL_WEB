import { Component, ElementRef } from '@angular/core';
import { MenuComponent } from './menu.component';
import { LayoutService } from '../../../../core/service/layout.service';

@Component({
    selector: 'app-sidebar',
    standalone: true,
    imports: [
        MenuComponent
    ],
    template: `
        <div class="layout-menu"> 
            <app-menu></app-menu>
        </div>
    `
})
export class SidebarComponent {
    constructor(
        public layoutService: LayoutService,
        public el: ElementRef
    ) { }
}
