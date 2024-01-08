export interface ImageDto {
    id?: number
    name: string
    attachment?: Blob
    contentType: string
    contentLength: number
    extension: string
}
