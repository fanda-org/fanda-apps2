export const authMenus = [
  {
    level: 1,
    title: 'Dashboard',
    icon: 'dashboard',
    path: ['/', 'pages'],
    open: false,
    selected: false,
    disabled: false,
  },
  {
    level: 1,
    title: 'Application',
    icon: 'snippets',
<<<<<<< HEAD
    router: '/pages/application',
    open: true,
    selected: true,
=======
    path: ['/', 'pages', 'application'],
    open: false,
    selected: false,
>>>>>>> d648d96ddbfd27c65c637dfd4e3131d4c50dc6a5
    disabled: false,
  },
  {
    level: 1,
    title: 'Tenants',
    icon: 'audit',
    open: false,
    children: [
      {
        level: 2,
        title: 'Users',
        icon: 'user',
      },
      {
        level: 2,
        title: 'Roles',
        icon: 'lock',
      },
    ],
  },
];
