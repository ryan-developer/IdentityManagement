import { __awaiter } from "tslib";
import HttpClient from '@/HttpClients/HttpClient';
export class ApplicationsClient extends HttpClient {
    constructor() {
        super('/api/applications');
    }
    GetAll() {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this.invokeRequest();
        });
    }
    Get(userId) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this.invokeRequest({
                url: userId
            });
        });
    }
}
export default new ApplicationsClient();
//# sourceMappingURL=ApplicationsClient.js.map