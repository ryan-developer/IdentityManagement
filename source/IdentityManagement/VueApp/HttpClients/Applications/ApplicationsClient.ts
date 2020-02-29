import HttpClient from '@/HttpClients/HttpClient';
import PaginationList from '@/HttpClients/Shared/PaginationList';
import ApplicationResponse from '@/HttpClients/Applications/Contracts/ApplicationResponse';

export class ApplicationsClient extends HttpClient {
    constructor() {
        super('/api/applications');
    }

    async GetAll() {
        return await this.invokeRequest<PaginationList<ApplicationResponse>>();
    }

    async Get(userId: string) {
        return await this.invokeRequest<ApplicationResponse>({
            url: userId
        });
    }
}

export default new ApplicationsClient();