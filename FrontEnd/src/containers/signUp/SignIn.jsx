import React, {useState} from 'react';
import {decodeToken, useJwt} from "react-jwt";
import {useNavigate} from 'react-router-dom';
import {UserHome} from "../../userPages";
import {useCookies} from 'react-cookie';
import './signUp.css';

function SignIn(props) {
    const [user, setUser] = useState({email: "", password: ""});
    const [error, setError] = useState("");
    const navigate = useNavigate();
    const [cookies, setCookie] = useCookies(['user']);

    const Login = async event => {
        event.preventDefault();

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({Email: user.email, Password: user.password})
        };


        const response = await fetch('http://localhost:8888/auth-service/auth/login', requestOptions)

        if (response.status === 200) {
            const data = await response.json();
            // console.log(data.token);
            // console.log(decodeToken(data.token).headers);

            setCookie('token', data.token, {path: '/'});

            if (data.isAdmin) {
                navigate("/admin/home", {state: {data: data}});
            } else if (data.isWorker) {
                navigate("/employee/home", {state: {data: data}});
            } else {
                navigate("/user/home", {state: {data: data}});
            }
            // console.log(data);
            // if (decodedToken.Role === "User") {
            //     navigate("/user/home", {state:{data:data}});
            // }


            return;
        }
        const text = await response.text()


        if (text === "User doesn't exist") {
            setError("User doesn't exist, please try again or sign up for a new account");
        } else if (text === "Wrong password.") {
            setError("Incorrect password, please try again");
        } else {
            setError("Something went wrong please try again");
        }
    }

    return (
        <div className="outer">
            {(error !== "") ? (<div className="error">{error}</div>) : ""}
            <div className="form">
                <form onSubmit={Login}>
                    <div className="input-container">
                        <label>Email </label>
                        <input type="email" name="email" id="email"
                               onChange={e => setUser({...user, email: e.target.value})} value={user.email} required/>
                    </div>
                    <div className="input-container">
                        <label>Password </label>
                        <input type="password" name="pass" onChange={e => setUser({...user, password: e.target.value})}
                               value={user.password} required/>
                    </div>
                    <div className="button-container input-container">
                        <button type="submit">Sign In</button>
                    </div>
                    <div className={"forgot_password_link"}>
                        <a href="/forgot-password">Forgot Password?</a>
                    </div>

                </form>

            </div>
        </div>
    )
}

export default SignIn;