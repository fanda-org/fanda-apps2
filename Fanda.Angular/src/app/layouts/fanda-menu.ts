import { Menu } from '../_models';

export const fandaMenus: Menu[] = [
    {
        level: 1,
        title: 'Dashboard',
        icon: 'dashboard',
        path: ['/', 'pages'],
        open: true,
        selected: true,
        disabled: false,
        children: null
    },
    // {
    //     level: 1,
    //     title: 'Invoice',
    //     icon: 'form',
    //     path: ['/', 'pages'],
    //     open: false,
    //     selected: false,
    //     disabled: false,
    //     children: [
    //         {
    //             level: 2,
    //             title: 'Sales',
    //             icon: 'book',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Sales Return',
    //             icon: 'credit-card',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Purchase',
    //             icon: 'container',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Purchase Return',
    //             icon: 'delivered-procedure',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Opening Stock',
    //             icon: 'appstore-add',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         }
    //     ]
    // },
    {
        level: 1,
        title: 'Transaction',
        icon: 'desktop',
        path: null,
        open: false,
        selected: false,
        disabled: false,
        children: [
            {
                level: 2,
                title: 'Receipts',
                icon: 'audit',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            },
            {
                level: 2,
                title: 'Payments',
                icon: 'carry-out',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            },
            {
                level: 2,
                title: 'Journals',
                icon: 'calculator',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            }
        ]
    },
    // {
    //     level: 1,
    //     title: 'Inventory',
    //     icon: 'folder-open',
    //     path: null,
    //     open: false,
    //     selected: false,
    //     disabled: false,
    //     children: [
    //         {
    //             level: 2,
    //             title: 'Goods and Services',
    //             icon: 'gift',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Categories',
    //             icon: 'apartment',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Units',
    //             icon: 'deployment-unit',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Brands',
    //             icon: 'gateway',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Segments',
    //             icon: 'block',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         },
    //         {
    //             level: 2,
    //             title: 'Varieties',
    //             icon: 'build',
    //             path: ['/', 'pages'],
    //             open: false,
    //             selected: false,
    //             disabled: false,
    //             children: null
    //         }
    //     ]
    // },
    {
        level: 1,
        title: 'Contacts',
        icon: 'contacts',
        path: null,
        open: false,
        selected: false,
        disabled: false,
        children: [
            {
                level: 2,
                title: 'Business Contacts',
                icon: 'contacts',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            },
            {
                level: 2,
                title: 'Banks',
                icon: 'bank',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            },
            {
                level: 2,
                title: 'Contact Categories',
                icon: 'fork',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            }
        ]
    },
    {
        level: 1,
        title: 'Accounts',
        icon: 'project',
        path: null,
        open: false,
        selected: false,
        disabled: false,
        children: [
            {
                level: 2,
                title: 'Ledgers',
                icon: 'inbox',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            },
            {
                level: 2,
                title: 'Ledger Groups',
                icon: 'group',
                path: ['/', 'pages'],
                open: false,
                selected: false,
                disabled: false,
                children: null
            }
        ]
    }
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
