import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Separator } from '@/components/ui/separator';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import z from 'zod';
import { useMutation } from '@tanstack/react-query';
import { Form, FormMessage } from './ui/form';
import { FormField } from '@/components/form/Field.tsx';
import { subscribe } from '@/services/api/subscribe.ts';

const subscribeFormSchema = z.object({
  email: z.string().refine((value) => (value.match(/.+@.+/)?.length ?? 0) > 0, {
    message: 'Email must contain \'@\'.',
  }),
});

type SubscribeFormType = z.infer<typeof subscribeFormSchema>;

const Footer = () => {

  const subscribeMutation = useMutation({
    mutationFn: async ({ email }: SubscribeFormType) =>
      subscribe(email),
  });

  const form = useForm({
    resolver: zodResolver(subscribeFormSchema),
    defaultValues: {
      email: '',
    },
  });

  const onSubmit = async (data: SubscribeFormType) => {
    form.clearErrors('root');

    await subscribeMutation.mutateAsync(data, {
      onError: () =>
        form.setError('root', { message: 'Invalid email.' }),
      onSuccess: () => {
        form.reset();
        form.setValue('email', '');
      },
    });
  };

  return (
    <div className="flex flex-col">
      <footer>
        <div className="max-w-screen-xl mx-auto">
          <div className="py-12 flex flex-col sm:flex-row items-start justify-between gap-x-8 gap-y-10 px-6 xl:px-0">
            <div className="my-auto">
              {/* Logo */}

              <img src="https://www.em.muni.cz/cache-thumbs/logo_muni_web-1580x790-2008259181.jpg" alt="Logo"
                   width="120" height="120" />
            </div>
            {/* Subscribe Newsletter */}
            <div className="max-w-xs w-full">
              <h6 className="font-semibold">Stay up to date</h6>
              <Form {...form} >
                {/* eslint-disable-next-line @typescript-eslint/no-misused-promises */}
                <form onSubmit={form.handleSubmit(onSubmit)}>
                  <FormField
                    name="email"
                    renderControl={(field) => (
                      <Input placeholder="Email Address" type="email" {...field} />
                    )}
                  />
                  {form.formState.errors.root && (
                    <FormMessage className="text-xs">
                      {form.formState.errors.root.message}
                    </FormMessage>
                  )}
                  <Button className='mt-2' type='submit'>Subscribe</Button>
                </form>
              </Form>
            </div>
          </div>
          <Separator />
          <div
            className="py-8 flex flex-col-reverse sm:flex-row items-center justify-between gap-x-2 gap-y-5 px-6 xl:px-0">
            {/* Copyright */}
            <span className="text-muted-foreground">
              &copy; {new Date().getFullYear()}{' '}

              . All rights reserved.
            </span>

          </div>
        </div>
      </footer>
    </div>
  );
};
export default Footer;
