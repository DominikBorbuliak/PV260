/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { HoldingChangeDto } from '../models/HoldingChangeDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class ReportService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @returns any OK
     * @throws ApiError
     */
    public generateReport(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/Report',
        });
    }
    /**
     * @returns HoldingChangeDto OK
     * @throws ApiError
     */
    public reportDiff({
        date,
    }: {
        date?: string,
    }): CancelablePromise<Array<HoldingChangeDto>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/api/Report',
            query: {
                'date': date,
            },
        });
    }
}
