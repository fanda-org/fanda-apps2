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
    path: ['/', 'pages', 'application'],
    open: false,
    selected: false,
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
