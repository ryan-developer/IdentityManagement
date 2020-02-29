import HttpClient from '@/HttpClients/HttpClient';
import PaginationList from '@/HttpClients/Shared/PaginationList';
import RoleResponse from '@/HttpClients/Roles/Contracts/RoleResponse';


export class RoleClient extends HttpClient {
    constructor() {
        super('/api/roles');
    }

    async GetAll() {
        return await this.invokeRequest<PaginationList<RoleResponse>>();
    }

    async Get(userId: string) {
        return await this.invokeRequest<RoleResponse>({
            url: userId
        });
    }
}

export default new RoleClient();