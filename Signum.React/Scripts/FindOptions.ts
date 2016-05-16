﻿import { TypeReference, PropertyRoute, PseudoType, QueryKey } from './Reflection';
import { Dic } from './Globals';
import { Lite, Entity } from './Signum.Entities';
import { PaginationMode, OrderType, FilterOperation, FilterType, ColumnOptionsMode, UniqueType } from './Signum.Entities.DynamicQuery';

export { PaginationMode, OrderType, FilterOperation, FilterType, ColumnOptionsMode, UniqueType };

export interface CountOptions {
    queryName: PseudoType | QueryKey;
    filterOptions?: FilterOption[];
}

export interface FindOptions {
    queryName: PseudoType | QueryKey;
    parentColumn?: string;
    parentValue?: any;

    filterOptions?: FilterOption[];
    orderOptions?: OrderOption[];
    columnOptionsMode?: ColumnOptionsMode;
    columnOptions?: ColumnOption[];
    pagination?: Pagination

    searchOnLoad?: boolean;
    showHeader?: boolean;
    showFilters?: boolean;
    showFilterButton?: boolean;
    showFooter?: boolean;
    allowChangeColumns?: boolean;
    create?: boolean;
    navigate?: boolean;
    contextMenu?: boolean;
}

export function expandParentColumn(findOptions: FindOptions) {

    if (!findOptions.parentColumn)
        return findOptions; 

    var fo = Dic.extend({}, findOptions) as FindOptions;

    fo.filterOptions = [
        { columnName: fo.parentColumn, operation: "EqualTo", value: fo.parentValue, frozen: true },
        ...(fo.filterOptions || [])
    ];

    if (!fo.parentColumn.contains(".") && (fo.columnOptionsMode == null || fo.columnOptionsMode == "Remove")) {
        fo.columnOptions = [
            { columnName: fo.parentColumn },
            ...(fo.columnOptions || [])
        ];

        fo.columnOptionsMode = "Remove";
    }

    if (fo.searchOnLoad == null)
        fo.searchOnLoad = true;

    fo.parentColumn = null;
    fo.parentValue = null;

    return fo;
}

export interface FilterOption {
    columnName: string;
    token?: QueryToken;
    frozen?: boolean;
    operation?: FilterOperation;
    value: any;
}




export interface OrderOption {
    columnName: string;
    token?: QueryToken;
    orderType: OrderType;
}


export interface ColumnOption {
    columnName: string;
    token?: QueryToken;
    displayName?: string;
}


export const DefaultPagination: Pagination = {
    mode: "Paginate",
    elementsPerPage: 20,
    currentPage: 1
};


export enum FindMode {
    Find = <any>"Find",
    Explore = <any>"Explore"
}

export enum SubTokensOptions {
    CanAggregate = 1,
    CanAnyAll = 2,
    CanElement = 4,
}

export interface QueryToken {
    toString: string;
    niceName: string;
    key: string;
    format?: string;
    unit?: string;
    type: TypeReference;
    typeColor: string;
    niceTypeName: string;
    isGroupable: boolean;
    filterType: FilterType;
    fullKey: string;
    queryTokenType?: QueryTokenType;
    parent?: QueryToken;
}

export enum QueryTokenType {
    Aggregate = "Aggregate" as any,
    Element = "Element" as any,
    AnyOrAll = "AnyOrAll" as any,
}

export function hasAnyOrAll(token: QueryToken) : boolean {
    if(token == null)
        return false;

    if(token.queryTokenType == QueryTokenType.AnyOrAll)
        return true;

    return hasAnyOrAll(token.parent);
}

export function hasAggregate(token: QueryToken): boolean {
    if (token == null)
        return false;

    if (token.queryTokenType == QueryTokenType.Aggregate)
        return true;

    return hasAggregate(token.parent);
}

export function getTokenParents(token: QueryToken): QueryToken[] {
    const result = [];
    while (token != null) {
        result.insertAt(0, token);
        token = token.parent;
    }
    return result;
}

