import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import {
    ApplicationService,
    AlertService,
    HiddenDataService,
} from '../../_services';
import { Application } from '../../_models';
import { capitalize } from 'src/app/_utils';
import { NzTableQueryParams } from 'ng-zorro-antd/table';

@Component({
    selector: 'app-applications',
    templateUrl: './applications.component.html',
    styleUrls: ['./applications.component.css'],
})
export class ApplicationsComponent implements OnInit {
    total = 1;
    applications: Application[] = [];
    loading = true;
    pageSize = 10;
    page = 1;
    filterActive = [
        { text: 'Active', value: ['true'] },
        { text: 'Inactive', value: ['false'] },
        { text: 'Both', value: '' },
    ];

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private applicationService: ApplicationService,
        private alertService: AlertService,
        private hiddenService: HiddenDataService
    ) {}

    loadData(
        page: number,
        pageSize: number,
        sort: string | null,
        filter: Array<{ key: string; value: string[] }>
    ): void {
        this.loading = true;
        this.applicationService
            .getAll(page, pageSize, sort, filter)
            .subscribe((res) => {
                this.loading = false;
                this.applications = res.data;
                this.total = res.itemsCount;
                this.alertService.success('Loading successful', {
                    keepAfterRouteChange: true,
                });
            });
    }

    ngOnInit(): void {
        // this.loadData(this.page, this.pageSize, null, []);
    }

    onQueryParamsChange(params: NzTableQueryParams): void {
        console.log(params);
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
        this.loadData(pageIndex, pageSize, sortFieldOrder, filter);
    }
}
