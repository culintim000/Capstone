import React, {useEffect, useState} from 'react';
import "./employeeHome.css"
import Navbar from "../employeeNavbar/Navbar";
import {TaskFeature} from "../index";
import EDayCareFeature from "../eDaycareFeature/EDayCareFeature";
import {decodeToken} from "react-jwt";
import {useCookies} from "react-cookie";

function EmployeeHome () {
    const [tasks, setTasks] = useState("");
    const [cookies, setCookie] = useCookies(['user']);


    useEffect(  () => {
        const FetchTasks = async event => {
            const requestOptions = {
                method: 'GET'
            };

            const response = await fetch('http://localhost:8888/bookingservice/emp/tasks', requestOptions)
            const data = await response.json();
            console.log(data);
            setTasks(data);
        }
        FetchTasks();
    }, []);


    function ShowDayCares() {
        const taskFeatures = [];

        // taskFeatures.push(<TaskFeature appointmentId={"test"} name={"tasks[i].name"}
        //                                description={"tasks[i].descriptiontasks[i].descriptiontasks[i].descriptiontasksdescriptiontasksdescridescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasksptiontasksdescriptiontasksdescriptiontasksdescriptiontasksdescriptiontasks[i].descriptiontasks[i].descriptiontasks[i].descriptiontasks[i].descriptiontasks[i].descriptiontasks[i].descriptiontasks[i].descriptiontasks[i].description"} startHour={"tasks[i].startHour"}
        //                                startMinute={"tasks[i].startMinute"} isCompleted={"tasks[i].isCompleted"}/>)

        for (let i = 0; i < tasks.length; i++) {
            // console.log(tasks[i]);
            taskFeatures.push(<TaskFeature appointmentId={tasks[i]._AppointmentId} name={tasks[i].name}
                                           description={tasks[i].description} startHour={tasks[i].startHour}
                                           startMinute={tasks[i].startMinute} isCompleted={tasks[i].isCompleted} isPickedUp={tasks[i].isPickedUp}/>)
        }

        if (taskFeatures.length === 0) {
            return (<div className="noBookings">No tasks to display</div>)
        }

        return (
            <div className={"taskList"}>
                {taskFeatures}
            </div>
        )
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
            <div className={"employee_navbar"}><Navbar/></div>

            <div className={"todo_container"}>
                {ShowDayCares()}
            </div>
        </div>
    );
}

export default EmployeeHome;