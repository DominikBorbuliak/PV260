/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class UserService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @returns any OK
     * @throws ApiError
     */
    public logout(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/logout',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public pingauth(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/api/User/pingauth',
        });
    }
}
