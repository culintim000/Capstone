import React, {useEffect, useState} from 'react'
import Navbar from "../employeeNavbar/Navbar";
import EDayCareFeature from "../eDaycareFeature/EDayCareFeature";
import EBoardingFeature from "../eBoardingFeature/EBoardingFeature";
import {decodeToken} from "react-jwt";
import {useCookies} from "react-cookie";

function Checkout() {
    const [dayCares, setDayCares] = useState("");
    const [boardings, setBoardings] = useState("");
    const [cookies, setCookie] = useCookies(['user']);

    const FetchBookings = async event => {
        const requestOptions = {
            method: 'GET'
        };

        const response = await fetch(' http://localhost:8888/bookingservice/book/getCheckedInDayCares', requestOptions)
        const data = await response.json();
        setDayCares(data);
        // console.log(data);

        const response2 = await fetch(' http://localhost:8888/bookingservice/book/getCheckedInBoardings', requestOptions)
        const data2 = await response2.json();
        setBoardings(data2);
        // console.log(data2);
    }

    useEffect(() => {
        FetchBookings();
    }, []);

    // console.log(boardings);

    function ShowDayCares() {
        const DayCareFeatures = [];

        for (let i = 0; i < dayCares.length; i++) {
            // console.log(dayCares[i]);
            DayCareFeatures.push(<EDayCareFeature animalName={dayCares[i].daycare.animalName}
                                                  animalAge={dayCares[i].daycare.animalAge}
                                                  animalType={dayCares[i].daycare.animalType}
                                                  dropOffTime={dayCares[i].daycare.dropOffTime} endDate={dayCares[i].daycare.endDate}
                                                  isCheckedIn={dayCares[i].daycare.isCheckedIn} notes={dayCares[i].daycare.notes}
                                                  pickUpTime={dayCares[i].daycare.pickUpTime}
                                                  startDate={dayCares[i].daycare.startDate}
                                                  pricePerHour={dayCares[i].daycare.pricePerHour}
                                                  ownerEmail={dayCares[i].email} id={dayCares[i].daycareId}/>)
        }

        if (DayCareFeatures.length === 0) {
            return (<div className="noBookings">No Daycares Check In</div>)
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

            BoardingFeatures.push(<EBoardingFeature animalName={boardings[i].boarding.animalName}
                                                    animalAge={boardings[i].boarding.animalAge}
                                                    animalType={boardings[i].boarding.animalType}
                                                    dropOffTime={boardings[i].boarding.dropOffTime}
                                                    endDate={boardings[i].boarding.endDate}
                                                    isCheckedIn={boardings[i].boarding.isCheckedIn} notes={boardings[i].notes}
                                                    pickUpTime={boardings[i].boarding.pickUpTime}
                                                    startDate={boardings[i].boarding.startDate}
                                                    pricePerNight={boardings[i].boarding.pricePerNight}
                                                    ownerEmail={boardings[i].email} id={boardings[i].boardingId}/>)
        }

        if (BoardingFeatures.length === 0) {
            return (<div className="noBookings">No Boardings Checked In</div>)
        } else {
            return (
                <div className={"page_margin"}>
                    {BoardingFeatures}
                </div>
            )
        }
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
                <hr className={"separator"}/>
                {ShowDayCares()}
                <hr/>
                {ShowBoardings()}
            </div>
        </div>
    );
}

export default Checkout;