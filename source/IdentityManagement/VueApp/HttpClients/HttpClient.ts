import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';

export enum HttpStatusCode {
    Ok = 200,
    NoContent = 204,
    NotFound = 404,
    Unauthorized = 401,
    Redirect = 302
}

export default class HttpClient {
    httpClient: AxiosInstance
    baseURL: string

    constructor(basePath: string) {
        this.baseURL = basePath;
        this.httpClient = axios.create({
            baseURL: this.baseURL,
        });
    }

    async invokeRequest<T>(requestConfig?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        if (!requestConfig) {
            requestConfig = {};
        }
        requestConfig.headers = {};
        requestConfig.withCredentials = true;

        try {
            const token = await this.getToken();
            requestConfig.headers.Authorization = `Bearer ${token}`;
            return this.httpClient.request<T>(requestConfig);
        }
        catch (error) {
            console.dir(error);
            // log/retry with incremental backoff depending on error
        }
    }

    private async getToken() {
        const token = window.sessionStorage.getItem('token');
        const tokenExpiration = window.sessionStorage.getItem('token_expiration');
        const now = new Date();
        const tokenExpirationDate = new Date(tokenExpiration);

        if (!token || !tokenExpiration || now >= tokenExpirationDate) {
            //const tokenResponse = await axios.get<UserProfileToken>('/api/user-profile/token');
            //if (!tokenResponse || tokenResponse.status !== HttpStatusCode.Ok) {
            //    throw new Error(tokenResponse.statusText);
            //}

            //const expirationDate = new Date(tokenResponse.data.expiration_date);
            //window.sessionStorage.setItem('token', tokenResponse.data.token);
            //window.sessionStorage.setItem('token_expiration', expirationDate.toUTCString());
        }

        return token;
    }
}