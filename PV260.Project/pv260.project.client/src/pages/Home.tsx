import { AuthorizedView } from '@/components/AuthorizedView';
import { Button } from '@/components/ui/button';
import { FC } from 'react';
import { Navbar } from '@/components/Navbar.tsx';
import { DatePicker } from 'antd';
import Footer from '@/components/Footer.tsx';

const HomePage: FC = () => {
  return (
    <AuthorizedView>
      <div className="container mx-auto">
        <Navbar />
        <div className="mt-10">
          <div className="grow bg-muted flex justify-end py-6 rounded-xl">
            <div className="mx-2">
              <DatePicker />
            </div>
            <Button className="mx-2">Update Data</Button>
            <div className="h-[600px]" />
          </div>
        </div>
        <Footer />
      </div>
    </AuthorizedView>
  );
};

export default HomePage;
