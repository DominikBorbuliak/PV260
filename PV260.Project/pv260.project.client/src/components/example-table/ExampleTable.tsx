import { useFacetedFilterOptions } from '@/hooks/generic-table-hooks';
import {
  getCoreRowModel,
  getFacetedRowModel,
  getFacetedUniqueValues,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  PaginationState,
  useReactTable,
  VisibilityState,
} from '@tanstack/react-table';
import { useState } from 'react';
import { exampleTableColumns } from './ExampleTableColumns';
import GenericTableSearch from '../generic-table/GenericTableSearch';
import GenericTableFacetedFilter from '../generic-table/GenericTableFacetedFilter';
import { ExampleType } from './ExampleType';
import GenericTableInputFilter from '../generic-table/GenericTableInputFilter';
import GenericTableExportDropdown from '../generic-table/GenericTableExportDropdown';
import GenericTableColumnDropdown from '../generic-table/GenericTableColumnDropdown';
import GenericTable from '../generic-table/GenericTable';
import GenericTablePagination from '../generic-table/GenericTablePagination';

type ExampleTableProps = {
  tableItems: ExampleType[];
  areTableItemsLoading: boolean; // attribute from useQuery
};

const ExampleTable = ({
  tableItems,
  areTableItemsLoading,
}: ExampleTableProps) => {
  const { getStringFacetedFilterOptions } = useFacetedFilterOptions();

  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({
    id: false,
  });
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const table = useReactTable({
    data: tableItems,
    columns: exampleTableColumns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFacetedRowModel: getFacetedRowModel(),
    getFacetedUniqueValues: getFacetedUniqueValues(),
    onPaginationChange: setPagination,
    onColumnVisibilityChange: setColumnVisibility,
    state: {
      columnVisibility,
      pagination,
    },
  });

  return (
    <div className="flex flex-col gap-3">
      <div className="flex items-end space-x-4 justify-between">
        <GenericTableSearch
          table={table}
          resetFilters={() => table.resetColumnFilters()}
        >
          <GenericTableFacetedFilter
            column={table.getColumn('role')}
            title="Role"
            options={getStringFacetedFilterOptions(
              tableItems.map((i) => i.role)
            )}
          />
          <GenericTableInputFilter
            column={table.getColumn('name')}
            placeholder="Name"
          />
        </GenericTableSearch>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
          <GenericTableExportDropdown
            table={table}
            exportFileName="example-data"
          />
          <GenericTableColumnDropdown table={table} />
        </div>
      </div>

      <GenericTable isLoading={areTableItemsLoading} table={table} />
      <GenericTablePagination table={table} pageSizes={[10, 20, 30, 40, 50]} />
    </div>
  );
};

export default ExampleTable;
