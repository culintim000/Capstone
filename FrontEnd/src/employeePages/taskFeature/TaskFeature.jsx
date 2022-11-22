import React, {useEffect, useState} from 'react';
import './taskFeature.css';
import {useCookies} from "react-cookie";
import {decodeToken} from "react-jwt";

const TaskFeature = ({appointmentId, name, description, startHour, startMinute, isCompleted, isPickedUp}) => {
    const [noReschedule, setReschedule] = useState(true);
    const [noPickup, setPickup] = useState(!isPickedUp);
    const [complete, setComplete] = useState(isCompleted);
    const [appointment, setAppointment] = useState("");
    const [cookies, setCookie] = useCookies(['user']);
    const [taskPickedUp, setTaskPickedUp] = useState(isPickedUp);
    // const [startTime, setStartTime] = useState(startHour + ":" + startMinute);
    const [localStartHour, setLocalStartHour] = useState(startHour);
    const [localStartMinute, setLocalStartMinute] = useState(startMinute);
    const [disableButtons, setDisableButtons] = useState(false);
    const [img, setImg] = React.useState();

    useEffect(() => {
        GetAppointment();
        const getImg = async () => {
            const res = await fetch("http://localhost:5261/pic?name=" + appointmentId);

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
    }, []);

    function SetImage(){
        if (img !== undefined) {
            return (
                <div className={"task_profileImage"}>
                    <img src={img} alt={"test"}/>
                </div>
            );
        }
    }

    async function PickUpTask() {
        setPickup(false);
        setTaskPickedUp(true);

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                _AppointmentId: appointmentId,
                Name: name,
                Description: description,
                StartHour: startHour,
                StartMinute: startMinute,
                IsCompleted: isCompleted,
                IsPickedUp: taskPickedUp,
            })
        };

        const response = await fetch('http://localhost:5241/emp/pickUpTask?email=' + decodeToken(cookies.token).Email, requestOptions)
        if (response.status !== 200){
            alert("Could not pick up the task")
        }
    }

    async function CompleteTask() {
        setDisableButtons(true);

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                _AppointmentId: appointmentId,
                Name: name,
                Description: description,
                StartHour: startHour,
                StartMinute: startMinute,
                IsCompleted: isCompleted,
                IsPickedUp: taskPickedUp,
            })
        };

        const response = await fetch('http://localhost:5241/emp/completeTask?email=' + decodeToken(cookies.token).Email, requestOptions)
        if (response.status !== 200){
            alert("Could not pick up the task")
        }
    }

    function Reschedule() {
        if (noReschedule) {
            setReschedule(false);
        } else setReschedule(true);
    }

    async function DropTask(){
        setPickup(true);
        setTaskPickedUp(false);

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                _AppointmentId: appointmentId,
                Name: name,
                Description: description,
                StartHour: localStartHour,
                StartMinute: localStartMinute,
                IsCompleted: isCompleted,
                IsPickedUp: taskPickedUp,
            })
        };

        const response = await fetch('http://localhost:5241/emp/dropTask?email=' + decodeToken(cookies.token).Email, requestOptions)

        if (response.status !== 200){
            alert("Could not drop the task")
        }

    }

    const GetAppointment = async event => {
        const requestOptions = {
            method: 'GET',
        };

        const response = await fetch(' http://localhost:5241/book/getCheckedInDaycareWithId?id=' + appointmentId, requestOptions)

        if (response.status === 200) {
            const data = await response.json();
            // console.log(data);
            setAppointment(data);
        } else {
            const requestOptions2 = {
                method: 'GET',
            };

            const response2 = await fetch(' http://localhost:5241/book/getCheckedInBoardingWithId?id=' + appointmentId, requestOptions)
            if (response2.status === 200) {
                const data2 = await response2.json();
                // console.log(data2);
                setAppointment(data2);
            }
        }
    }

    const RescheduleTask = async e => {
        setPickup(true);
        setTaskPickedUp(false);
        let hour = 0;
        let minute = 0;

        if (e.target.value > 5){
            hour = 0;
            minute = e.target.value;
        }
        else{
            hour = e.target.value;
            minute = 0;
        }

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                _AppointmentId: appointmentId,
                Name: name,
                Description: description,
                StartHour: localStartHour,
                StartMinute: localStartMinute,
                IsCompleted: isCompleted,
                IsPickedUp: taskPickedUp,
            })
        };

        const response = await fetch('http://localhost:5241/emp/rescheduleTask?email=' + decodeToken(cookies.token).Email + '&hour=' + hour + '&minute=' + minute, requestOptions)
        const data = await response.json();
        console.log(data);

        if (response.status !== 200){
            alert("Could not drop the task")
        }

        setLocalStartHour( data.startHour);
        setLocalStartMinute(data.startMinute);
    }

    function DisplayTime(){
        if (localStartMinute < 10 ){
            return(
                <div className={"task_start_time task_detail"}>
                    <p>StartTime: {localStartHour}:0{localStartMinute}</p>
                </div>
            )
        }
        else{
            return(
                <div className={"task_start_time task_detail"}>
                    <p>StartTime: {localStartHour}:{localStartMinute}</p>
                </div>
            )
        }
    }

    const GetBookingType = () => {
      if (appointment.pricePerHour !== undefined) {
        return "Daycare";
      }
      else if (appointment.pricePerNight !== undefined) {
        return "Boarding";
      }
    }

