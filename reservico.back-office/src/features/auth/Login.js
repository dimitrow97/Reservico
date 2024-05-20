import { useRef, useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'

import { useDispatch } from 'react-redux'
import { setCredentials } from './authSlice'
import { useLoginMutation, useTokenMutation } from './authApiSlice'

const Login = () => {
    const grantType = 'authorization_code';
    const clientId = '1CE67DD0AE1CFC2FBB4883EFC4B5DFBAF58A939ACD93DC9A81A2DFE5747397BCE3260ADC70BF9EBE946C4B2C1460C67F';
    const emailRef = useRef()
    const errRef = useRef()
    const [email, setEmail] = useState('')
    const [password, setPwd] = useState('')
    const [errMsg, setErrMsg] = useState('')
    const navigate = useNavigate()

    const [login, { isLoading }] = useLoginMutation()
    const [token, { isLoading2 }] = useTokenMutation()
    const dispatch = useDispatch()

    useEffect(() => {
        emailRef.current.focus()
    }, [])

    useEffect(() => {
        setErrMsg('')
    }, [email, password])

    const handleSubmit = async (e) => {
        e.preventDefault()

        try {
            const auth = await login({ email, password, clientId, grantType }).unwrap()
            const userData = await token({ authorizationCode: auth.authorizationCode }).unwrap()
            dispatch(setCredentials({ ...userData, email }))
            setEmail('')
            setPwd('')
            navigate('/welcome')
        } catch (err) {
            if (!err?.originalStatus) {
                // isLoading: true until timeout occurs
                setErrMsg('No Server Response');
            } else if (err.originalStatus === 400) {
                setErrMsg('Missing Username or Password');
            } else if (err.originalStatus === 401) {
                setErrMsg('Unauthorized');
            } else {
                setErrMsg('Login Failed');
            }
            errRef.current.focus();
        }
    }

    const handleEmailInput = (e) => setEmail(e.target.value)

    const handlePwdInput = (e) => setPwd(e.target.value)

    const content = isLoading || isLoading2 ? <h1>Loading...</h1> : (
        <section className="login">
            <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>

            <h1>Employee Login</h1>

            <form onSubmit={handleSubmit}>
                <label htmlFor="email">Email:</label>
                <input
                    type="text"
                    id="email"
                    ref={emailRef}
                    value={email}
                    onChange={handleEmailInput}
                    autoComplete="off"
                    required
                />

                <label htmlFor="password">Password:</label>
                <input
                    type="password"
                    id="password"
                    onChange={handlePwdInput}
                    value={password}
                    required
                />
                <button>Sign In</button>
            </form>
        </section>
    )

    return content
}
export default Login