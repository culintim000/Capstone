import React, {useEffect} from 'react';
import './dayCareFeature.css';

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

const DayCareFeature = ({
                            animalName,
                            animalAge,
                            animalType,
                            pricePerHour,
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
        <div className="day_care_user">
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
            <div className={"checkedIn dc_schedule"}>
                <p>Currently in our care: {isCheckedIn.toString()}</p>
            </div>
            {/*<div className={"is_checked_in"}>*/}
            {/*    <p>Is Checked: {notes}</p>*/}
            {/*</div>*/}
        </div>
    );
}

export default DayCareFeature;