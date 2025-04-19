import { useForm } from 'react-hook-form';
import { Form, FormMessage } from '../ui/form';
import { Input } from '../ui/input';
import { zodResolver } from '@hookform/resolvers/zod';
import z from 'zod';
import { Button } from '../ui/button';
import { NavLink, useNavigate } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { register } from '@/services/api/auth';
import { Separator } from '@radix-ui/react-separator';
import { FormField } from '../form/Field';

const registerFormSchema = z.object({
  email: z.string().refine((value) => value.match(/.+@.+/), {
    message: "Email must contain '@'.",
  }),
  password: z
    .string()
    .min(6, 'Password must be at least 6 characters.')
    .refine((password) => password.match(/[@#$%*!?;.]/), {
      message: 'Password must contain at least one special character.',
    })
    .refine((password) => password.match(/[0-9]/), {
      message: 'Password must contain at least one digit.',
    })
    .refine((password) => password.match(/[A-Z]/), {
      message: 'Password must include at least one uppercase letter.',
    }),
});

type RegisterFormType = z.infer<typeof registerFormSchema>;

export const RegisterForm = () => {
  const navigate = useNavigate();
  const registerMutation = useMutation({
    mutationFn: async ({ email, password }: RegisterFormType) =>
      register(email, password),
  });

  const form = useForm({
    resolver: zodResolver(registerFormSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  const onSubmit = async (data: RegisterFormType) => {
    form.clearErrors('root');

    await registerMutation.mutateAsync(data, {
      onError: () =>
        form.setError('root', {
          message:
            'Something went wrong during registration. Please try again.',
        }),
      onSuccess: () => void navigate('/login'),
    });
  };

  return (
    <div className="w-56">
      <Form {...form}>
        {/* eslint-disable-next-line @typescript-eslint/no-misused-promises */}
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <FormField
            name="email"
            renderControl={(field) => (
              <Input placeholder="Email Address" type="email" {...field} />
            )}
          />
          <FormField
            name="password"
            renderControl={(field) => (
              <Input placeholder="Password" type="password" {...field} />
            )}
          />

          {form.formState.errors.root && (
            <FormMessage className="text-xs">
              {form.formState.errors.root.message}
            </FormMessage>
          )}

          <Button
            type="submit"
            className="my-2"
            disabled={registerMutation.isPending}
          >
            Register
          </Button>
        </form>
      </Form>
      <Separator className="bg-secondary h-[1px] my-2" />
      <span className="text-xs text-center w-full block">
        Already have an account?{' '}
        <NavLink to={'/login'} className="font-semibold">
          Log in.
        </NavLink>
      </span>
    </div>
  );
};
