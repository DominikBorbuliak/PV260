import { ColumnDef } from '@tanstack/react-table';
import GenericTableColumnHeader from '../generic-table/GenericTableColumnHeader';
import { HoldingChangeDto } from '@/_generatedClient';

export const reportDiffTableColumns: ColumnDef<HoldingChangeDto>[] = [
  {
    accessorKey: 'company',
    accessorFn: (row) => row.company,
    meta: 'Company',
    header: ({ column }) => (
      <GenericTableColumnHeader column={column} title="Company" />
    ),
    cell: ({ row }) => <div>{row.getValue('company')}</div>,
  },
  {
    accessorKey: 'ticker',
    accessorFn: (row) => row.ticker,
    meta: 'Ticker',
    header: ({ column }) => (
      <GenericTableColumnHeader column={column} title="Ticker" />
    ),
    cell: ({ row }) => <div>{row.getValue('ticker')}</div>,
  },
  {
    accessorKey: 'shares',
    accessorFn: (row) => row.newShares,
    meta: 'Shares',
    header: ({ column }) => (
      <GenericTableColumnHeader column={column} title="Shares" />
    ),
    cell: ({ row }) => (
      <div
        className={
          (row.original.newShares ?? 0) - (row.original.oldShares ?? 0) > 0
            ? 'text-green-500'
            : 'text-red-500'
        }
      >
        {row.getValue('shares')}
      </div>
    ),
  },
  {
    accessorKey: 'changeType',
    accessorFn: (row) => row.changeType,
    meta: 'Change',
    header: ({ column }) => (
      <GenericTableColumnHeader column={column} title="Change" />
    ),
    cell: ({ row }) => <div>{row.getValue('changeType')}</div>,
  },
];
