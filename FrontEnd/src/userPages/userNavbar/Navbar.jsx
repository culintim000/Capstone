import React, {useState} from 'react'
import {RiMenu3Line, RiCloseLine} from 'react-icons/ri'
import './navbar.css'
import logo from '../../assets/pawLogo.png' //https://www.freepik.com/free-vector/paw-print-sticker-animal-vector-clipart-paper-textured-design_19085851.htm#page=2&query=pet%20logo&position=22&from_view=search&track=sph" Image by rawpixel.com
import {Link, useNavigate} from 'react-router-dom';
import {useCookies} from "react-cookie";

const Menu = () => (
    <>
        <p><a href="/user/home">Home</a></p>
        <p><a href="/user/daycare">Day Care</a></p>
        <p><a href="/user/boarding">Animal Boarding</a></p>
    </>
)

// function Home(){
//     const navigate = useNavigate();
//     navigate("/user/home", {state:{data:data}});
// }

// function LogOut(event){
//     event.preventDefault();
//     const [cookies, setCookie, removeCookie] = useCookies(['user']);
//     const navigate = useNavigate();
//     navigate("/");
//     removeCookie('user', {path: "/"});
// }

const Navbar =
    () => {
        const [toggleMenu, setToggleMenu] = useState(false);
        const [cookies, setCookie, removeCookie] = useCookies(['user']);
        const navigate = useNavigate();

        function LogOut(event){
            event.preventDefault();
            removeCookie('token', {path: "/"});
            navigate("/");
        }

        return (
            <div className='navbar'>
                <div className='navbar-links'>
                    <div className='navbar-links_logo'>
                        <img src={logo} alt='logo'/>
                    </div>
                    <div className='navbar-links_container'>
                        <Menu/>
                    </div>
                </div>
                <div className='navbar-sign'>
                    <button type='button' onClick={event => {LogOut(event)}}>Log Out</button>
                </div>
                <div className='navbar-menu'>
                    {toggleMenu
                        ? <RiCloseLine color="#fff" size={27} onClick={() => setToggleMenu(false)}/>
                        : <RiMenu3Line color="#fff" size={27} onClick={() => setToggleMenu(true)}/>}

                    {toggleMenu && (
                        <div className='navbar-menu_container scale-up-center'>
                            <div className="navbar-menu_container-links">
                                <Menu/>
                            </div>
                            <div className="navbar-menu_container-links-sign">
                                <button type="button" onClick={event => {LogOut(event)}}>Log Out</button>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        )

    }

export default Navbar