export const authMenus = [
    {
        level: 1,
        title: 'Dashboard',
        icon: 'dashboard',
        path: ['/', 'pages'],
        open: false,
        selected: false,
        disabled: false
    },
    {
        level: 1,
        title: 'Applications',
        icon: 'snippets',
        path: ['/', 'pages', 'applications'],
        open: false,
        selected: false,
        disabled: false
    },
    {
        level: 1,
        title: 'Tenants',
        icon: 'audit',
        open: false,
        children: [
            {
                level: 2,
                title: 'Tenants',
                icon: 'audit',
                path: ['/', 'pages', 'tenants']
            },
            {
                level: 2,
                title: 'Users',
                icon: 'user',
                path: ['/', 'pages', 'users']
            },
            {
                level: 2,
                title: 'Roles',
                icon: 'lock'
            }
        ]
    }
];
