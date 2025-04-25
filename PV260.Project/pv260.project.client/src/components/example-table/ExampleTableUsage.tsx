import { Card } from '../ui/card';
import { exampleData } from './ExampleData';
import ExampleTable from './ExampleTable';

const ExampleTableUsage = () => {
  // Normally you would fetch data via useQuery and use ?? []

  return (
    <div>
      <Card className="w-full p-4">
        <ExampleTable tableItems={exampleData} areTableItemsLoading={false} />
      </Card>
    </div>
  );
};

export default ExampleTableUsage;
