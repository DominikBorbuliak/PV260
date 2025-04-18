import { RegisterForm } from '@/components/login/RegisterForm';
import { FC } from 'react';

const RegisterPage: FC = () => {
  return (
    <main className="flex items-center justify-center pt-8">
      <RegisterForm />
    </main>
  );
};

export default RegisterPage;
