import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { RippleModule } from 'primeng/ripple';
import { BadgeModule } from 'primeng/badge';
import { MENU } from '../../menu-items';

@Component({
    selector: 'app-menu',
    standalone: true,
    imports: [
        MenuModule,
        RippleModule,
        BadgeModule
    ],
    template: `
        <p-menu [model]="menu" styleClass="w-full">
            <ng-template pTemplate="start">
                <div class="text-center py-2">
                    <span class="text-3xl font-bold">Menu</span>
                </div>
            </ng-template>
            <ng-template pTemplate="submenuheader" let-item>
                <span class="text-primary font-bold">{{ item.label }}</span>
            </ng-template>
            <ng-template pTemplate="item" let-item>
                <a pRipple class="text-color flex align-items-center p-menuitem-link">
                    <span [class]="item.icon"></span>
                    <span class="ml-2">{{ item.label }} item</span>
                    @if(item.badge){<p-badge class="ml-auto" [value]="item.badge" />}
                    @if(item.shortcut){<span class="ml-auto border-1 surface-border border-round surface-100 text-xs p-1">{{ item.shortcut }}</span>}
                </a>
            </ng-template>
        </p-menu>
    `
})

export class MenuComponent implements OnInit {
    
    menu: MenuItem[] = [];

    constructor() { }

    ngOnInit(): void {
        this.menu = MENU;
    }
}
