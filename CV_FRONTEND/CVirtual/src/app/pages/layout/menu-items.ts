import { MenuItem } from "primeng/api";

export const MENU: MenuItem[] = [
    {
        separator: true
    },
    {
        label: 'Documents',
        items: [
            {
                label: 'New',
                icon: 'pi pi-plus',
                shortcut: '⌘+N',
                command: () => {
                    alert("New item clicked");
                }
            },
            {
                label: 'Search',
                icon: 'pi pi-search',
                shortcut: '⌘+S'
            }
        ]
    },
    {
        label: 'Profile',
        items: [
            {
                label: 'Settings',
                icon: 'pi pi-cog',
                shortcut: '⌘+O'
            },
            {
                label: 'Messages',
                icon: 'pi pi-inbox',
                badge: '2'
            },
            {
                label: 'Logout',
                icon: 'pi pi-sign-out',
                shortcut: '⌘+Q'
            }
        ]
    }
];