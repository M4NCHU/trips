export interface PagedResult<T> {
  items: T[];
  totalItems: number;
  pageSize: number;
  currentPage: number;
}
