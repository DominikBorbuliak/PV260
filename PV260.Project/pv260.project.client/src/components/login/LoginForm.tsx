import { useForm } from 'react-hook-form';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '../ui/form';
import { Input } from '../ui/input';
import { zodResolver } from '@hookform/resolvers/zod';
import z from 'zod';
import { Button } from '../ui/button';
import { NavLink, useNavigate } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { login } from '@/services/api/auth';
import { Separator } from '@radix-ui/react-separator';

const loginFormSchema = z.object({
  email: z.string().refine((value) => (value.match(/.+@.+/)?.length ?? 0) > 0, {
    message: "Email must contain '@'.",
  }),
  password: z.string().min(6, 'Password must be at least 6 characters.'),
});

type LoginFormType = z.infer<typeof loginFormSchema>;

export const LoginForm = () => {
  const navigate = useNavigate();
  const loginMutation = useMutation({
    mutationFn: async ({ email, password }: LoginFormType) =>
      login(email, password),
  });

  const form = useForm({
    resolver: zodResolver(loginFormSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  const onSubmit = async (data: LoginFormType) => {
    form.clearErrors('root');

    await loginMutation.mutateAsync(data, {
      onError: () =>
        form.setError('root', { message: 'Invalid username or password.' }),
      onSuccess: () => void navigate('/'),
    });
  };

  return (
    <div className="w-56">
      <Form {...form}>
        {/* eslint-disable-next-line @typescript-eslint/no-misused-promises */}
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <FormField
            control={form.control}
            name="email"
            render={({ field: { ref: _, ...field } }) => (
              <FormItem>
                <FormLabel>Email</FormLabel>
                <FormControl>
                  <Input placeholder="Email Address" type="email" {...field} />
                </FormControl>
                <FormMessage className="text-xs" />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="password"
            render={({ field: { ref: _, ...field } }) => (
              <FormItem className="mt-2">
                <FormLabel>Password</FormLabel>
                <FormControl>
                  <Input placeholder="Password" type="password" {...field} />
                </FormControl>
                <FormMessage className="text-xs" />
              </FormItem>
            )}
          />

          {form.formState.errors.root && (
            <FormMessage className="text-xs">
              {form.formState.errors.root.message}
            </FormMessage>
          )}

          <Button type="submit" className="my-2">
            Log in
          </Button>
        </form>
      </Form>
      <Separator className="bg-secondary h-[1px] my-2" />
      <span className="text-xs text-center w-full block">
        Don't have an account?{' '}
        <NavLink to={'/register'} className="font-semibold">
          Register.
        </NavLink>
      </span>
    </div>
  );
};
