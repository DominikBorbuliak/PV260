import { AuthorizedUser } from '@/components/AuthorizedUser';
import { AuthorizedView } from '@/components/AuthorizedView';
import { Button } from '@/components/ui/button';
import { logOut } from '@/services/api/auth';
import { useMutation } from '@tanstack/react-query';
import { FC } from 'react';
import { useNavigate } from 'react-router-dom';

const HomePage: FC = () => {
  const navigate = useNavigate();
  const logoutMutation = useMutation({
    mutationFn: logOut,
  });

  return (
    <>
      <AuthorizedView>
        <h1>
          Hello world <AuthorizedUser value="email" />!
        </h1>
        <Button
          type="button"
          onClick={() =>
            void logoutMutation.mutateAsync(undefined, {
              onSuccess: () => void navigate('/login'),
            })
          }
        >
          Log out
        </Button>
      </AuthorizedView>
    </>
  );
};

export default HomePage;
