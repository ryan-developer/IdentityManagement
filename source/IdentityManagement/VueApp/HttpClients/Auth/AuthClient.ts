import HttpClient from '@/HttpClients/HttpClient';
import UserAuthResponse from '@/HttpClients/Auth/UserAuthResponse';

export class AuthClient extends HttpClient {
    constructor() {
        super('/api/auth');
    }

    async GetAuth() {
        return await this.invokeRequest<UserAuthResponse>();
    }
}

export default new AuthClient();