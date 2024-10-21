import { Component, OnDestroy, Renderer2, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TopbarComponent } from './parts/topbar/topbar.component';
import { SidebarComponent } from './parts/sidebar/sidebar.component';
import { Subscription } from 'rxjs';
import { LayoutService } from '../../core/service/layout.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnDestroy{
    overlayMenuOpenSubscription!: Subscription;
    menuOutsideClickListener: any;
    profileMenuOutsideClickListener: any;

    @ViewChild(TopbarComponent) appTopBar!: TopbarComponent;
    @ViewChild(SidebarComponent) appSideBar!: SidebarComponent;

    constructor(
        private layoutService: LayoutService,
        private renderer: Renderer2
    ) {
        //console.log("By ZKOF");
        this.overlayMenuOpenSubscription = this.layoutService.overlayOpen$.subscribe({
            next: () => {
                if(!this.menuOutsideClickListener){
                    //Si il n'y a pas encore d'ecouteur on en crée
                    this.menuOutsideClickListener = this.renderer.listen('document', 'click', event => {
                        const isOutsideClicked = !(
                            this.appSideBar.el.nativeElement.isSameNode(event.target) //si on a cliquer sur la bar latérale
                            || this.appSideBar.el.nativeElement.contains(event.target) //si on cliquer sur le contenu de la bar latérale
                            || this.appTopBar.menuButton.nativeElement.isSameNode(event.target) //si on a cliquer sur le bouton du menu
                            || this.appTopBar.menuButton.nativeElement.contains(event.target) //si on cliquer sur le contenu du bouton du menu
                        );

                        if(isOutsideClicked){
                            this.hideMenu();
                        }
                    });
                }

                if(!this.profileMenuOutsideClickListener){
                    //Si il n'y a pas encore d'ecouteur on en crée
                    this.profileMenuOutsideClickListener = this.renderer.listen('document', 'click', event => {
                        const isOutsideClicked = !(
                            this.appSideBar.el.nativeElement.isSameNode(event.target) //si on a cliquer sur la bar latérale
                            || this.appSideBar.el.nativeElement.contains(event.target) //si on cliquer sur le contenu de la bar latérale
                            || this.appTopBar.topBarMenuButton.nativeElement.isSameNode(event.target) //si on a cliquer sur le bouton du menu
                            || this.appTopBar.topBarMenuButton.nativeElement.contains(event.target) //si on cliquer sur le contenu du bouton du menu
                        );

                        if(isOutsideClicked){
                            this.hideProfileMenu();
                        }
                    });
                }

                if(this.layoutService.state.staticMenuMobileActive){
                    //Si le menu s'affiche en mode superposition (cas des petit ecrans)
                    this.blockBodyScroll();
                }
            }
        });
    }

    /**
     * Permet de cacher les menus
     */
    hideMenu(): void  {
        this.layoutService.state.overlayMenuActive = false;
        this.layoutService.state.staticMenuMobileActive = false;
        this.layoutService.state.menuHoverActive = false;
        if(this.menuOutsideClickListener){
            this.menuOutsideClickListener(); //déinscription au listener
            this.menuOutsideClickListener = null; //on met null pour bloquer permettre la redéfinition
        }
        this.unblockBodyScroll();//on débloque le scroll de la page
    }

    hideProfileMenu(){
        this.layoutService.state.profileSidebarVisible = false;
        if(this.profileMenuOutsideClickListener){
            this.profileMenuOutsideClickListener(); //on se désabonne
            this.profileMenuOutsideClickListener = null; //on met null pour bloquer permettre la redéfinition
        }
    }

    blockBodyScroll(): void {
        if(document.body.classList) {
            document.body.classList.add('blocked-scroll');
        }else {
            document.body.className += ' blocked-scroll';
        }
    }

    /**
     * Supprime la classe blocked-scroll des classes du body
     */
    unblockBodyScroll(): void {
        if(document.body.classList) {
            document.body.classList.remove('blocked-scroll');
        }else {
            document.body.className = document.body.className.replace(new RegExp('(^|\\b)' +
            'blocked-scroll'.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
        }
    }

    get containerClass(){
        //console.log("By ZKOF");
        return {
            'layout-theme-light': this.layoutService.config().colorScheme === 'light',
            'layout-theme-dark': this.layoutService.config().colorScheme === 'dark',
            'layout-overlay': this.layoutService.config().menuMode === 'overlay',
            'layout-static': this.layoutService.config().menuMode === 'static',
            'layout-static-inactive': this.layoutService.state.staticMenuDesktopInactive && this.layoutService.config().menuMode === 'static',
            'layout-overlay-active': this.layoutService.state.overlayMenuActive,
            'layout-mobile-active': this.layoutService.state.staticMenuMobileActive,
            'p-input-filled': this.layoutService.config().inputStyle === 'filled',
            'p-ripple-disabled': !this.layoutService.config().ripple
        }
    }

    ngOnDestroy(): void {
        if(this.overlayMenuOpenSubscription) {
            this.overlayMenuOpenSubscription.unsubscribe();
        }

        if(this.menuOutsideClickListener) {
            this.menuOutsideClickListener.unsubscribe();
        }

        if(this.profileMenuOutsideClickListener) {
            this.profileMenuOutsideClickListener.unsubscribe();
        }
    }
}
