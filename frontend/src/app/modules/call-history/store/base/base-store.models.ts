import { CallHistorySection } from '../../models/call-history.models';

export interface BaseCallHistoryState {
  sections: CallHistorySection[];
	loading: boolean;
	error: string;
}
