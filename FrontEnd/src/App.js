import React from 'react'
import {Animals, Navbar} from './components'
import {Footer, Services, Header, WhyUs, Amenities, signUp} from './containers'
import {BrowserRouter, Router, Route, Switch} from 'react-router-dom';
import './App.css'
import SignUp from "./containers/signUp/SignUp";

const App = () => {
    return (
        <div className='App'>
            {/*<Route exact path={"/"} componet={Services}/>*/}
            <Route exact path={"/"} render={<h1>TEST</h1>}/>
        </div>

)
}

function Test(){
    return <p>test</p>;
}

function Home() {
    return <p>Home</p>;
    // return (
    //     <div className='App'>
    //         <div className='gradient__bg'>
    //             <Navbar/>
    //             <Header/>
    //         </div>
    //         <Animals/>
    //         <WhyUs/>
    //         <Services/>
    //         <Amenities/>
    //         <Footer/>
    //     </div>
    // )
}

export default App