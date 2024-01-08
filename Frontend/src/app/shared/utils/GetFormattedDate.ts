import { formatDate } from "@angular/common";

export function getFormattedDate(date: Date) {
    return formatDate(date, 'yyyy-MM-dd', 'en-US');
}