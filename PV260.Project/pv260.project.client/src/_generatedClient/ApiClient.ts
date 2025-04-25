/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { BaseHttpRequest } from './core/BaseHttpRequest';
import type { OpenAPIConfig } from './core/OpenAPI';
import { AxiosHttpRequest } from './core/AxiosHttpRequest';
import { Pv260ProjectServerService } from './services/Pv260ProjectServerService';
import { ReportService } from './services/ReportService';
import { UserService } from './services/UserService';
type HttpRequestConstructor = new (config: OpenAPIConfig) => BaseHttpRequest;
export class ApiClient {
    public readonly pv260ProjectServer: Pv260ProjectServerService;
    public readonly report: ReportService;
    public readonly user: UserService;
    public readonly request: BaseHttpRequest;
    constructor(config?: Partial<OpenAPIConfig>, HttpRequest: HttpRequestConstructor = AxiosHttpRequest) {
        this.request = new HttpRequest({
            BASE: config?.BASE ?? 'https://localhost:7149',
            VERSION: config?.VERSION ?? '1.0.0',
            WITH_CREDENTIALS: config?.WITH_CREDENTIALS ?? false,
            CREDENTIALS: config?.CREDENTIALS ?? 'include',
            TOKEN: config?.TOKEN,
            USERNAME: config?.USERNAME,
            PASSWORD: config?.PASSWORD,
            HEADERS: config?.HEADERS,
            ENCODE_PATH: config?.ENCODE_PATH,
        });
        this.pv260ProjectServer = new Pv260ProjectServerService(this.request);
        this.report = new ReportService(this.request);
        this.user = new UserService(this.request);
    }
}

