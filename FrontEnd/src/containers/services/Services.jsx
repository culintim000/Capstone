import React from 'react'
import Feature from '../../components/feature/Feature';
import './services.css'

const featuresData = [
  {
    title: 'Daycare for all animals',
    text: 'If you would like to check in your animal for just the day our check in time starts at 6 a.m. and the latest time to pick up is 9 p.m.',
  },
  {
    title: 'Boarding for all animals',
    text: 'If for any reason you need to leave your animal in safe hands for more then a day we got you as we have boarding space for all types of animals.',
  },
  {
    title: 'Grooming for dogs and cats while they are in our care',
    text: 'While your cat or dog is in our care you can select an optional grooming to be done before its your time to pick them up.',
  },
  {
    title: 'Boarding+ for all animals',
    text: 'If you would like to have your animal treated extra special this is the perfect option for you. Boarding+ includes higher quality food, more attention from workers, more than comparable rooms or cages.',
  },
  {
    title: 'Boarding Premium for cats and dogs',
    text: 'Only available for dogs and cats Boarding Premium takes Boarding+ to a whole new level. It comes with an included grooming if you say "yes", food of your choice. Many different comfortable sleeping places with very few other animals in the same room.',
  }
];

const Services = () => {
  return (
    <div className="features section__padding" id="services">
      <div className="features-heading">
        <h1 className="gradient__text">Some of the Services we Offer</h1>
        <p>Sign up to get your animal the best care you can find</p>
      </div>
      <div className="features-container">
        {featuresData.map((item, index) => (
          <Feature title={item.title} text={item.text} key={item.title + index} />
        ))}
      </div>
    </div>
  )
}


export default Services