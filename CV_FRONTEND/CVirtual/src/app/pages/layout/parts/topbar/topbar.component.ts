import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutService } from '../../../../core/service/layout.service';

@Component({
    selector: 'app-topbar',
    standalone: true,
    templateUrl: './topbar.component.html',
    styleUrl: './topbar.component.css',
    imports: [
        RouterModule
    ]
})
export class TopbarComponent implements OnInit {
    
    @ViewChild('menuButton') menuButton!: ElementRef;
    @ViewChild('topBarMenuButton') topBarMenuButton!: ElementRef;
    @ViewChild('topBarMenu') topBarMenu!: ElementRef;


    constructor(
        public layoutService: LayoutService
    ) { }

    ngOnInit(): void { }
}