export function toQueryToken(cd: ColumnDescription): QueryToken {
    return {
        toString: cd.displayName,
        niceName: cd.displayName,
        key: cd.name,
        fullKey: cd.name,
        unit: cd.unit,
        format: cd.format,
        type: cd.type,
        typeColor: cd.typeColor,
        niceTypeName: cd.niceTypeName,
        filterType: cd.filterType,
        isGroupable: cd.isGroupable,
    };
}

export interface FilterRequest {
    token: string;
    operation: FilterOperation;
    value: any;
}

export interface OrderRequest {
    token: string;
    orderType: OrderType
}

export interface ColumnRequest {
    token: string;
    displayName: string;
}

export interface QueryRequest {
    queryKey: string;
    filters: FilterRequest[];
    orders: OrderRequest[];
    columns: ColumnRequest[];
    pagination: Pagination;
}

export interface CountQueryRequest {
    queryKey: string;
    filters: FilterRequest[];
}

export interface ResultColumn {
    displayName: string;
    token: QueryToken;
}

export interface ResultTable {
    queryKey: string;
    entityColumn: string;
    columns: string[];
    rows: ResultRow[];
    pagination: Pagination
    totalElements: number;
}


export interface ResultRow {
    entity: Lite<Entity>;
    columns: any[];
}

export interface Pagination {
    mode: PaginationMode;
    elementsPerPage?: number;
    currentPage?: number;
}

export module PaginateMath {
    export function startElementIndex(p: Pagination) {
        return (p.elementsPerPage * (p.currentPage - 1)) + 1;
    }

    export function endElementIndex(p: Pagination, rows: number) {
        return startElementIndex(p) + rows - 1;
    }

    export function totalPages(p: Pagination, totalElements: number) {
        return Math.ceil(totalElements / p.elementsPerPage); //Round up
    }

    export function maxElementIndex(p: Pagination) {
        return (p.elementsPerPage * (p.currentPage + 1)) - 1;
    }
}





export interface QueryDescription {
    queryKey: string;
    columns: { [name: string]: ColumnDescription };
}

export interface ColumnDescription {
    name: string;
    type: TypeReference;
    filterType: FilterType;
    typeColor: string;
    niceTypeName: string;
    unit?: string;
    format?: string;
    displayName: string;
    isGroupable: boolean;
}

export function isList(fo: FilterOperation) {
    return fo == "IsIn" ||
        fo == "IsNotIn";
}


export const filterOperations: { [a: string /*FilterType*/]: FilterOperation[] } = {};
filterOperations["String"] = [
    "Contains",
    "EqualTo",
    "StartsWith",
    "EndsWith",
    "Like",
    "NotContains",
    "DistinctTo",
    "NotStartsWith",
    "NotEndsWith",
    "NotLike",
    "IsIn",
    "IsNotIn"
];

filterOperations["DateTime"] = [
    "EqualTo",
    "DistinctTo",
    "GreaterThan",
    "GreaterThanOrEqual",
    "LessThan",
    "LessThanOrEqual",
    "IsIn",
    "IsNotIn"
];

filterOperations["Integer"] = [
    "EqualTo",
    "DistinctTo",
    "GreaterThan",
    "GreaterThanOrEqual",
    "LessThan",
    "LessThanOrEqual",
    "IsIn",
    "IsNotIn"
];

filterOperations["Decimal"] = [
    "EqualTo",
    "DistinctTo",
    "GreaterThan",
    "GreaterThanOrEqual",
    "LessThan",
    "LessThanOrEqual",
    "IsIn",
    "IsNotIn"
];

filterOperations["Enum"] = [
    "EqualTo",
    "DistinctTo",
    "IsIn",
    "IsNotIn"
];

filterOperations["Guid"] = [
    "EqualTo",
    "DistinctTo",
    "IsIn",
    "IsNotIn"
];

filterOperations["Lite"] = [
    "EqualTo",
    "DistinctTo",
    "IsIn",
    "IsNotIn"
];

filterOperations["Embedded"] = [
    "EqualTo",
    "DistinctTo",
];

filterOperations["Boolean"] = [
    "EqualTo",
    "DistinctTo",
];