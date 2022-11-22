import React, {useEffect, useState} from 'react';
import "./userFeature.css";
import axios from "axios";

const UserFeature = ({email, isWorker, name, phone}) => {
    const [isWorkerState, setIsWorkerState] = useState(isWorker);
    const [noError, setNoError] = useState(true);

    async function MakeEmployee(e) {
        e.preventDefault();

        const result = await axios.post('http://localhost:8888/auth-service/auth/makeWorker?email=' + email);

        if (result.status === 200) {
            setIsWorkerState(true);
        }
        else {
            setNoError(false);
            //get element by id
            document.getElementById("errorCode").innerHTML = "Something went wrong";
        }

    }

    async function RemoveEmployee(e) {
        e.preventDefault();

        const result = await axios.post('http://localhost:8888/auth-service/auth/removeWorker?email=' + email);

        if (result.status === 200) {
            console.log("all good");
            setIsWorkerState(false);
        }
        else {
            console.log("something wrong");
            setNoError(false);
            //get element by id
            document.getElementById("errorCode").innerHTML = "Something went wrong";
        }

    }

    return (
        <div className={"userFeature"}>
            <p className={"errorCode"} hidden={noError}/>
            <div className={"userFeatureName"}>Users Name: {name}</div>
            <div className={"userFeatureEmail"}>Users Email: {email}</div>
            <div className={"userFeaturePhone"}>Users Phone Number: {phone}</div>
            <div className={"userFeatureIsWorker"}>Is User an Employee: {isWorkerState.toString()}</div>
            <div hidden={isWorkerState}>
                <button className={"makeEmployee"} onClick={MakeEmployee}>Make into Employee</button>
            </div>
            <div hidden={!isWorkerState}>
                <button className={"removeEmployee"} onClick={RemoveEmployee}>Remove Employee Status</button>
            </div>

        </div>
    );
}

export default UserFeature;