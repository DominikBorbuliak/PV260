import { Table } from '@tanstack/react-table';
import { Input } from '../ui/input';
import { PropsWithChildren } from 'react';
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover';
import { Button } from '../ui/button';
import { Filter } from 'lucide-react';
import { ScrollArea } from '../ui/scroll-area';

export type GenericTableSearchProps<T> = PropsWithChildren & {
  table: Table<T>;
  resetFilters?: () => void;
};

const GenericTableSearch = <T,>({
  table,
  children,
  resetFilters,
}: GenericTableSearchProps<T>) => {
  const filters = table.getState().columnFilters.filter((cf) => {
    // Count switch filter only if value is true
    if (typeof cf.value === 'boolean') {
      return cf.value;
    }

    // Count array filter only if there is at least one defined value
    if (Array.isArray(cf.value)) {
      return cf.value?.filter((v) => v).length > 0;
    }

    return true;
  }, 0);

  return (
    <div className="flex flex-col md:flex-row gap-2">
      <div>
        <Input
          placeholder="Search"
          onChange={(event) => table.setGlobalFilter(event.target.value)}
          value={table.getState().globalFilter as string}
          className="flex-1 placeholder:text-foreground space-x-3 h-8 border-dashed inline-flex items-center justify-center whitespace-nowrap font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 rounded-md px-3 text-xs border border-input bg-background shadow-sm"
        />
      </div>

      {children && (
        <Popover>
          <PopoverTrigger asChild>
            <Button className="p-0 h-8 w-8 relative">
              <Filter className="h-4 w-4" />

              {filters.length > 0 && (
                <div className="absolute -top-2 -right-2 bg-destructive text-white rounded-full text-xs w-4 h-4 flex items-center justify-center">
                  {filters.length}
                </div>
              )}
            </Button>
          </PopoverTrigger>
          <PopoverContent
            side="right"
            align="start"
            className="flex w-[280px] p-1"
          >
            <ScrollArea className="max-h-screen w-full" type="always">
              <div className="flex flex-col gap-3 p-3">
                {children}
                {resetFilters && (
                  <Button onClick={resetFilters}>Clear filters</Button>
                )}
              </div>
            </ScrollArea>
          </PopoverContent>
        </Popover>
      )}
    </div>
  );
};

export default GenericTableSearch;
