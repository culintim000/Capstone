import React from 'react';
import './signUp.css';

const SignIn = () => {
    return (
        <div className="form">
            <form>
                <div className="input-container">
                    <label>Email </label>
                    <input type="email" name="email" required />
                </div>
                <div className="input-container">
                    <label>Password </label>
                    <input type="password" name="pass" required />
                </div>
                <div className="button-container input-container">
                    <button type="button">Sign In</button>
                </div>
            </form>
        </div>
    );
}

export default SignIn;