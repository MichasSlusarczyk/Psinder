export function getLocale(): string {
    return localStorage.getItem('language') === 'en' ? 'en-GB' : 'pl';
}