import { Inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
    providedIn: 'root',
})
export class ThemeService {
	constructor(@Inject(DOCUMENT) private document: Document) {}

	public switchTheme(theme: string) {
		const themeLink = this.document.getElementById('app-theme') as HTMLLinkElement;
		if (themeLink) {
			themeLink.href = `lara-${theme}-indigo.css`;
		}
	}
}