// console.log(appointment);
    return (
        <div className="task_container">
            <div className={"task_name"}>
                <p>{name}</p>
            </div>
            <div className={"twoSides"}>
                <div className={"leftSide"}>
                    {DisplayTime()}
                    <div className={"task_is_completed task_detail"}>
                        <p>Is Completed: {complete.toString()}</p>
                    </div>
                </div>
                <div className={"middle"}>
                    {SetImage()}
                </div>
                <div className={"rightSide"}>
                    <div className={"animalName"}>
                        <p>Animal Name: {appointment.animalName}</p>
                    </div>
                    <div className={"animalType"}>
                        <p>Animal Type: {appointment.animalType}</p>
                    </div>
                    <div>
                        <p>Booking Type: {GetBookingType()}</p>
                    </div>
                </div>
            </div>
            <div className={"task_description task_detail"}>
                <p>Description:</p>
                <p>{description}</p>
            </div>
            <div className={"main_buttons"}>
                <button hidden={!noPickup} onClick={PickUpTask} id={"pickUpBtn"} disabled={disableButtons}>Pick Up</button>
                <button hidden={noPickup} onClick={DropTask} id={"dropBtn"} disabled={disableButtons}>Drop Task</button>
                <button hidden={noPickup} onClick={CompleteTask} id={"completeBtn"} disabled={disableButtons}>Complete</button>
                <button onClick={Reschedule} id={"rescheduleBtn"} disabled={disableButtons}>Reschedule</button>
            </div>

            <hr className={"sep"} hidden={noReschedule}/>
            <div className={"reschedule_buttons"}>
                <div className={"time_btn"}>
                    <button hidden={noReschedule} value={15} onClick={RescheduleTask}>15 min</button>
                    <button hidden={noReschedule} value={30} onClick={RescheduleTask}>30 min</button>
                    <button hidden={noReschedule} value={45} onClick={RescheduleTask}>45 min</button>
                    <button hidden={noReschedule} value={1} onClick={RescheduleTask}>1 hr</button>
                    <button hidden={noReschedule} value={2} onClick={RescheduleTask}>2 hr</button>
                    <button hidden={noReschedule} value={3} onClick={RescheduleTask}>3 hr</button>
                </div>
                <div className={"cancel_btn"}>
                    <button hidden={noReschedule}>Cancel Task</button>
                </div>
            </div>
        </div>
    );
}

export default TaskFeature;