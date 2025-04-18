import { LoginForm } from '@/components/login/LoginForm';
import { FC } from 'react';

const LoginPage: FC = () => {
  return (
    <main className="flex items-center justify-center pt-8">
      <LoginForm />
    </main>
  );
};

export default LoginPage;
