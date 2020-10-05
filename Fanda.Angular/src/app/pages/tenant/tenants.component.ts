import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NzTableQueryParams } from 'ng-zorro-antd/table';

import { Tenant, FilterModel } from '../../_models';
import {
    TenantService,
    AlertService,
    HiddenDataService
} from '../../_services';

@Component({
    selector: 'app-tenants',
    templateUrl: './tenants.component.html',
    styleUrls: ['./tenants.component.css']
})
export class TenantsComponent implements OnInit {
    loading = true;
    total = 1;
    tenants: Tenant[] = null;
    pageIndex = 1;
    pageSize = 10;
    sortFieldOrder = '';
    filters: Array<FilterModel> = null;
    nameFilter: FilterModel = null;
    customFilters: Array<FilterModel> = null;

    customSearchValue = '';
    nameSearchValue = '';
    nameDropdownVisible = false;

    filterActive = [
        { text: 'Active', value: true },
        { text: 'Inactive', value: false },
        { text: 'Both', value: null }
    ];

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private tenantService: TenantService,
        private alertService: AlertService,
        private hiddenService: HiddenDataService
    ) {}

    resetNameSearch(): void {
        this.nameSearchValue = '';
        this.searchName();
    }

    searchName(): void {
        this.nameDropdownVisible = false;
        if (this.nameSearchValue) {
            this.nameFilter = { key: 'name', value: this.nameSearchValue };
        } else {
            this.nameFilter = null;
        }
        this.loadData(
            this.pageIndex,
            this.pageSize,
            this.sortFieldOrder,
            [...this.filters, this.nameFilter],
            this.customFilters
        );
    }

    resetCustomSearch(): void {
        this.customSearchValue = '';
        this.searchCustom();
    }

    searchCustom(): void {
        // console.log('custom search');
        if (this.customSearchValue) {
            this.customFilters = [
                { key: 'code', value: this.customSearchValue },
                { key: 'name', value: this.customSearchValue },
                { key: 'description', value: this.customSearchValue }
            ];
        } else {
            this.customFilters = null;
        }
        this.loadData(
            this.pageIndex,
            this.pageSize,
            this.sortFieldOrder,
            [...this.filters, this.nameFilter],
            this.customFilters
        );
    }

    loadData(
        pageIndex: number,
        pageSize: number,
        sort: string | null,
        filters: Array<FilterModel>,
        customFilters: Array<FilterModel>
    ): void {
        // console.log('Filters', filters);
        this.loading = true;
        this.tenantService
            .getAll(pageIndex, pageSize, sort, filters, customFilters)
            .subscribe(
                (response) => {
                    this.loading = false;
                    this.tenants = response.data;
                    this.total = response.itemsCount;
                    this.alertService.success('Loading successful', {
                        keepAfterRouteChange: true
                    });
                },
                (error) => {
                    this.loading = false;
                    // this.nameSearchValue = '';
                    if (this.nameFilter) {
                        this.nameDropdownVisible = true;
                    }
                    console.log('Error', error);
                }
            );
    }

    activate(id: string, active: boolean): void {
        console.log('Activate', id, active);
        this.tenantService.activate(id, active).subscribe(
            (res) => {
                console.log('Activated');
            },
            (err) => {
                console.log('Activate failed', err);
            }
        );
    }

    ngOnInit(): void {
        // this.loadData(this.page, this.pageSize, null, []);
    }

    onQueryParamsChange(params: NzTableQueryParams): void {
        console.log('Params', params);
        const { pageSize, pageIndex, sort, filter } = params;
        const currentSort = sort.find((item) => item.value !== null);
        const sortField = (currentSort && currentSort.key) || '';
        let sortOrder = (currentSort && currentSort.value) || '';
        if (sortOrder) {
            sortOrder = sortOrder.startsWith('asc')
                ? currentSort.value.substring(0, 3)
                : currentSort.value.substring(0, 4);
        }
        const sortFieldOrder =
            (currentSort && sortField + ' ' + sortOrder) || '';

        this.pageIndex = pageIndex;
        this.pageSize = pageSize;
        this.sortFieldOrder = sortFieldOrder;
        this.filters = filter;
        console.log('NameFilter', this.nameFilter);
        this.loadData(
            pageIndex,
            pageSize,
            sortFieldOrder,
            [...filter, this.nameFilter],
            this.customFilters
        );
    }
}
