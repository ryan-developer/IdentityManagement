export default class PaginationResponse<TType> {
    page: number
    pageCount: number
    itemCount: number
    totalItems: number
    items: TType[]
}