import { AuthorizedUser } from '@/components/AuthorizedUser';
import { AuthorizedView } from '@/components/AuthorizedView';
import { Button } from '@/components/ui/button';
import { logOut } from '@/services/api/auth';
import { useMutation } from '@tanstack/react-query';
import { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import { Navbar } from '@/components/navbar.tsx';
import { DatePicker } from 'antd';
import Footer from '@/components/footer.tsx';

const HomePage: FC = () => {
  const navigate = useNavigate();
  const logoutMutation = useMutation({
    mutationFn: logOut,
  });

  return (
    <AuthorizedView>
      <div className="container mx-auto">
        <Navbar />

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
        <div className="mt-10">
          <div className="grow bg-muted flex justify-end py-6 rounded-xl">
            <div className="mx-2">
              <DatePicker />
            </div>
            <Button className="mx-2">Update Data </Button>
            <div className="h-[600px]" />
          </div>
        </div>
        <Footer />
      </div>
    </AuthorizedView>
  );
};

export default HomePage;
