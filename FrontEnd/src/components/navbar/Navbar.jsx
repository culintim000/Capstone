import React, { useState } from 'react'
import { RiMenu3Line, RiCloseLine } from 'react-icons/ri'
import './navbar.css'
import logo from '../../assets/pawLogo.png' //https://www.freepik.com/free-vector/paw-print-sticker-animal-vector-clipart-paper-textured-design_19085851.htm#page=2&query=pet%20logo&position=22&from_view=search&track=sph" Image by rawpixel.com
import {Link} from 'react-router-dom';

const Menu = () => (
  <>
    <p><a href="/#home">Home</a></p>
    <p><a href="/#why-us">Why Choose Us</a></p>
    <p><a href="/#services">Services Offered</a></p>
    <p><a href="/#amenities">Amenities</a></p>
    <p><a href="/#location">Location</a></p>
  </>
)

const Navbar =
  () => {
    const [toggleMenu, setToggleMenu] = useState(false);
    return (
      <div className='navbar'>
        <div className='navbar-links'>
          <div className='navbar-links_logo'>
            <img src={logo} alt='logo' />
          </div>
          <div className='navbar-links_container'>
            <Menu />
          </div>
        </div>
        <div className='navbar-sign'>
          <p><Link to="/sign-in">Sign In</Link></p>
          <button type='button'><Link to="/sign-up">Sign Up</Link></button>
        </div>
        <div className='navbar-menu'>
          {toggleMenu
            ? <RiCloseLine color="#fff" size={27} onClick={() => setToggleMenu(false)} />
            : <RiMenu3Line color="#fff" size={27} onClick={() => setToggleMenu(true)} />}

          {toggleMenu && (
            <div className='navbar-menu_container scale-up-center'>
              <div className="navbar-menu_container-links">
                <Menu />
              </div>
              <div className="navbar-menu_container-links-sign">
                <p><Link to="/sign-in">Sign In</Link></p>
                <button type="button"><Link to="/sign-in">Sign Up</Link> </button>
              </div>
            </div>
          )}
        </div>
      </div>
    )

  }

const NavigateToSignUp = () => {
    console.log("Navigate to sign up");
    // Navigate("/sign-up");
};

export default Navbar