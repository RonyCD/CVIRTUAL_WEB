import { Injectable, effect, signal } from '@angular/core';
import { Subject } from 'rxjs';

export interface AppConfig {
    inputStyle: string,
    colorScheme: string,
    theme: string,
    ripple: boolean,
    menuMode: string,
    scale: number
}

export interface LayoutState {
    staticMenuDesktopInactive: boolean,
    overlayMenuActive: boolean,
    profileSidebarVisible: boolean,
    configSidebarVisible: boolean,
    staticMenuMobileActive: boolean,
    menuHoverActive: boolean
}

@Injectable({
    providedIn: 'root'
})
export class LayoutService {
    _config: AppConfig = {
        inputStyle: 'outlined',
        colorScheme: 'light',
        theme: 'lara-light-indigo',
        ripple: true,
        menuMode: 'static',
        scale: 14
    }

    state: LayoutState = {
        staticMenuDesktopInactive: false,
        overlayMenuActive: false,
        profileSidebarVisible: false,
        configSidebarVisible: false,
        staticMenuMobileActive: false,
        menuHoverActive: false
    }

    config = signal<AppConfig>(this._config);
    private configUpdate = new Subject<AppConfig>();
    configUpdate$ = this.configUpdate.asObservable();

    overlayOpen$ = new Subject<any>(); //indique si le menu est superposé sur le contenu de la page

    constructor(){
        effect( () => {
            const config = this.config();
            if(this.updateStyle(config)){
                this.changeTheme()
            }
            this.changeScale(config.scale);
            this.onConfigUpdate();
        });
    }
    
    /**
     * S'occupe d'afficher ou de cacher le menu
     */
    onMenuToggle() {  
        if(this.isOverlay()){
            this.state.overlayMenuActive = !this.state.overlayMenuActive;
            if(this.state.overlayMenuActive){
                //si on a activer la superposition on informe les écouteur que le menu doit être superposé
                this.overlayOpen$.next(undefined);
            }
        }
        if(this.isDesktop()){
            this.state.staticMenuDesktopInactive = !this.state.staticMenuDesktopInactive;
        }else{
            this.state.staticMenuMobileActive = !this.state.staticMenuMobileActive;
            if(this.state.staticMenuMobileActive){
                //Si on active le menu sur un mobile, c'est forcément de la superposition
                this.overlayOpen$.next(undefined);
            }
        }
    }

    showProfileSidebar(){
        this.state.profileSidebarVisible = !this.state.profileSidebarVisible;
        if(this.state.profileSidebarVisible){
            //Si on ouvre la bar latérale on prévient que ca sera superposé
            this.overlayOpen$.next(null);
        }
    }

    showConfigSidebar() {
        this.state.configSidebarVisible = true;
    }

    /**
     * indique si le menu est superposé ou pas
     * @returns 
     */
    isOverlay(){
        return this.config().menuMode === 'overlay';
    }

    isDesktop(){
        return window.innerWidth > 991;
    }

    updateStyle(config: AppConfig) {
        return (
            config.theme !== this._config.theme ||
            config.colorScheme !== this._config.colorScheme
        );
    }

    onConfigUpdate() {
        this._config = { ...this.config() };
        this.configUpdate.next(this.config());
    }

    changeTheme() {
        const config = this.config();
        const themeLink = <HTMLLinkElement>document.getElementById('theme-css');
        themeLink.setAttribute('href', config.theme+".css");
        console.log("By ZKOF");
        /*
        const themeLinkHref = themeLink.getAttribute('href')!;
        const newHref = themeLinkHref
            .split('/')
            .map((el) =>
                el == this._config.theme
                    ? (el = config.theme)
                    : el == `theme-${this._config.colorScheme}`
                    ? (el = `theme-${config.colorScheme}`)
                    : el
            )
            .join('/');
        this.replaceThemeLink(newHref);
        */
    }
    replaceThemeLink(href: string) {
        const id = 'theme-css';
        let themeLink = <HTMLLinkElement>document.getElementById(id);
        const cloneLinkElement = <HTMLLinkElement>themeLink.cloneNode(true);

        cloneLinkElement.setAttribute('href', href);
        cloneLinkElement.setAttribute('id', id + '-clone');

        themeLink.parentNode!.insertBefore(
            cloneLinkElement,
            themeLink.nextSibling
        );
        cloneLinkElement.addEventListener('load', () => {
            themeLink.remove();
            cloneLinkElement.setAttribute('id', id);
        });
    }

    changeScale(value: number) {
        document.documentElement.style.fontSize = `${value}px`;
    }
}