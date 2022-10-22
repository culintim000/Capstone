import React from 'react';
import {decodeToken} from "react-jwt";
import './userHome.css';
import Navbar from "../userNavbar/Navbar";
import {Footer} from "../../containers";
import { useCookies } from 'react-cookie';

function UserHome() {
    const [cookies, setCookie] = useCookies(['user']);

    try {
        if (cookies.token === undefined) {
            throw new Error("User not logged in");
        }

        const decodedToken = decodeToken(cookies.token);
        if (decodedToken.exp * 1000 < Date.now()) {
            console.log("Token expired");
            throw new Error("Token expired");
        }

        return (
            <div>
                <div className={"gradient__bg__logInSignUp"}><Navbar/></div>

                <div className={"page_margin"}>
                    <div className="day_care day_care_background">one</div>
                    <div className={"boarding boarding_background"}>two</div>
                </div>
                <Footer/>
            </div>
        );
    } catch (error) {
        console.log(error);
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color:"#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }
}

export default UserHome;