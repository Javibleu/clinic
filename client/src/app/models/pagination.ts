/*
 * Interface for our pagination information
 * The 4 properties must match exactly what we got in the Response Header
*/
export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

/* This class is gonna be used for any of our different types <T>
*/
export class PaginatedResult<T> {

    // T represents an array of Members, So our result, our list of members are going to be stored in the result property
    result: T;
    // pagination is a Type of Pagination(interface above)and the pagination information is going to be stored in the pagination.
    pagination: Pagination;
}
