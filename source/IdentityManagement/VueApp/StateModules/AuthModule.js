import { __awaiter } from "tslib";
import AuthClient from '@/HttpClients/Auth/AuthClient';
import UserAuthResponse from '@/HttpClients/Auth/UserAuthResponse';
export default {
    namespaced: true,
    state: {
        isLoading: false,
        userAuthentication: {},
        serviceUnavailable: false
    },
    getters: {
        serviceUnavailable: (state) => state.serviceUnavailable,
        userAuthentication: (state) => state.userAuthentication,
        isLoading: (state) => state.isLoading
    },
    mutations: {
        setServiceUnavailable(state, value) {
            state.serviceUnavailable = value;
        },
        setUserAuthentication(state, auth) {
            state.userAuthentication = auth;
        },
        setIsLoading(state, isLoading) {
            state.isLoading = isLoading;
        }
    },
    actions: {
        authenticate({ commit, getters }) {
            return __awaiter(this, void 0, void 0, function* () {
                try {
                    commit('setIsLoading', true);
                    let response = yield AuthClient.GetAuth();
                    if (response.status === 200) {
                        let authData = response.data;
                        commit('setUserAuthentication', authData);
                    }
                    else {
                        commit('setServiceUnavailable', false);
                        commit('setUserAuthentication', new UserAuthResponse());
                    }
                }
                catch (error) {
                    commit('setServiceUnavailable', false);
                    commit('setUserAuthentication', new UserAuthResponse());
                }
                finally {
                    commit('setIsLoading', false);
                }
            });
        }
    }
};
//# sourceMappingURL=AuthModule.js.map