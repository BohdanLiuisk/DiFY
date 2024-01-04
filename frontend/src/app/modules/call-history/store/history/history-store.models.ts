import { PaginationConfig } from '@core/models/pagination.config';
import { Call } from '../../models/call-history.models';

export interface CallHistoryState {
	paginationConfig: PaginationConfig;
	calls: Call[];
	totalCount: number;
	loading: boolean;
	error: string;
}
