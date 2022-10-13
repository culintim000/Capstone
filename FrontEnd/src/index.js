import React from "react";
import ReactDOM from "react-dom";

import App from "./App";
import './index.css'
// import {BrowserRouter} from "react-router-dom";
import {BrowserRouter as Router, Route, Routes} from "react-router-dom";
import {Animals, Navbar} from './components'
import {Footer, Services, Header, WhyUs, Amenities, SignUp, SignIn} from './containers'

ReactDOM.render(
    <Router>
        {/*<div className='gradient__bg'>*/}
        {/*<Navbar/>*/}
        {/*</div>*/}
        <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="/sign-up" element={<SignUpPage />}/>
            <Route path="/sign-in" element={<SignInPage />}/>
        </Routes>
    </Router>,

    document.getElementById("root")
);

function Home() {
    return (
        <div className='App'>
            <div className='gradient__bg'>
                <Navbar/>
                <Header/>
            </div>
            <Animals/>
            <WhyUs/>
            <Services/>
            <Amenities/>
            <Footer/>
        </div>
    );
}

function SignUpPage() {
    return (
        <div className='App'>
            <div className='gradient__bg__logInSignUp'>
                <Navbar/>
            </div>
            <SignUp/>
        </div>
    );
}

function SignInPage() {
    return (
        <div className='App'>
            <div className='gradient__bg__logInSignUp'>
                <Navbar/>
            </div>
            <SignIn/>
        </div>
    );
}