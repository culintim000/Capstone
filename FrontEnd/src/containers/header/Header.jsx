import React from 'react';
import cat from '../../assets/headerCat.png'; //Photo by Just  a Couple Photos from Pexels: https://www.pexels.com/photo/photo-of-tabby-cat-3777622/
import './header.css';

const Header = () => (
  <div className="header section__padding" id="home">
    <div className="header-content">
      <h1 className="gradient__text">Welcome to Paws in Good Hands</h1>
      <p>
          Paws in Good Hands has one job and that is to make you and your animal happy and comfortable
          by allowing you to check in animals for daycare or multiple days where they get the highest level of care.
          We accept many different types of animals including dogs, cats, birds, reptiles, and even small mammals like hamsters and ferrets.
      </p>
    </div>

    <div className="header-image">
      <img src={cat}/>
    </div>
  </div>
);

export default Header;