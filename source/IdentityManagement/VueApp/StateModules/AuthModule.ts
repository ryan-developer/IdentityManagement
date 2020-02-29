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
        serviceUnavailable: (state: any) => state.serviceUnavailable,
        userAuthentication: (state: any) => state.userAuthentication,
        isLoading: (state: any) => state.isLoading
    },
    mutations: {
        setServiceUnavailable(state: any, value: boolean) {
            state.serviceUnavailable = value;
        },
        setUserAuthentication(state: any, auth: UserAuthResponse) {
            state.userAuthentication = auth;
        },
        setIsLoading(state: any, isLoading: boolean) {
            state.isLoading = isLoading;
        }
    },
    actions: {
        async authenticate({ commit, getters }: any) {
            try {
                commit('setIsLoading', true);
                let response = await AuthClient.GetAuth();
                if (response.status === 200) {
                    let authData = response.data;
                    commit('setUserAuthentication', authData);
                } else {
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
        }
    }
}