import { FC } from 'react';
import { useUser } from '../contexts/UserContext';
import { UserDto } from '@/_generatedClient';

interface AuthorizedUserProps {
  value: keyof UserDto;
}

export const AuthorizedUser: FC<AuthorizedUserProps> = ({ value }) => {
  const user = useUser();

  if (!user) return null;

  return <>{user[value]}</>;
};
