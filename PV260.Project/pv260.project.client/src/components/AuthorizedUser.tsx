import { FC } from 'react';
import { useUser } from '../contexts/UserContext';
import { User } from '../types/User';

interface AuthorizedUserProps {
  value: keyof User;
}

export const AuthorizedUser: FC<AuthorizedUserProps> = ({ value }) => {
  const user = useUser();

  if (!user) return null;

  return <>{user[value]}</>;
};
