import { ApiError } from '@/_generatedClient';
import { UserContext } from '@/contexts/UserContext';
import { apiClient } from '@/services/api/base';
import { User } from '@/types/User';
import { useQuery } from '@tanstack/react-query';
import { FC } from 'react';
import { Navigate } from 'react-router-dom';

interface AuthorizeViewProps {
  children: React.ReactNode;
}

export const AuthorizedView: FC<AuthorizeViewProps> = ({ children }) => {
  const {
    data: userData,
    isLoading,
    isError,
    error,
  } = useQuery<User, ApiError>({
    queryKey: ['pingauth'],
    queryFn: async () => {
      const res = (await apiClient.user.pingauth()) as User;
      return res;
    },
    retry: 1,
    retryDelay: 500,
  });

  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (isError) {
    if (error?.status === 401) {
      return <Navigate to="/login" replace />;
    }
    return <p>Unexpected error occurred.</p>;
  }

  return (
    <UserContext.Provider value={userData ?? null}>
      {children}
    </UserContext.Provider>
  );
};
