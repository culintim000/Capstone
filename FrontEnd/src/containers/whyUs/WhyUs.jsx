import React from 'react';
import Feature from '../../components/feature/Feature';
import './whyUs.css';

const WhyUs = () => (
    <div className="whatgpt3 section__margin">
        <div className="whatgpt3-heading">
            <h1 className="gradient__text">Why Choose Paws in Good Hands?</h1>
        </div>
        <div className="whatgpt3-container">
            <Feature title="4.7★  on Google"
                     text="With more than 2 thousand reviews on Google our rating stands strong with a 4.7 out of 5 stars."/>
            <Feature title="Most types of animals accepted"
                     text="We are proud to say that our animal hotel accepts the biggest variety of animals in the Salt Lake Valley."/>
            <Feature title="Live Updates on Your Animal"
                     text="Our built in chat allows you to get live updates on your animal. This feature also allows you to ask any questions you have."/>
            <Feature title="Video Surveillance"
                     text="To keep your animals and our works at all times safe we have 24 hours a day video surveillance with audio."/>
        </div>
    </div>
);

export default WhyUs;