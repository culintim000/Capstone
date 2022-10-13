import React from 'react'
import './animals.css'
import {ferret, hamster, parrot, rabbit, reptile, snake, cat, dog} from './imports';

const Animals = () => {
    return (
        <div className="animals section__padding" id="why-us">
            <div>
                <img src={ferret}/>
            </div>
            <div>
                <img src={hamster}/>
            </div>
            <div>
                <img src={parrot}/>
            </div>
            <div>
                <img src={rabbit}/>
            </div>
            <div>
                <img src={reptile}/>
            </div>
            <div>
                <img src={snake}/>
            </div>
            <div>
                <img src={cat}/>
            </div>
            <div>
                <img src={dog}/>
            </div>
        </div>
    )
}

export default Animals

//parrot <a href="https://www.freepik.com/free-vector/exotic-birds-set_3817867.htm#query=parrot&position=0&from_view=search&track=sph">Image by macrovector</a> on Freepik
//reptile and snake <a href="https://www.freepik.com/free-vector/reptiles-amphibians-set-turtle-lizard-triton-gecko-isolated-shite-background-vector-illustration-animals-wildlife-rainforest-fauna-concept_11671959.htm#query=reptile%20pet&position=0&from_view=search&track=sph">Image by pch.vector</a> on Freepik
//rabbit hamster and ferret Image by <a href="https://www.freepik.com/free-vector/hand-drawn-style-cute-pets_7879354.htm#&position=0&from_view=collections">Freepik</a>
//dog <a href="https://www.freepik.com/free-vector/set-dog-cartoon_22740732.htm#query=dog%20drawing&position=45&from_view=search&track=sph">Image by brgfx</a> on Freepik
//cat <a href="https://www.freepik.com/free-vector/sticker-template-cat-cartoon-character_19785514.htm#query=cat%20drawing&position=41&from_view=search&track=sph">Image by brgfx</a> on Freepik
