import LoginForm from "../components/auth/login-form"

const Login = () => {
    return (
        <div id="login-root">
            <section className="w-full">
                <div className="flex-row flex h-2/4">
                    <div className='basis-1/3 content-center flex items-center justify-start'>  
                        <LoginForm />    
                    </div>
                    <div className="basis-2/3 flex items-center justify-start bg-contain bg-[url('/login.svg')]">
                    </div>
                </div>            
            </section>
        </div>
    )
}

export default Login