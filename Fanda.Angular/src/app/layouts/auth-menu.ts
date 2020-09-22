export const authMenus = [
  {
    level: 1,
    title: 'Dashboard',
    icon: 'dashboard',
    router: '/pages',
    open: true,
    selected: true,
    disabled: false,
  },
  {
    level: 1,
    title: 'Application',
    icon: 'snippets',
    router: '/pages/application',
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
