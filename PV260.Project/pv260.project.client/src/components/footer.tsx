import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Separator } from '@/components/ui/separator';

const Footer = () => {
  return (
    <div className="flex flex-col">
      <footer>
        <div className="max-w-screen-xl mx-auto">
          <div className="py-12 flex flex-col sm:flex-row items-start justify-between gap-x-8 gap-y-10 px-6 xl:px-0">
            <div className="my-auto">
              {/* Logo */}

              <img src="https://shadcnblocks.com/images/block/logos/shadcnblockscom-icon.svg" alt="Shadcnblocks Logo"
                   width="40" height="40" />
            </div>
            {/* Subscribe Newsletter */}
            <div className="max-w-xs w-full">
              <h6 className="font-semibold">Stay up to date</h6>
              <form className="mt-6 flex items-center gap-2">
                <Input type="email" placeholder="Enter your email" />
                <Button>Subscribe</Button>
              </form>
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
