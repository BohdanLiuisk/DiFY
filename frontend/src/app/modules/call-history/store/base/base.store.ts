import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { BaseCallHistoryState } from './base-store.models';
import { CallHistorySection } from '../../models/call-history.models';

@Injectable()
export class BaseCallHistoryStore extends ComponentStore<BaseCallHistoryState> {
  
  constructor() {
    super({
      sections: [],
      loading: false,
      error: ''
    });
  }

  public readonly sections$ = this.select(state => state.sections);

  public readonly setSections = this.updater((state, sections: CallHistorySection[]) => ({
    ...state, sections
  }));

  public readonly setSectionLoading = this.updater((state, code: string) => {
    const updatedSections = state.sections.map(section =>
      section.code === code ? { ...section, loading: true } : section
    );
    return { ...state, loading: true, sections: updatedSections };
  });

  public readonly setSectionLoaded = this.updater((state, code: string) => {
    const updatedSections = state.sections.map(section =>
      section.code === code ? { ...section, loading: false } : section
    );
    return { ...state, loading: false, sections: updatedSections };
  });
}
