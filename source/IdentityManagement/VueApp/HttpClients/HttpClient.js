import { __awaiter } from "tslib";
import axios from 'axios';
export var HttpStatusCode;
(function (HttpStatusCode) {
    HttpStatusCode[HttpStatusCode["Ok"] = 200] = "Ok";
    HttpStatusCode[HttpStatusCode["NoContent"] = 204] = "NoContent";
    HttpStatusCode[HttpStatusCode["NotFound"] = 404] = "NotFound";
    HttpStatusCode[HttpStatusCode["Unauthorized"] = 401] = "Unauthorized";
    HttpStatusCode[HttpStatusCode["Redirect"] = 302] = "Redirect";
})(HttpStatusCode || (HttpStatusCode = {}));
export default class HttpClient {
    constructor(basePath) {
        this.baseURL = basePath;
        this.httpClient = axios.create({
            baseURL: this.baseURL,
        });
    }
    invokeRequest(requestConfig) {
        return __awaiter(this, void 0, void 0, function* () {
            if (!requestConfig) {
                requestConfig = {};
            }
            requestConfig.headers = {};
            requestConfig.withCredentials = true;
            try {
                const token = yield this.getToken();
                requestConfig.headers.Authorization = `Bearer ${token}`;
                return this.httpClient.request(requestConfig);
            }
            catch (error) {
                console.dir(error);
                // log/retry with incremental backoff depending on error
            }
        });
    }
    getToken() {
        return __awaiter(this, void 0, void 0, function* () {
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
        });
    }
}
//# sourceMappingURL=HttpClient.js.map