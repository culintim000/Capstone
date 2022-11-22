import React, {useEffect, useState} from 'react';
import {decodeToken} from "react-jwt";
import './userHome.css';
import Navbar from "../userNavbar/Navbar";
import {Footer} from "../../containers";
import {useCookies} from 'react-cookie';
import DayCareFeature from "../dayCareFeature/DayCareFeature"
import BoardingFeature from "../boardingFeature/BoardingFeature";


function UserHome() {
    const [cookies, setCookie] = useCookies(['user']);
    const [dayCares, setDayCares] = useState("");
    const [boardings, setBoardings] = useState("");
    const decodedToken = decodeToken(cookies.token);

    const FetchBookings = async event => {
        const requestOptions = {
            method: 'GET'
        };

        const response = await fetch(' http://localhost:5241/book/daycares?email=' + decodedToken.Email, requestOptions)
        // console.log(response);
        const data = await response.json();
        // console.log(data);
        // return (<div><h1>data</h1></div>)
        setDayCares(data);
        // console.log(dayCares);

        const response2 = await fetch(' http://localhost:5241/book/boardings?email=' + decodedToken.Email, requestOptions)
        const data2 = await response2.json();
        setBoardings(data2);
    }

    function LoadBookings() {
        FetchBookings();
    }

    useEffect(() => {
        LoadBookings();
    }, []);

// console.log(dayCares);
    try {
        if (cookies.token === undefined) {
            throw new Error("User not logged in");
        }

        if (decodedToken.exp * 1000 < Date.now()) {
            console.log("Token expired");
            throw new Error("Token expired");
        }

        function ShowDayCares() {
            const DayCareFeatures = [];

            for (let i = 0; i < dayCares.length; i++) {
                // console.log(dayCares[i]);
                DayCareFeatures.push(<DayCareFeature animalName={dayCares[i].animalName}
                                                     animalAge={dayCares[i].animalAge}
                                                     animalType={dayCares[i].animalType}
                                                     dropOffTime={dayCares[i].dropOffTime} endDate={dayCares[i].endDate}
                                                     isCheckedIn={dayCares[i].isCheckedIn} notes={dayCares[i].notes}
                                                     pickUpTime={dayCares[i].pickUpTime}
                                                     startDate={dayCares[i].startDate}
                                                     pricePerHour={dayCares[i].pricePerHour} id={dayCares[i].id}/>)
            }

            if (DayCareFeatures.length === 0) {
                return (<div className="noBookings">No bookings</div>)
            }
            else {
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

                BoardingFeatures.push(<BoardingFeature animalName={boardings[i].animalName}
                                                       animalAge={boardings[i].animalAge}
                                                       animalType={boardings[i].animalType}
                                                       dropOffTime={boardings[i].dropOffTime}
                                                       endDate={boardings[i].endDate}
                                                       isCheckedIn={boardings[i].isCheckedIn} notes={boardings[i].notes}
                                                       pickUpTime={boardings[i].pickUpTime}
                                                       startDate={boardings[i].startDate}
                                                       pricePerNight={boardings[i].pricePerNight} id={boardings[i].id}/>)
            }

            if (BoardingFeatures.length === 0) {
                return (<div className="noBookings">No bookings</div>)
            }
            else {
                return (
                    <div className={"page_margin"}>
                        {BoardingFeatures}
                    </div>
                )
            }
        }

        return (
            <div>
                <div className={"gradient__bg__logInSignUp"}><Navbar/></div>

                <div className={"today_info"}>
                    <p>Today is: &nbsp;</p>
                    <p>{new Date().toDateString()}</p>
                </div>

                <hr/>
                <div className={"daycare_bookings"}>Upcoming Daycare</div>
                {ShowDayCares()}

                <hr/>
                <div className={"daycare_bookings"}>Upcoming Animal Boardings</div>
                    {ShowBoardings()}
                <Footer/>
            </div>
        );
    } catch (error) {
        console.log(error);
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }
}

export default UserHome;