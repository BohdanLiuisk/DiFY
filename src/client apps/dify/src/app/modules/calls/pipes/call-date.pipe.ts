import { DATE_PIPE_DEFAULT_TIMEZONE, formatDate } from '@angular/common';
import { Inject, LOCALE_ID, Optional, Pipe, PipeTransform } from '@angular/core';
import { dify } from '@shared/constans/app-settings';

@Pipe({ name: 'callDate' })
export class CallDatePipe implements PipeTransform {
    constructor(
        @Inject(LOCALE_ID) private locale: string,
        @Inject(DATE_PIPE_DEFAULT_TIMEZONE) @Optional() private defaultTimezone?: string | null,
    ) { }

    transform(value: Date | string, format?: string, defaultValue?: string, timezone?: string, locale?: string): string {
        if(new Date(value).getFullYear() === 1) {
            return defaultValue ?? dify.emptyString;
        } else {
            const _format = format ?? 'mediumDate';
            const _timezone = timezone ?? this.defaultTimezone ?? undefined;
            return formatDate(value, _format, this.locale ?? locale, _timezone);
        }
    }
}
