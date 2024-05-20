import {
    Card,
    CardContent,
    CardHeader,
    CardFooter,
  } from "@/components/ui/card";
import AuthHeader from "./auth-header";
import BackButton from "./back-button";

const CardWrapper = ({label, title, backButtonHref, backButtonLabel, children}) => {
  return (
    <Card className="h-screen shadow-md items-center justify-center flex max-h-[60%]">
      <div className="items-center justify-center">
        <CardHeader>
            <AuthHeader label={label} title={title} />
        </CardHeader>
        <CardContent>          
            {children}          
        </CardContent>
        <CardFooter>
            <BackButton label={backButtonLabel} href={backButtonHref} />
        </CardFooter>
      </div>
    </Card>
  )
}

export default CardWrapper;
