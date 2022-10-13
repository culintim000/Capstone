import React from 'react';
import dog from '../../assets/amenitiesDog.png'; //Photo by Inge Van den Heuvel: https://www.pexels.com/photo/german-shepherd-in-close-up-photography-11570517/
import './amenities.css';

const amenities = () => (
    <div className="amenities section__padding" id="amenities">
      <div className="amenities-image">
        <img src={dog} alt="amenities" />
      </div>
      <div className="amenities-content">
        <h1 className="gradient__text">Many different amenities so each animal has something they love.</h1>
          <p>Note that some amenities are for specific animals only.</p>
          <ul className="amenities-list">
              <li>Large Dog Play Area</li>
              <li>Small Dog Play Area</li>
              <li>Indoor Pool</li>
              <li>Enclosed Outside Space for Cats</li>
              <li>Inside Area Designed for Cats</li>
              <li>Private Cages for Cats</li>
              <li>Indoor Dog Sleeping</li>
              <li>Large Play Area for Social Small Mammals</li>
              <li>Clean Cages for Small Mammals</li>
              <li>Specialized Tanks for Reptiles</li>
              <li>Many Sizes of Cages for Birds</li>
              <li>Different Food Options for all Animals</li>
              <li>Cat Nail Clipping</li>
              <li>Dog Grooming</li>
              <li>Special Rooms for Boarding+ and Boarding Premium</li>
          </ul>
      </div>
    </div>
);

export default amenities;