export interface Menu {
    level: number;
    title: string;
    icon: string;
    path: string[];
    open: boolean;
    selected: boolean;
    disabled: boolean;
    children: Menu[];
}
