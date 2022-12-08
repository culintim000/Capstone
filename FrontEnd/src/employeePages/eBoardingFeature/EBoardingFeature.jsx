import React, {useEffect, useState} from 'react';
import './eBoardingFeature.css';

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

const EBoardingFeature = ({
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
                              ownerEmail,
                              id
                          }) => {
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

    async function CheckInBoarding(e) {
        e.preventDefault();
        const requestOptions = {
            method: 'POST'
        };

        const response = await fetch(' http://localhost:8888/bookingservice/book/checkInBoarding?email=' + ownerEmail + '&animalName=' + animalName, requestOptions)
        // const data = await response.json();
        // console.log(data);
        // console.log(response)

        if (response.status === 200) {
            setIsCheckedInStatus(true);
        } else if (response.status === 404) {
            alert("Check in failed");
        } else {
            alert("Check in failed for suspicious reasons");
        }
    }

    async function CheckOutBoarding(e) {
        e.preventDefault()
        const requestOptions = {
            method: 'POST'
        };

        const response = await fetch('http://localhost:8888/bookingservice/book/checkOutBoarding?email=' + ownerEmail + "&animalName=" + animalName, requestOptions)
        // const data = await response.json();
        // console.log(data);
        // console.log(response)
        setIsCheckedInStatus(false);
    }

    return (
        <div className="boarding_background">
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
            <div className={"day_care_price"}>
                <p>Price per night: ${pricePerNight}</p>
            </div>
            <div className={"boarding_email"}>
                <p>Email: {ownerEmail}</p>
            </div>
            <div className={"animalInOurCare"}>
                <p>Currently in our care: {isCheckedInStatus.toString()}</p>
            </div>

            <div className={"day_care_btn_div"}>
                <button className={"day_care_btn"} onClick={CheckInBoarding} hidden={isCheckedInStatus}>Check In
                </button>
                <button className={"day_care_btn"} hidden={!isCheckedInStatus} onClick={CheckOutBoarding}>Check Out
                </button>
            </div>
            <div className={"day_care_notes"}>
                <p>Notes: {notes}</p>
            </div>
        </div>);
};

export default EBoardingFeature;