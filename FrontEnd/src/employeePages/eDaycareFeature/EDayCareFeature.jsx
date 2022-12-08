import React, {useEffect, useState} from 'react';
import './eDayCareFeature.css';
import {useCookies} from "react-cookie";
import {decodeToken} from "react-jwt";

const DisplayTime = (time) => {
    if (time > 12){
        return (time - 12) + " PM";
    }
    if (time === 12){
        return time + " PM";
    }
    else {
        return time + " AM";
    }
}

const ConvertDate = (date) => {
    let newDate = new Date(date);
    newDate.setDate(newDate.getDate() + 1);
    return newDate.toDateString();
}

const EDayCareFeature = ({ animalName, animalAge, animalType, pricePerHour,
                             notes, dropOffTime, pickUpTime, startDate, endDate, isCheckedIn, ownerEmail, id }) => {
    const [cookies, setCookie] = useCookies(['user']);
    const decodedToken = decodeToken(cookies.token);
    let [isCheckedInStatus, setIsCheckedInStatus] = useState(isCheckedIn);
    const [img, setImg] = React.useState();

    useEffect(() => {
        const getImg = async () => {
            const res = await fetch("http://localhost:5261/pic?name=" + id);

            if (res.status === 200) {
                const imageBlob = await res.blob();

                let base64data = undefined;
                var reader = new FileReader();
                reader.readAsDataURL(imageBlob);
                reader.onloadend = function() {
                    base64data = reader.result;
                    setImg(base64data);
                }
            }
            else setImg(undefined);
        }
        getImg();
    },[]);

    function SetImage(){
        if (img !== undefined) {
            return (
                <div className={"profileImage"}>
                    <img src={img} alt={"test"}/>
                </div>
            );
        }
    }

    async function CheckInDayCare(e){
        e.preventDefault();
        const requestOptions = {
            method: 'POST'
        };

        const response = await fetch(' http://localhost:8888/bookingservice/book/checkInDaycare?email=' + ownerEmail + '&animalName=' + animalName, requestOptions)
        // const data = await response.json();
        // console.log(data);
        console.log(response)

        if (response.status === 200) {
            setIsCheckedInStatus(true);
        }
        else if (response.status === 404) {
            alert("Check in failed");
        }
        else {
            alert("Check in failed for suspicious reasons");
        }

    }

    async function CheckOutDayCare(e){
        e.preventDefault()
        const requestOptions = {
            method: 'POST'
        };

        const response = await fetch('http://localhost:8888/bookingservice/book/checkOutDaycare?email=' + ownerEmail + "&animalName=" + animalName, requestOptions)
        // const data = await response.json();
        // console.log(data);
        // console.log(response)
        setIsCheckedInStatus(false);
    }

    return (<div className="day_care">
        <div className={"outer_profile_image"}>
            {SetImage()}
        </div>
        <div className={"name"}>
            <p>{animalName}</p>
        </div>
        <div className={"age"}>
            <p>Age: {animalAge}</p>
        </div>
        <div className={"type"}>
            <p>Type: {animalType}</p>
        </div>
        <div className={"daycare_schedule"}>
            <p>Daycare Schedule</p>
        </div>
        <div className={"dp_times dc_schedule"}>
            <p>Drop off: {DisplayTime(dropOffTime)}</p>
            <p>Pick up: {DisplayTime(pickUpTime)}</p>
        </div>
        <div className={"dates dc_schedule"}>
            <p>First Day: {ConvertDate(startDate)}</p>
            <p>Last Day: {ConvertDate(endDate)}</p>
        </div>
        <div className={"day_care_price"}>
            <p>Price per hour: ${pricePerHour}</p>
        </div>
        <div className={"day_care_email"}>
            <p>Email: {ownerEmail}</p>
        </div>
        <div className={"animalInOurCare"}>
            <p>Currently in our care: {isCheckedInStatus.toString()}</p>
        </div>

        <div className={"day_care_btn_div"}>
            <button className={"day_care_btn"} onClick={CheckInDayCare} hidden={isCheckedInStatus}>Check In</button>
            <button className={"day_care_btn"} onClick={CheckOutDayCare} hidden={!isCheckedInStatus}>Check Out</button>
        </div>
        <div className={"day_care_notes"}>
            <p>Notes: {notes}</p>
        </div>

    </div>);
};

export default EDayCareFeature;