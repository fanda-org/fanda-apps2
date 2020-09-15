export const fandaMenus = [
  {
    level: 1,
    title: 'Dashboard',
    icon: 'dashboard',
    open: true,
    selected: true,
    disabled: false,
  },
  {
    level: 1,
    title: 'Invoice',
    icon: 'form',
    open: false,
    selected: false,
    disabled: false,
    children: [
      {
        level: 2,
        title: 'Sales',
        icon: 'book',
        selected: false,
        disabled: false,
      },
      {
        level: 2,
        title: 'Sales Return',
        icon: 'credit-card',
        selected: false,
        disabled: false,
      },
      {
        level: 2,
        title: 'Purchase',
        icon: 'container',
        selected: false,
        disabled: false,
      },
      {
        level: 2,
        title: 'Purchase Return',
        icon: 'delivered-procedure',
        selected: false,
        disabled: false,
      },
      {
        level: 2,
        title: 'Opening Stock',
        icon: 'appstore-add',
        selected: false,
        disabled: false,
      },
    ],
  },
  {
    level: 1,
    title: 'Transaction',
    icon: 'desktop',
    children: [
      {
        level: 2,
        title: 'Receipts',
        icon: 'audit',
      },
      {
        level: 2,
        title: 'Payments',
        icon: 'carry-out',
      },
      {
        level: 2,
        title: 'Journals',
        icon: 'calculator',
      },
    ],
  },
  {
    level: 1,
    title: 'Inventory',
    icon: 'folder-open',
    children: [
      {
        level: 2,
        title: 'Goods and Services',
        icon: 'gift',
      },
      {
        level: 2,
        title: 'Categories',
        icon: 'apartment',
      },
      {
        level: 2,
        title: 'Units',
        icon: 'deployment-unit',
      },
      {
        level: 2,
        title: 'Brands',
        icon: 'gateway',
      },
      {
        level: 2,
        title: 'Segments',
        icon: 'block',
      },
      {
        level: 2,
        title: 'Varieties',
        icon: 'build',
      },
    ],
  },
  {
    level: 1,
    title: 'Contacts',
    icon: 'contacts',
    children: [
      {
        level: 2,
        title: 'Business Contacts',
        icon: 'contacts',
      },
      {
        level: 2,
        title: 'Banks',
        icon: 'bank',
      },
      {
        level: 2,
        title: 'Contact Categories',
        icon: 'fork',
      },
    ],
  },
  {
    level: 1,
    title: 'Accounts',
    icon: 'project',
    children: [
      {
        level: 2,
        title: 'Ledgers',
        icon: 'inbox',
      },
      {
        level: 2,
        title: 'Ledger Groups',
        icon: 'group',
      },
    ],
  },
];

/*
    children: [
      {
        level: 2,
        title: 'Group 1',
        icon: 'bars',
        open: false,
        selected: false,
        disabled: false,
        children: [
          {
            level: 3,
            title: 'Option 1',
            selected: false,
            disabled: false,
          },
          {
            level: 3,
            title: 'Option 2',
            selected: false,
            disabled: true,
          },
        ],
      },
      {
        level: 2,
        title: 'Group 2',
        icon: 'bars',
        selected: false,
        disabled: false,
      },
      {
        level: 2,
        title: 'Group 3',
        icon: 'bars',
        selected: false,
        disabled: false,
      },
    ],


[nzPaddingLeft]="menu.level * 24"
[nzPaddingLeft]="menu.level * 24"
*/
