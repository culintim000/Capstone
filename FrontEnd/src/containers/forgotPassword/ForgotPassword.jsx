import React, {useEffect, useState} from 'react';
import {Navbar} from "../../components";
import "./forgotPassword.css";
import axios from "axios";
import {useNavigate} from 'react-router-dom';

function ForgotPassword() {
    const [email, setEmail] = useState("");
    const [emailSent, setEmailSent] = useState(false);
    const [SendResend, setSendResend] = useState("Send Email");
    const [error, setError] = useState("");
    const [disableEmail, setDisableEmail] = useState(false);
    const navigate = useNavigate();

    async function SendRecoveryEmail(e){
        e.preventDefault();

        let email = document.getElementById("usersEmail").value;

        try{
            const res = await axios.post("http://localhost:8888/auth-service/auth/RecoverPassword?email=" + email)

            if (res.status === 200){
                setEmailSent(true);
                setSendResend("Resend Email");
                setError("");
                setDisableEmail(true);
            }
            else{
                setError("Something went wrong please try again.");
            }
        } catch (err) {
            setError("No User found with that email, please try again.");
        }
    }

    async function ChangePassword(e){
        e.preventDefault();

        let email = document.getElementById("usersEmail").value;
        let password = document.getElementById("usersPassword").value;
        let confirmPassword = document.getElementById("usersConfirmPassword").value;
        let code = document.getElementById("usersCode").value;

        if (password !== confirmPassword){
            setError("Passwords do not match, please try again.");
            return;
        }

        try{
            const result = await axios.post("http://localhost:8888/auth-service/auth/ChangePassword?email=" + email + "&password=" + password + "&verificationCode=" + code)
            if (result.status === 200) {
                setError("");
                navigate("/sign-in");
            }

        } catch (err) {
            setError("Something went wrong with changing your password please try again.");
        }
    }

    return (
        <div>
            <Navbar/>
            <h2 className={"forgotPasswordTitle"}>Forgot Password</h2>
            <div className={'outer_forgot_password'}>

                <div className={"email_form_forgot_password"}>
                    <form onSubmit={SendRecoveryEmail}>
                        <div className={"info_forgot_password"}>
                            {(error !== "") ? (<div className="error">{error}</div>) : ""}
                        </div>
                        <div className={"input-container"}>
                            <label>Enter the email you registered with</label>
                            <input id={'usersEmail'} disabled={disableEmail} type="email" onChange={e => setEmail(e.target.value)} value={email} required/>
                        </div>
                        <div className={"button_forgot_password"}>
                            <button type={"submit"}>{SendResend}</button>
                        </div>


                    </form>
                </div>
                <div hidden={!emailSent} className={"new_password_forgot_password"}>
                    <div className={"info_forgot_password"}>
                        <label>We've sent you a one time verification code please enter it here and choose your new password</label>
                    </div>
                    <form onSubmit={ChangePassword}>
                        <div className={"input-container"}>
                            <label>Enter the verification code</label>
                            <input id={"usersCode"} type="text" required/>
                        </div>

                        <div className={"input-container"}>
                            <label>Enter the new password</label>
                            <input id={"usersPassword"} type="password" required/>
                        </div>

                        <div className={"input-container"}>
                            <label>Confirm the new password</label>
                            <input id={"usersConfirmPassword"} type="password" required/>
                        </div>

                        <div className={"button_forgot_password"}>
                            <button type={"submit"}>Change Password</button>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    )
}

export default ForgotPassword;