import React, {useEffect} from 'react';
import './boardingFeature.css';
import axios from "axios";
import {DomSanitizer} from '@angular/platform-browser';


const DisplayTime = (time) => {
    if (time > 12) {
        return (time - 12) + " PM";
    }
    if (time === 12) {
        return time + " PM";
    } else {
        return time + " AM";
    }
}

const ConvertDate = (date) => {
    let newDate = new Date(date);
    newDate.setDate(newDate.getDate() + 1);
    return newDate.toDateString();
}

const BoardingFeature = ({
                                   animalName,
                                   animalAge,
                                   animalType,
                                   pricePerNight,
                                   notes,
                                   dropOffTime,
                                   pickUpTime,
                                   startDate,
                                   endDate,
                                   isCheckedIn,
                                   id
                               }) => {
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

    return (
        <div className="boarding_background_user">
            <div className={"outer_profile_image"}>
                {SetImage()}
                {/*<div className={"profileImage"}>*/}
                {/*    <img src={img} alt={"test"}/>*/}
                {/*</div>*/}
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
            <div className={"boarding_schedule"}>
                <p>Boarding Schedule</p>
            </div>
            <div className={"dp_times boarding_schedule_children"}>
                <p>Drop off: {ConvertDate(startDate)} at {DisplayTime(dropOffTime)}</p>
                {/*<p>Drop off: {dropOffTime}</p>*/}
            </div>
            <div className={"dates boarding_schedule_children"}>
                <p>Pick Up: {ConvertDate(endDate)} at {DisplayTime(pickUpTime)}</p>
                {/*<p>Last Day: {endDate}</p>*/}
            </div>
            <div className={"checkedIn boarding_schedule_children"}>
                <p>Currently in our care: {isCheckedIn.toString()}</p>
            </div>

        </div>
    );
}

export default BoardingFeature;