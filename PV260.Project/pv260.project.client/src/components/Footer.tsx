import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '@/services/api/base';
import { useUser } from '@/contexts/UserContext';

const Footer = () => {
  const queryClient = useQueryClient();

  const user = useUser();

  const toggleSubscribeMutation = useMutation({
    mutationFn: async () => {
      await apiClient.user.toggleIsSubscribed();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ['pingauth'],
      });
    },
  });

  const handleToggleSubscribe = () => {
    toggleSubscribeMutation.mutate();
  };

  return (
    <div className="flex flex-col">
      <footer>
        <div className="max-w-screen-xl mx-auto">
          <div className="py-12 flex flex-col sm:flex-row items-start justify-between gap-x-8 gap-y-10 px-6 xl:px-0">
            <div className="my-auto">
              {/* Logo */}

              <img
                src="https://www.em.muni.cz/cache-thumbs/logo_muni_web-1580x790-2008259181.jpg"
                alt="Logo"
                width="120"
                height="120"
              />
            </div>
            {/* Subscribe Newsletter */}
            <div className="max-w-xs w-full">
              <h6 className="font-semibold">Stay up to date</h6>
              <Button
                className="mt-2"
                type="submit"
                onClick={handleToggleSubscribe}
                disabled={toggleSubscribeMutation.isPending}
              >
                {user?.isSubscribed ? 'Unsubscribe' : 'Subscribe'}
              </Button>
            </div>
          </div>
          <Separator />
          <div className="py-8 flex flex-col-reverse sm:flex-row items-center justify-between gap-x-2 gap-y-5 px-6 xl:px-0">
            {/* Copyright */}
            <span className="text-muted-foreground">
              &copy; {new Date().getFullYear()} . All rights reserved.
            </span>
          </div>
        </div>
      </footer>
    </div>
  );
};
export default Footer;
