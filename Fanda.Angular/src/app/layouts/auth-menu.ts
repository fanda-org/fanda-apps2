export const authMenus = [
  {
    level: 1,
    title: 'Application',
    icon: 'snippets',
  },
  {
    level: 1,
    title: 'Tenants',
    icon: 'audit',
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
