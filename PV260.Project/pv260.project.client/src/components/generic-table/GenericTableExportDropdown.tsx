import { Table as TableDef } from '@tanstack/react-table';
import { Button } from '../ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '../ui/dropdown-menu';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver';
import { FileDown } from 'lucide-react';

type GenericTableExportDropdownProps<T> = {
  table: TableDef<T>;
  exportFileName: string;
};

const GenericTableExportDropdown = <T,>({
  table,
  exportFileName,
}: GenericTableExportDropdownProps<T>) => {
  const exportTable = async (fileType: 'csv' | 'xlsx') => {
    const wb = new Workbook();
    const ws = wb.addWorksheet('Sheet 1');

    const lastHeaderGroup = table.getHeaderGroups().at(-1);

    if (!lastHeaderGroup) {
      return;
    }

    ws.columns = lastHeaderGroup.headers
      .filter((h) => h.column.getIsVisible())
      .map((header) => {
        return {
          header: header.column.columnDef.meta as string,
          key: header.id,
          width: 20,
        };
      });

    table.getFilteredRowModel().rows.forEach((row) => {
      const cells = row.getVisibleCells();
      const values = cells.map((cell) => cell.getValue() ?? '');

      ws.addRow(values);
    });

    ws.getRow(1).eachCell((cell) => {
      cell.font = { bold: true };
    });

    if (fileType === 'csv') {
      const buf = await wb.csv.writeBuffer();
      saveAs(new Blob([buf]), `${exportFileName}.${fileType}`);
    } else if (fileType === 'xlsx') {
      const buf = await wb.xlsx.writeBuffer();
      saveAs(new Blob([buf]), `${exportFileName}.${fileType}`);
    }
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="outline" size="sm" className="h-8 flex">
          <FileDown className="mr-2 h-4 w-4" />
          Export
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="w-[150px]">
        <DropdownMenuLabel>Export type</DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuItem onClick={() => void exportTable('xlsx')}>
          XLSX
        </DropdownMenuItem>
        <DropdownMenuItem onClick={() => void exportTable('csv')}>
          CSV
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export default GenericTableExportDropdown;
