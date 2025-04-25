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
import GenericTableSearch from '../generic-table/GenericTableSearch';
import GenericTableInputFilter from '../generic-table/GenericTableInputFilter';
import GenericTableExportDropdown from '../generic-table/GenericTableExportDropdown';
import GenericTableColumnDropdown from '../generic-table/GenericTableColumnDropdown';
import GenericTable from '../generic-table/GenericTable';
import GenericTablePagination from '../generic-table/GenericTablePagination';
import { reportDiffTableColumns } from './ReportDiffTableColumns';
import { HoldingChangeDto } from '@/_generatedClient';

type ReportTableProps = {
  tableItems: HoldingChangeDto[];
  areTableItemsLoading: boolean; // attribute from useQuery
};

const ReportDiffTable = ({
  tableItems,
  areTableItemsLoading,
}: ReportTableProps) => {
  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({
    id: false,
  });
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const table = useReactTable({
    data: tableItems,
    columns: reportDiffTableColumns,
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
          <GenericTableInputFilter
            column={table.getColumn('ticker')}
            placeholder="Ticker"
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

export default ReportDiffTable;
