import { __awaiter } from "tslib";
import HttpClient from '@/HttpClients/HttpClient';
export class AuthClient extends HttpClient {
    constructor() {
        super('/api/auth');
    }
    GetAuth() {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this.invokeRequest();
        });
    }
}
export default new AuthClient();
//# sourceMappingURL=AuthClient.js.map