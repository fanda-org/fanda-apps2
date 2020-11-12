import { Menu } from './../_models/menu';

export const authMenus: Menu[] = [
    {
        level: 1,
        title: 'Dashboard',
        icon: 'dashboard',
        path: ['/', 'pages'],
        open: false,
        selected: false,
        disabled: false,
        children: null
    },
    {
        level: 1,
        title: 'Applications',
        icon: 'snippets',
        path: ['/', 'pages', 'applications'],
        open: false,
        selected: false,
        disabled: false,
        children: null
    },
    {
        level: 1,
        title: 'Tenants',
        icon: 'audit',
        path: null,
        open: false,
        selected: false,
        disabled: false,
        children: [
            {
                level: 2,
                title: 'Tenants',
                icon: 'audit',
                path: ['/', 'pages', 'tenants'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            },
            {
                level: 2,
                title: 'Users',
                icon: 'user',
                path: ['/', 'pages', 'users'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            },
            {
                level: 2,
                title: 'Roles',
                icon: 'lock',
                path: ['/', 'pages', 'roles'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            }
        ]
    }
];
