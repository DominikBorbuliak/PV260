import { ColumnDef } from '@tanstack/react-table';
import { Button } from '@/components/ui/button.tsx';
import { ArrowUpDown } from 'lucide-react';
import { ArkHolding } from '@/components/ArkHoldingsTableDiffs/ArkHoldingsTableDiffs.tsx';

export const getColumns = (date1: string, date2: string): ColumnDef<ArkHolding>[] => [
  {
    accessorKey: 'date',
    header: 'Date',
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('date')}</div>,
  },
  {
    accessorKey: 'date2',
    header: 'Date2',
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('date2')}</div>,
  },
  {
    accessorKey: 'fund',
    header: 'Fund',
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('fund')}</div>,
  },
  {
    accessorKey: 'company',
    header: ({ column }) => (
      <Button
        variant="ghost"
        onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
      >
        {`Company`}
        <ArrowUpDown className="ml-2 h-4 w-4" />
      </Button>
    ),
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('company')}</div>,
  },
  {
    accessorKey: 'ticker',
    header: 'Ticker',
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('ticker')}</div>,
  },
  {
    accessorKey: 'cusip',
    header: 'Cusip',
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('cusip')}</div>,
  },
  {
    accessorKey: 'shares',
    header: `Shares (${date1})`,
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('shares')}</div>,
  },
  {
    accessorKey: 'shares2',
    header: `Shares (${date2})`,
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('shares2')}</div>,
  },
  {
    accessorKey: 'sharesDiff',
    header: 'Shares Diff',
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('sharesDiff')}</div>,
  },
  {
    accessorKey: 'marketValue',
    header: `Market Value (${date1})`,
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('marketValue')}</div>,
  },
  {
    accessorKey: 'marketValue2',
    header: `Market Value (${date2})`,
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('marketValue2')}</div>,
  },
  {
    accessorKey: 'marketValueDiff',
    header: 'Market Value Diff',
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('marketValueDiff')}</div>,
  },
  {
    accessorKey: 'weight',
    header: ({ column }) => (
      <Button
        variant="ghost"
        onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
      >
        {`weight % (${date1})`}
        <ArrowUpDown className="ml-2 h-4 w-4" />
      </Button>
    ),
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('weight')}</div>,
  },
  {
    accessorKey: 'weight2',
    header: ({ column }) => (
      <Button
        variant="ghost"
        onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
      >
        {`weight % (${date2})`}
        <ArrowUpDown className="ml-2 h-4 w-4" />
      </Button>
    ),
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('weight2')}</div>,
  },
  {
    accessorKey: 'weightDiff',
    header: ({ column }) => (
      <Button
        variant="ghost"
        onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
      >
        Weight diff (%)
        <ArrowUpDown className="ml-2 h-4 w-4" />
      </Button>
    ),
    cell: ({ row }) => <div className="flex justify-center">{row.getValue('weightDiff')}</div>,
  },
];
