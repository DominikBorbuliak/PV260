import { AuthorizedView } from '@/components/AuthorizedView';
import { Button } from '@/components/ui/button';
import { FC } from 'react';
import { Navbar } from '@/components/Navbar.tsx';
import { DatePicker } from 'antd';
import Footer from '@/components/Footer.tsx';
import { ArkHoldingsTableDiffs } from '@/components/ArkHoldingsTableDiffs/ArkHoldingsTableDiffs.tsx';
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '@/services/api/base.ts';
import { ApiError, ArkHoldingsDiffDto } from '@/_generatedClient';

const HomePage: FC = () => {

  const {
    data: arkHoldings,
    isLoading,
  } = useQuery<ArkHoldingsDiffDto[], ApiError>({
    queryKey: ['GET', 'ArkHoldingsDiff'],
    queryFn: async () => {
      return await apiClient.arkHoldings.getArkHoldingsDiff();
    },
  });

  return (
    <AuthorizedView>
      <div className="container mx-auto">
        <Navbar />
        <div className="mt-10 p-5 rounded-xl">
          <div className="grow flex justify-end py-6 ">
            <div className="mx-2">
              <DatePicker />
            </div>
            <Button className="mx-2">Update Data</Button>
          </div>
          {isLoading ? <p>Loading...</p> : (<ArkHoldingsTableDiffs data={arkHoldings ? arkHoldings : []} />)}
        </div>
        <Footer />
      </div>
    </AuthorizedView>
  );
};

export default HomePage;
