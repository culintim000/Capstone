import React, {useEffect, useState} from 'react';
import Navbar from "../employeeNavbar/Navbar";
import {TaskFeature} from "../index";
import {useCookies} from "react-cookie";
import {decodeToken} from "react-jwt";

function MyTasks () {
    const [tasks, setTasks] = useState("");
    const [cookies, setCookie] = useCookies(['user']);

    async function FetchTasks() {
        const requestOptions = {
            method: 'GET'
        };

        const response = await fetch(' http://localhost:5241/emp/getEmployeeTasks?email=' + decodeToken(cookies.token).Email, requestOptions)
        const data = await response.json();
        // console.log(data);
        setTasks(data);
    }

    useEffect(  () => {
        FetchTasks();
    }, []);

    function ShowTasks() {
        const taskFeatures = [];

        // console.log(tasks);
        for (let i = 0; i < tasks.length; i++) {
            // console.log(tasks[i]);
            taskFeatures.push(<TaskFeature appointmentId={tasks[i]._AppointmentId} name={tasks[i].name}
                                           description={tasks[i].description} startHour={tasks[i].startHour}
                                           startMinute={tasks[i].startMinute} isCompleted={tasks[i].isCompleted} isPickedUp={tasks[i].isPickedUp}/>)
        }
        return (
            <div className={"taskList"}>
                {/*<p>TESSt</p>*/}
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
            <Navbar/>
            <h1>My Tasks</h1>
            {ShowTasks()}
        </div>
    );
}

export default MyTasks;