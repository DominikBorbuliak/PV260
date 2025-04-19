/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AccessTokenResponse } from '../models/AccessTokenResponse';
import type { ForgotPasswordRequest } from '../models/ForgotPasswordRequest';
import type { InfoRequest } from '../models/InfoRequest';
import type { InfoResponse } from '../models/InfoResponse';
import type { LoginRequest } from '../models/LoginRequest';
import type { RefreshRequest } from '../models/RefreshRequest';
import type { RegisterRequest } from '../models/RegisterRequest';
import type { ResendConfirmationEmailRequest } from '../models/ResendConfirmationEmailRequest';
import type { ResetPasswordRequest } from '../models/ResetPasswordRequest';
import type { TwoFactorRequest } from '../models/TwoFactorRequest';
import type { TwoFactorResponse } from '../models/TwoFactorResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class Pv260ProjectServerService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @returns any OK
     * @throws ApiError
     */
    public postApiUserRegister({
        requestBody,
    }: {
        requestBody: RegisterRequest,
    }): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/register',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @returns AccessTokenResponse OK
     * @throws ApiError
     */
    public postApiUserLogin({
        requestBody,
        useCookies,
        useSessionCookies,
    }: {
        requestBody: LoginRequest,
        useCookies?: boolean,
        useSessionCookies?: boolean,
    }): CancelablePromise<AccessTokenResponse> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/login',
            query: {
                'useCookies': useCookies,
                'useSessionCookies': useSessionCookies,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns AccessTokenResponse OK
     * @throws ApiError
     */
    public postApiUserRefresh({
        requestBody,
    }: {
        requestBody: RefreshRequest,
    }): CancelablePromise<AccessTokenResponse> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/refresh',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public mapIdentityApiApiUserConfirmEmail({
        userId,
        code,
        changedEmail,
    }: {
        userId: string,
        code: string,
        changedEmail?: string,
    }): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/api/User/confirmEmail',
            query: {
                'userId': userId,
                'code': code,
                'changedEmail': changedEmail,
            },
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public postApiUserResendConfirmationEmail({
        requestBody,
    }: {
        requestBody: ResendConfirmationEmailRequest,
    }): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/resendConfirmationEmail',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public postApiUserForgotPassword({
        requestBody,
    }: {
        requestBody: ForgotPasswordRequest,
    }): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/forgotPassword',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public postApiUserResetPassword({
        requestBody,
    }: {
        requestBody: ResetPasswordRequest,
    }): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/resetPassword',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @returns TwoFactorResponse OK
     * @throws ApiError
     */
    public postApiUserManage2Fa({
        requestBody,
    }: {
        requestBody: TwoFactorRequest,
    }): CancelablePromise<TwoFactorResponse> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/manage/2fa',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                404: `Not Found`,
            },
        });
    }
    /**
     * @returns InfoResponse OK
     * @throws ApiError
     */
    public getApiUserManageInfo(): CancelablePromise<InfoResponse> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/api/User/manage/info',
            errors: {
                400: `Bad Request`,
                404: `Not Found`,
            },
        });
    }
    /**
     * @returns InfoResponse OK
     * @throws ApiError
     */
    public postApiUserManageInfo({
        requestBody,
    }: {
        requestBody: InfoRequest,
    }): CancelablePromise<InfoResponse> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/api/User/manage/info',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                404: `Not Found`,
            },
        });
    }
}
