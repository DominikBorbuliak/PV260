import { Navbar } from '@/components/navbar.tsx';
import Footer from '@/components/footer.tsx';
import { Button } from '@/components/ui/button.tsx';
import { DatePicker, DatePickerProps } from 'antd';

export default function Homepage() {

  const onChange: DatePickerProps['onChange'] = (date, dateString) => {
    console.log(date, dateString);
  };

  return (
    <div className="container mx-auto">
      <Navbar />
      <div className="mt-10">
        <div className="grow bg-muted flex justify-end py-6 rounded-xl">
          <div className="mx-2">
            {/* eslint-disable-next-line @typescript-eslint/no-unsafe-assignment */}
            <DatePicker onChange={onChange} />
          </div>
          <Button className="mx-2">Submit </Button>
          <Button variant="outline" className="mx-2">Update Data </Button>
          <div className="h-[600px]" />
        </div>
      </div>
      <Footer />
    </div>
  );
};
