import React from 'react';
import './signUp.css';
import {Link} from "react-router-dom";

const SignUp = () => {
    return (
        <div className="form">
            <form>
                <div className="input-container">
                    <label>Email </label>
                    <input type="email" name="email" required />
                </div>
                <div className="input-container">
                    <label>Phone Number </label>
                    <input type="tel" name="tel" required />
                </div>
                <div className="input-container">
                    <label>Full Name </label>
                    <input type="text" name="name" required />
                </div>
                <div className="input-container">
                    <label>Password </label>
                    <input type="password" name="pass" required />
                </div>
                <div className="input-container">
                    <label>Confirm Password </label>
                    <input type="password" name="confPass" required />
                </div>
                <div className="button-container input-container">
                    <button type="button">Sign Up</button>
                </div>
            </form>
        </div>
    );
};

export default SignUp;