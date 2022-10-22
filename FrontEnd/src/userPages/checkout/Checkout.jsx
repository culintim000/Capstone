import React, {useState} from 'react'
import {useLocation} from 'react-router-dom';
import {type} from "@testing-library/user-event/dist/type";
import './checkout.css';

function Checkout() {
    const location = useLocation();
    const [error, setError] = useState("");
    const [card, setCard] = useState({number: "", name: "", expMonth: "", expYear: "", cvv: "", postal: ""});

    let difference = location.state.appointmentDetails.endDate.getTime() - location.state.appointmentDetails.startDate.getTime();

    const dropOffTime = TimeChange(location.state.appointmentDetails.dropOff);
    const pickUpTime = TimeChange(location.state.appointmentDetails.pickUp);


    function TimeChange(passedTime) {
        if (passedTime.includes("p.m.") && !passedTime.includes("12")) {
            let result = passedTime.split(" ");
            return parseInt(result[0]) + 12;
        }
        else {
            let resultTwo = passedTime.split(" ");
            return parseInt(resultTwo[0]);
        }
    }

    function MakeReceipt() {
        if (location.state.appointmentDetails.bookingType === "Day Care"){
            let totalDays = Math.ceil(difference / (1000 * 3600 * 24)) + 1;
            const totalTime = pickUpTime - dropOffTime;
            const totalCost = totalTime * totalDays * location.state.appointmentDetails.price;

            return (
                <div className="details">
                    <p className="detailsTitle">{location.state.appointmentDetails.bookingType} Booking Details</p>
                    <p className="detail" >Start Date: {location.state.appointmentDetails.startDate.toDateString()}</p>
                    <p className="detail" >End Date: {location.state.appointmentDetails.endDate.toDateString()}</p>
                    <p className="detail" >Total Days: {totalDays}</p>
                    <p className="detail" >Drop Off Time: {location.state.appointmentDetails.dropOff}</p>
                    <p className="detail" >Pick Up Time: {location.state.appointmentDetails.pickUp}</p>
                    <p className="detail" >Total Time Per Day: {totalTime}</p>
                    <p className="detail" >Total Cost: ${totalCost}</p>
                </div>
            )
        }
        else {
            let totalNights = Math.ceil(difference / (1000 * 3600 * 24));
            const totalCost = totalNights * location.state.appointmentDetails.price;
            return (
                <div>
                    <p className="detailsTitle">{location.state.appointmentDetails.bookingType} Booking Details</p>
                    <p className="detail" >Start Date: {location.state.appointmentDetails.startDate.toDateString()}</p>
                    <p className="detail" >End Date: {location.state.appointmentDetails.endDate.toDateString()}</p>
                    <p className="detail" >Drop Off Time: {location.state.appointmentDetails.dropOff}</p>
                    <p className="detail" >Pick Up Time: {location.state.appointmentDetails.pickUp}</p>
                    <p className="detail" >Total Nights: {totalNights}</p>
                    <p className="detail" >Total Cost: ${totalCost}</p>

                </div>
            )
        }
    }

    function ConfirmPayment(event){
        event.preventDefault();

        if (card.number.length !== 16){
            setError("Please enter a valid 16 digit card number");
            return;
        }
        if (card.expMonth > 12 || card.expMonth < 1 || card.expMonth.length !== 2){
            setError("Please enter a valid 2 digit month");
            return;
        }
        if (card.expYear > 50 || card.expYear < 21 || card.expYear.length !== 2){
            setError("Please enter a valid year");
            return;
        }
        if (card.cvv.length < 3 || card.cvv.length > 4){
            setError("Please enter a valid CVV number");
            return;
        }
        if (card.postal.length !== 5){
            setError("Please enter a valid postal code");
            return;
        }
        setError("Payment Successful");
    }

    return (
        <div className="outer">
            <div className="receipt">{MakeReceipt()}</div>
            <div className="form">
                {(error !== "") ? (<div className="error">{error}</div>) : ""}
                <form onSubmit={ConfirmPayment}>
                    <div className="input-container" >
                        <label>Card Number </label>
                        <input type="number" maxLength={4} name="cardNum1" id="cardNum1" onChange={e => setCard({...card, number: e.target.value.slice(0, 16)})} value={card.number} required />
                        </div>
                    <div className="input-container">
                        <label>Name on Card </label>
                        <input type="text" name="cardName" onChange={e => setCard({...card, name: e.target.value})} value={card.name} required />
                    </div>
                    <div className="dateAndCvv">
                        <div className="input-container">
                            <label>Expiration Date </label>
                            <div className="expDate">
                                <input type="number" name="name" onChange={e => setCard({...card, expMonth: e.target.value.slice(0, 2)})} value={card.expMonth} required />
                                <h2 className="slash">/</h2>
                                <input type="number" name="name" onChange={e => setCard({...card, expYear: e.target.value.slice(0, 2)})} value={card.expYear} required />
                            </div>
                        </div>
                        <div className="input-container">
                            <label>CVV </label>
                            <input type="number" className="cvv" name="cvv" onChange={e => setCard({...card, cvv: e.target.value.slice(0, 4)})} value={card.cvv} required />
                        </div>

                        <div className="input-container">
                            <label>Postal Code</label>
                            <input type="number" className="postal" name="postal" onChange={e => setCard({...card, postal: e.target.value.slice(0, 5)})} value={card.postal} required />
                        </div>
                    </div>
                    <div className="button-container input-container button">
                        <button type="submit" >Confirm Payment</button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default Checkout