import { User } from '@/types/User';
import { createContext, useContext } from 'react';

export const UserContext = createContext<User | null>(null);

export const useUser = (): User | null => {
  return useContext(UserContext);
};
