import CardWrapper from "./card-wrapper";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { LoginSchema } from "../../schema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { Input } from "../ui/input";
import { Button } from "../ui/button";
import { z } from "zod";
//import { useFormStatus } from "react-dom";
import { useDispatch } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { useRef, useState, useEffect } from 'react'
import { setCredentials } from '../../features/auth/auth-slice'
import { useLoginMutation, useTokenMutation } from '../../features/auth/auth-аpi-slice'

const LoginForm = () => {
  const grantType = 'authorization_code';
  const clientId = 'FB2BC825BDEBD22D10FAACA79C38E881C8C919735B9DC653F8885EB234B5DEBBFB49E926E05F3E7AC745BEE9618B9572';

  const dispatch = useDispatch()
  const navigate = useNavigate()
  const [errMsg, setErrMsg] = useState('')
  const errRef = useRef()
  const [loading, setLoading] = useState(false);
  const form = useForm({
    resolver: zodResolver(LoginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const [login] = useLoginMutation()
  const [token] = useTokenMutation()

  const onSubmit = async (data) => {
    setErrMsg('');
    setLoading(true);

    const email = data.email
    const password = data.password

    try {
      const auth = await login({ email, password, clientId, grantType }).unwrap()

      const userData = await token({ authorizationCode: auth.authorizationCode }).unwrap()

      dispatch(setCredentials({ ...userData, email }))
      navigate('/home')
    } catch (err) {
      if (!err?.originalStatus) {
        // isLoading: true until timeout occurs
        setErrMsg('Oops.. Something went wrong! Please try again later.');
      } else if (err.originalStatus === 400) {
        setErrMsg('Wrong Credentials!');
      } else if (err.originalStatus === 401) {
        setErrMsg('Unauthorized');
      } else {
        setErrMsg('Login ttemp failed..');
      }
      errRef.current.focus();
    }

    setLoading(false)
  };

  //const { pending } = useFormStatus();

  return (
    <CardWrapper
      label="Login to your account"
      title="Welcome to Reservico!"
      backButtonHref="/auth/register"
      backButtonLabel="Don't have an account? Register here."
    >
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
          <div className="space-y-4">
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem className="text-left">
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input
                      {...field}
                      type="email"
                      placeholder="example@domain.com"
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem className="text-left">
                  <FormLabel>Password</FormLabel>
                  <FormControl>
                    <Input {...field} type="password" placeholder="******" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
          <p ref={errRef} className="errmsg text-red-500" aria-live="assertive">{errMsg}</p>
          <Button type="submit" className="w-full" >
            {loading ? "Loading..." : "Login"}
          </Button>
        </form>
      </Form>
    </CardWrapper>
  );
};

export default LoginForm;