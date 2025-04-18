import { AuthorizeView } from '@/components/AuthorizedView';
import { FC } from 'react';

const HomePage: FC = () => {
  return (
    <AuthorizeView>
      <h1>Hello world!</h1>
    </AuthorizeView>
  );
};

export default HomePage;
