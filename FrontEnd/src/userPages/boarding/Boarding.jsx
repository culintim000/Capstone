import React, {useState,} from 'react';
import Calendar from 'moedim';
import TextareaAutosize from 'react-textarea-autosize';
import "./boarding.css";
import Navbar from "../userNavbar/Navbar";
import {Footer} from "../../containers";
import {decodeToken} from "react-jwt";
import {useCookies} from "react-cookie";
import {useNavigate} from "react-router-dom";


function Boarding() {
    const currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0);
    const [error, setError] = useState("");
    const [cookies, setCookie] = useCookies(['user']);
    const [animal, setAnimal] = useState("Dog");
    const [initPrice, setInitPrice] = useState(40);
    const [appointmentDetails, setAppointmentDetails] = useState({
        bookingType: "Boarding",
        animalName: "",
        animalAge: "",
        dropOff: "6 a.m.",
        pickUp: "8 a.m.",
        notes: "",
        startDate: currentDate,
        endDate: currentDate,
        price: initPrice,
        animalType: animal,
        animalPicture: undefined
    });
    const navigate = useNavigate();

    // console.log(appointmentDetails);

    if (cookies.token === undefined) {
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }

    const SaveFile = (e) => {
        setAppointmentDetails({...appointmentDetails, animalPicture: e.target.files[0]});
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

    const CalculatePrice = (event) => {
        setAnimal(event.target.value);

        let pr = 0;
        if (event.target.value === "Dog" || event.target.value === "Cat" || event.target.value === "Sugar Glider") {
            setInitPrice(40);
            pr = 40;
        } else if (event.target.value === "Parrot" || event.target.value === "Lizard" || event.target.value === "Snake" || event.target.value === "Tortoise" || event.target.value === "Turtle" || event.target.value === "Ferret") {
            setInitPrice(30);
            pr = 30;
        } else {
            setInitPrice(20);
            pr = 20;
        }

        setAppointmentDetails({...appointmentDetails, animalType: event.target.value, price: pr});
    }

    function AddDays(date, days) {
        let result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    }


    const Proceed = async event => {
        event.preventDefault();

        if (appointmentDetails.startDate > appointmentDetails.endDate) {
            setError("The start date must be before the end date");
            return;
        }

        if (appointmentDetails.startDate < currentDate) {
            setError("The start date must be after the current date");
            return;
        }

        if (appointmentDetails.startDate === appointmentDetails.endDate) {
            setError("The start and end dates can't be the same");
            return;
        }

        if (appointmentDetails.endDate > AddDays(currentDate, 90)) {
            setError("You can only book up to 90 days in advance");
            return;
        }
        // const rec = Receipt();
        navigate("/user/checkout", {state: {appointmentDetails: appointmentDetails}});
    }

return (

    <div className={"outer"}>
        <div className={"gradient__bg__logInSignUp"}>
            <Navbar/>
        </div>
        <h1 className={"gradient__text"}>Animal Boarding</h1>
        {(error !== "") ? (<div className="error">{error}</div>) : ""}
        <div className={"form"}>
            <form onSubmit={Proceed}>
                <div className={"form_container"}>
                    <label>Animals Name</label>
                    <input type="text" placeholder={"Name"} required={true} id="animalName"
                           onChange={e => setAppointmentDetails({...appointmentDetails, animalName: e.target.value})}
                           value={appointmentDetails.animalName}/>
                </div>
                <div className={"form_container"}>
                    <label>Animals Age</label>
                    <input type="number" placeholder={"Age"} required={true} id="animalAge"
                           onChange={e => setAppointmentDetails({
                               ...appointmentDetails,
                               animalAge: parseInt(e.target.value)
                           })} value={appointmentDetails.animalAge}/>
                </div>

                <div className={"form_container"}>
                    <label>Type of Animal</label>
                    <select name="selectList" id="selectList" onChange={CalculatePrice}>
                        value={animal} required={true}>
                        <option value="Dog">Dog</option>
                        <option value="Cat">Cat</option>
                        <option value="Parrot">Parrot</option>
                        <option value="Snake">Snake</option>
                        <option value="Tortoise">Tortoise</option>
                        <option value="Turtle">Turtle</option>
                        <option value="Lizard">Lizard</option>
                        <option value="Hamster">Hamster</option>
                        <option value="Ferret">Ferret</option>
                        <option value="Genuine Pig">Genuine Pig</option>
                        <option value="Mice/Rat">Mice/Rat</option>
                        <option value="Sugar Glider">Sugar Glider</option>
                        <option value="Rabbit">Rabbit</option>
                    </select>
                    {(initPrice !== 0) ? (<div className="price">Price Per Night: ${initPrice}</div>) : ""}
                </div>

                <h1 className={"gradient__text"}>Make your own schedule</h1>
                <div className={"schedule"}>
                    <div className={"schedule_calendar"}>
                        <div className={"form_container"}>
                            <label>Starting Date</label>
                            <Calendar value={appointmentDetails.startDate}
                                      onChange={(e) => setAppointmentDetails({...appointmentDetails, startDate: e})}/>
                        </div>
                        <div className={"form_container"}>
                            <label>Ending Date</label>
                            <Calendar value={appointmentDetails.endDate}
                                      onChange={(e) => setAppointmentDetails({...appointmentDetails, endDate: e})}/>
                        </div>
                    </div>
                    <div className={"schedule_time"}>
                        <div className={"form_container"}>
                            <label>Drop-off Time</label>
                            <select name="startTime" required={true} onChange={e => setAppointmentDetails({
                                ...appointmentDetails,
                                dropOff: e.target.value
                            })}>
                                <option value="6 a.m.">6 a.m.</option>
                                <option value="7 a.m.">7 a.m.</option>
                                <option value="8 a.m.">8 a.m.</option>
                                <option value="9 a.m.">9 a.m.</option>
                                <option value="10 a.m.">10 a.m.</option>
                                <option value="11 a.m.">11 a.m.</option>
                                <option value="12 p.m.">12 p.m.</option>
                                <option value="1 p.m.">1 p.m.</option>
                                <option value="2 p.m.">2 p.m.</option>
                                <option value="3 p.m.">3 p.m.</option>
                                <option value="4 p.m.">4 p.m.</option>
                                <option value="5 p.m.">5 p.m.</option>
                                <option value="6 p.m.">6 p.m.</option>
                                <option value="7 p.m.">7 p.m.</option>
                            </select>
                        </div>

                        <div className={"form_container"}>
                            <label>Pick-up Time</label>
                            <select name="startTime" required={true} onChange={e => setAppointmentDetails({
                                ...appointmentDetails,
                                pickUp: e.target.value
                            })}>
                                <option value="8 a.m.">8 a.m.</option>
                                <option value="9 a.m.">9 a.m.</option>
                                <option value="10 a.m.">10 a.m.</option>
                                <option value="11 a.m.">11 a.m.</option>
                                <option value="12 p.m.">12 p.m.</option>
                                <option value="1 p.m.">1 p.m.</option>
                                <option value="2 p.m.">2 p.m.</option>
                                <option value="3 p.m.">3 p.m.</option>
                                <option value="4 p.m.">4 p.m.</option>
                                <option value="5 p.m.">5 p.m.</option>
                                <option value="6 p.m.">6 p.m.</option>
                                <option value="7 p.m.">7 p.m.</option>
                                <option value="8 p.m.">8 p.m.</option>
                                <option value="9 p.m.">9 p.m.</option>
                            </select>
                        </div>
                    </div>
                </div>
                <div className={"fileUpload_container"}>
                    <label>Choose a picture of your animal</label>
                    <input type="file" onChange={event => SaveFile(event)} accept=".jpg, .jpeg, .png" />
                </div>
                <div className={"form_container"}>
                    <label>Anything else we should know?</label>
                    <TextareaAutosize placeholder="Notes" onChange={e => setAppointmentDetails({
                        ...appointmentDetails,
                        notes: e.target.value
                    })}/>
                </div>
                <div className="button-container boarding_continue_to_payment">
                    <button type="submit">Continue to Payment</button>
                </div>
            </form>
        </div>
        <div className={"endError"}>
            {(error !== "") ? (<div className="error">{error}</div>) : ""}
        </div>

        <Footer/>
    </div>
);
}

export default Boarding;
