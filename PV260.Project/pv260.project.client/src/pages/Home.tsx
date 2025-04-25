import { AuthorizedView } from '@/components/AuthorizedView';
import { Button } from '@/components/ui/button';
import { FC } from 'react';
import { Navbar } from '@/components/Navbar.tsx';
import { DatePicker } from 'antd';
import Footer from '@/components/Footer.tsx';
import { Card } from '@/components/ui/card';
import ReportDiffTable from '@/components/reports-table/ReportDiffTable';
import { apiClient } from '@/services/api/base';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { ApiError, HoldingChangeDto } from '@/_generatedClient';
import { format } from 'date-fns';
import { toast } from 'sonner';

const HomePage: FC = () => {
  const queryClient = useQueryClient();
  const currentDateTime = format(new Date(), 'yyyy-MM-dd HH:mm:ss');

  const {
    data: reportData,
    isLoading,
    isError,
  } = useQuery<Array<HoldingChangeDto>, ApiError>({
    queryKey: ['reportDiff'],
    queryFn: async () => {
      return await apiClient.report.reportDiff({ date: currentDateTime });
    },
  });

  const { mutate: generateReport, isPending: isGenerating } = useMutation({
    mutationFn: async () => {
      await apiClient.report.generateReport();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ['reportDiff'],
      });

      toast.success('New report generated successfully!');
    },
    onError: () => {
      toast.error('Failed to generate report! Try again later.');
    },
  });

  return (
    <AuthorizedView>
      <div className="container mx-auto">
        <Navbar />
        <div className="mt-10">
          <div className="flex flex-col bg-muted py-6 px-4 rounded-xl shadow-md">
            <div className="flex justify-between items-center mb-6">
              <DatePicker />
              <Button
                className="ml-4 hover:cursor-pointer"
                onClick={() => generateReport()}
                disabled={isGenerating}
              >
                {isGenerating ? 'Generating...' : 'Generate Report'}
              </Button>
            </div>
            <div>
              <Card className="w-full p-6 shadow-sm">
                {isLoading ? (
                  <p>Loading...</p>
                ) : (
                  <ReportDiffTable
                    tableItems={reportData ?? []}
                    areTableItemsLoading={false}
                  />
                )}
                {isError && <p>Oops something went wrong</p>}
              </Card>
            </div>
          </div>
        </div>
        <Footer />
      </div>
    </AuthorizedView>
  );
};

export default HomePage;
