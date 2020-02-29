import HttpClient from '@/HttpClients/HttpClient';
import PaginationList from '@/HttpClients/Shared/PaginationList';
import UserResponse from '@/HttpClients/Users/Contracts/UserResponse';


export class UsersClient extends HttpClient {
    constructor() {
        super('/api/users');
    }

    async GetAllUsers() {
        return await this.invokeRequest<PaginationList<UserResponse>>();
    }

    async GetUser(userId: string) {
        return await this.invokeRequest<UserResponse>({
            url: userId
        });
    }
}

export default new UsersClient();