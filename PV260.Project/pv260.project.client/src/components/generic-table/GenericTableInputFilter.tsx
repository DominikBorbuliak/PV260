import { Column } from '@tanstack/react-table';
import { Input } from '../ui/input';

type GenericTableInputFilterProps<T, U> = {
  column?: Column<T, U>;
  placeholder?: string;
};

const GenericTableInputFilter = <T, U>({
  column,
  placeholder,
}: GenericTableInputFilterProps<T, U>) => {
  return (
    <Input
      placeholder={placeholder}
      value={(column?.getFilterValue() as string) ?? ''}
      onChange={(event) => column?.setFilterValue(event.target.value)}
      className="placeholder:text-foreground space-x-3 h-8 border-dashed inline-flex items-center justify-center whitespace-nowrap font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 rounded-md px-3 text-xs border border-input bg-background shadow-sm"
    />
  );
};

export default GenericTableInputFilter;
