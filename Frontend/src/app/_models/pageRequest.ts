export class PageRequest<T> {
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
  pageNumber: number;
  pageSize: number;
  items: T[];
}
