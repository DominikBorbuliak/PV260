import { HtmlHTMLAttributes, ReactNode } from 'react';
import { ControllerRenderProps, useFormContext } from 'react-hook-form';
import {
  FormField as ShadFormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
} from '../ui/form';

export interface FormFieldProps extends HtmlHTMLAttributes<HTMLElement> {
  name: string;
  label?: string;
  renderControl: (control: Partial<ControllerRenderProps>) => ReactNode;
}

export const FormField = ({
  name,
  label,
  className,
  renderControl,
}: FormFieldProps) => {
  const form = useFormContext();

  return (
    <ShadFormField
      control={form.control}
      name={name}
      render={({ field: { ref: _, ...field } }) => (
        <FormItem className={className}>
          <FormLabel>{label}</FormLabel>
          <FormControl>{renderControl(field)}</FormControl>
          <FormMessage className="text-xs" />
        </FormItem>
      )}
    />
  );
};
