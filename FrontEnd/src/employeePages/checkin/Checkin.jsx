import React, {useEffect, useState} from 'react'
import Navbar from "../employeeNavbar/Navbar";
import './checkin.css'
import {decodeToken} from "react-jwt";
import EDayCareFeature from "../eDaycareFeature/EDayCareFeature";
import EBoardingFeature from "../eBoardingFeature/EBoardingFeature";
import {useCookies} from "react-cookie";

function Checkin() {
    const [dayCares, setDayCares] = useState("");
    const [boardings, setBoardings] = useState("");
    const [cookies, setCookie] = useCookies(['user']);

    const FetchBookings = async event => {
        const requestOptions = {
            method: 'GET'
        };

        const response = await fetch(' http://localhost:8888/bookingservice/book/todaysDaycares', requestOptions)
        const data = await response.json();
        setDayCares(data);
        // console.log(data);

        const response2 = await fetch(' http://localhost:8888/bookingservice/book/todaysBoardings', requestOptions)
        const data2 = await response2.json();
        setBoardings(data2);
        // console.log(data2);
    }

    useEffect(() => {
        FetchBookings();
    }, []);

    // console.log(dayCares[0].animalName);

    function ShowDayCares() {
        const DayCareFeatures = [];

        for (let i = 0; i < dayCares.length; i++) {
            // console.log(dayCares[i]);
            DayCareFeatures.push(<EDayCareFeature animalName={dayCares[i].animalName}
                                                  animalAge={dayCares[i].animalAge}
                                                  animalType={dayCares[i].animalType}
                                                  dropOffTime={dayCares[i].dropOffTime} endDate={dayCares[i].endDate}
                                                  isCheckedIn={dayCares[i].isCheckedIn} notes={dayCares[i].notes}
                                                  pickUpTime={dayCares[i].pickUpTime}
                                                  startDate={dayCares[i].startDate}
                                                  pricePerHour={dayCares[i].pricePerHour}
                                                  ownerEmail={dayCares[i].ownerEmail} id={dayCares[i].id}/>)
        }

        if (DayCareFeatures.length === 0) {
            return (<div className="noBookings">No bookings</div>)
        } else {
            return (
                <div className={"page_margin"}>
                    {DayCareFeatures}
                </div>
            )
        }
    }

    function ShowBoardings() {
        const BoardingFeatures = [];

        for (let i = 0; i < boardings.length; i++) {
            // console.log(boardings[i]);

            BoardingFeatures.push(<EBoardingFeature animalName={boardings[i].animalName}
                                                    animalAge={boardings[i].animalAge}
                                                    animalType={boardings[i].animalType}
                                                    dropOffTime={boardings[i].dropOffTime}
                                                    endDate={boardings[i].endDate}
                                                    isCheckedIn={boardings[i].isCheckedIn} notes={boardings[i].notes}
                                                    pickUpTime={boardings[i].pickUpTime}
                                                    startDate={boardings[i].startDate}
                                                    pricePerNight={boardings[i].pricePerNight}
                                                    ownerEmail={boardings[i].ownerEmail} id={boardings[i].id}/>)
        }

        if (BoardingFeatures.length === 0) {
            return (<div className="noBookings">No bookings</div>)
        } else {
            return (
                <div className={"page_margin"}>
                    {BoardingFeatures}
                </div>
            )
        }
    }

    async function SearchWithUserEmail(e){
        e.preventDefault();
        const email = document.getElementById("search").value;
        // console.log(email);
        const requestOptions = {
            method: 'GET'
        };

        const response = await fetch(' http://localhost:8888/bookingservice/book/searchForBoardings?email=' + email, requestOptions)
        const data = await response.json();
        // setDayCares("");
        setBoardings(data);

        const response2 = await fetch(' http://localhost:8888/bookingservice/book/searchForDayCares?email=' + email, requestOptions)
        const data2 = await response2.json();
        // setDayCares("");
        setDayCares(data2);
    }

    if (cookies.token === undefined) {
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }

    const decodedToken = decodeToken(cookies.token);
    if (decodedToken.exp * 1000 < Date.now()) {
        console.log("Token expired");
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }

    if (decodedToken.Role === "User"){
        console.log("Role is User");
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }

    return (
        <div>
            <Navbar/>
            <div className={"outer_checkin"}>
                <div className={"searchDiv"}>
                    <form onSubmit={SearchWithUserEmail}>
                        <input id={"search"} type="search" placeholder={"Search"}/>
                        <button className={"buttonSearch"} type="submit"></button>
                    </form>
                </div>

                <hr className={"separator"}/>
                {ShowDayCares()}
                <hr/>
                {ShowBoardings()}
            </div>
        </div>
    );
}

export default Checkin