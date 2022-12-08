import React from "react";
import ReactDOM from "react-dom";
import { CookiesProvider } from "react-cookie";
import App from "./App";
import './index.css'
import {BrowserRouter as Router, Route, Routes} from "react-router-dom";
import {Animals, Navbar} from './components'
import {Footer, Services, Header, WhyUs, Amenities, SignUp, SignIn, ForgotPassword} from './containers'
import {UserHome, DayCare, Checkout, Boarding} from "./userPages";
import {EmployeeHome, CheckIn, CheckOut, MyTasks} from "./employeePages";
import {AdminHome, EditShop} from "./adminPages";
import {ShopHome, Cart, ShopCheckout} from "./shop";

ReactDOM.render(
    <CookiesProvider>
    <Router>
        <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="/sign-up" element={<SignUpPage />}/>
            <Route path="/sign-in" element={<SignInPage />}/>
            <Route path="/user/home" element={<UserHome />}/>
            <Route path="/user" element={<UserHome />}/>
            <Route path="/user/daycare" element={<DayCare />}/>
            <Route path="/user/checkout" element={<Checkout />}/>
            <Route path="/user/boarding" element={<Boarding />}/>
            <Route path="/employee/home" element={<EmployeeHome />}/>
            <Route path="/employee" element={<EmployeeHome />}/>
            <Route path="/employee/checkin" element={<CheckIn/>}/>
            <Route path="/employee/checkout" element={<CheckOut/>}/>
            <Route path="/employee/myTasks" element={<MyTasks/>}/>
            <Route path="/admin/home" element={<AdminHome/>}/>
            <Route path="/forgot-password" element={<ForgotPassword/>}/>
            <Route path="/admin/shop" element={<EditShop/>}/>
            <Route path="/shop" element={<ShopHome/>}/>
            <Route path="/shop/cart" element={<Cart/>}/>
            <Route path="/shop/checkout" element={<ShopCheckout/>}/>
        </Routes>
    </Router>
    </CookiesProvider>,

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