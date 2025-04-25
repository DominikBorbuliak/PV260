import { ColumnDef } from '@tanstack/react-table';
import { ExampleType } from './ExampleType';
import GenericTableColumnHeader from '../generic-table/GenericTableColumnHeader';

export const exampleTableColumns: ColumnDef<ExampleType>[] = [
  {
    accessorKey: 'id',
    accessorFn: (row) => row.id,
    header: ({ column }) => {
      <GenericTableColumnHeader column={column} title="Id" />;
    },
    enableHiding: false,
  },
  {
    accessorKey: 'name',
    accessorFn: (row) => row.name,
    meta: 'Name', // Should be same as header name to propagate to column dropdown
    header: ({ column }) => {
      <GenericTableColumnHeader column={column} title="Name" />;
    },
    cell: ({ row }) => <div>{row.getValue('name')}</div>,
  },
  {
    accessorKey: 'role',
    accessorFn: (row) => row.role,
    meta: 'Role',
    header: ({ column }) => {
      <GenericTableColumnHeader column={column} title="Role" />;
    },
    cell: ({ row }) => <div>{row.getValue('role')}</div>,
    // Use filterFn only if you want to use faceted filters
    filterFn: (row, id, value) => {
      return (value as string[]).includes(row.getValue(id));
    },
  },
];
