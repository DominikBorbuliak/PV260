import { UserDto } from '@/_generatedClient';
import { createContext, useContext } from 'react';

export const UserContext = createContext<UserDto | null>(null);

export const useUser = (): UserDto | null => {
  return useContext(UserContext);
};
