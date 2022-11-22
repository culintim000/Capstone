import React, {useState} from 'react';
import './signUp.css';
import {useNavigate} from 'react-router-dom';
import {useCookies} from "react-cookie";

function SignUp() {
    const [user, setUser] = useState({email: "", number: "", name: "", password: "", confPassword: ""});
    const [error, setError] = useState("");
    const [cookies, setCookie] = useCookies(['user']);
    const navigate = useNavigate();


    const Register = async event => {
        event.preventDefault();

        if (user.password !== user.confPassword) {
            setError("Passwords do not match");
            return;
        }

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Email: user.email, Number: user.number, Name: user.name, Password: user.password })
        };
        const response = await fetch('http://localhost:8888/auth-service/auth/register', requestOptions)
        if (response.status === 200) {
            const data = await response.json()
            setCookie('token', data.token, { path: '/' });
            navigate("/user/home", {state:{data:data}});
            // console.log(data)
            // return;
        }

        if (await response.text() === "User already exists") {
            setError("User already exists");
        }

        else {
            setError("Something went wrong please try again");
        }
    }

    return (
        <div className="outer">
            {(error !== "") ? (<div className="error">{error}</div>) : ""}
            <div className="form">
                <form onSubmit={Register}>
                    <div className="input-container">
                        <label>Email </label>
                        <input type="email" name="email" id="email" onChange={e => setUser({...user, email: e.target.value})} value={user.email} required />
                    </div>
                    <div className="input-container">
                        <label>Phone Number </label>
                        <input type="number" name="number" onChange={e => setUser({...user, number: e.target.value.slice(0, 11)})} value={user.number}/>
                    </div>
                    <div className="input-container">
                        <label>Full Name </label>
                        <input type="text" name="name" onChange={e => setUser({...user, name: e.target.value})} value={user.name} required />
                    </div>
                    <div className="input-container">
                        <label>Password </label>
                        <input type="password" name="pass" onChange={e => setUser({...user, password: e.target.value})} value={user.password} required />
                    </div>
                    <div className="input-container">
                        <label>Confirm Password </label>
                        <input type="password" name="confPass" onChange={e => setUser({...user, confPassword: e.target.value})} value={user.confPassword} required />
                    </div>
                    <div className="button-container input-container">
                        <button type="submit">Sign Up</button>
                    </div>
                </form>
            </div>
        </div>
    )
}
export default SignUp;