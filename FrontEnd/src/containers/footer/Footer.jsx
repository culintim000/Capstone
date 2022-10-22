import React from 'react';
import logo from '../../assets/pawLogo.png';
import './footer.css';

const Footer = () => (
    <div className="footer section__padding" id="location">
      <div className="footer-heading">
        <h1 className="gradient__text">We can't wait to see you</h1>
        <img src={logo} alt="logo" />
      </div>

      <div className="footer-btn">
        <p>Office Phone Number: (801) 593-1234</p>
        <p>Address: 126 Red Pkwy. Salt Lake City, UT 84111</p>
        <p>Office Hours: 8 a.m. - 6 p.m.</p>
        <p>Drop off/Pick up Times: 5 a.m. - 9 p.m.</p>
        <p>Cancellation Fee - $20</p>
      </div>

      <div className="footer-copyright">
        <p>@2022 Paws in Good Handsⓒ. All rights reserved.</p>
      </div>
    </div>
);

export default Footer;