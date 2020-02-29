import { __awaiter } from "tslib";
import HttpClient from '@/HttpClients/HttpClient';
export class UsersClient extends HttpClient {
    constructor() {
        super('/api/users');
    }
    GetAllUsers() {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this.invokeRequest();
        });
    }
    GetUser(userId) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this.invokeRequest({
                url: userId
            });
        });
    }
}
export default new UsersClient();
//# sourceMappingURL=UsersClient.js.map