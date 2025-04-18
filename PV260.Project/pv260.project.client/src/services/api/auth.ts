import { User } from '@/types/User';
import { apiClient } from './base';

export const login = async (email: string, password: string) => {
  await apiClient.pv260ProjectServer.postApiUserLogin({
    requestBody: {
      email,
      password,
    },
    useSessionCookies: true,
  });
};

export const register = async (email: string, password: string) => {
  await apiClient.pv260ProjectServer.postApiUserRegister({
    requestBody: {
      email,
      password,
    },
  });
};

export const logOut = async () => {
  await apiClient.user.logout();
};

export const pingAuth = async (): Promise<User> => {
  return (await apiClient.user.pingauth()) as User;
};
