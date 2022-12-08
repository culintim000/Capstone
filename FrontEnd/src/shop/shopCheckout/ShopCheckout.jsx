import {useLocation} from "react-router-dom";
import React, {useState} from 'react'
import {useCookies} from "react-cookie";
import axios from "axios";
import {wait} from "@testing-library/user-event/dist/utils";
import "./shopCheckout.css";

function ShopCheckout() {
    const location = useLocation();
    const [error, setError] = useState("");
    const [card, setCard] = useState({number: "", name: "", expMonth: "", expYear: "", cvv: "", postal: ""});
    const [cookies, setCookie] = useCookies(['user']);

    // console.log(location.state);


    function MakeReceipt() {
        let itemsList = [];
        // console.log(location.state.total);

        const listOfLines = location.state.items.lines;
        for (let i = 0; i < listOfLines.length; i++) {
            // console.log(listOfLines[i]);
            itemsList.push(<div className={"receipt_one_item"}><p>Name: {listOfLines[i].itemReturn.name}</p>
                                        <p>Quantity: {listOfLines[i].quantity}</p>
                                        <p>Price: {listOfLines[i].itemReturn.price * listOfLines[i].quantity}</p><hr/></div>);
        }

        return (
            <div className={"shop_receipt"}>
                {itemsList}
            </div>
        );
        // let totalCost = location.state.total;
        // return (
        //     <div className="details">
        //         <p className="detailsTitle">Shop Booking Details</p>
        //         <p className="detailsText">Total Cost: ${totalCost}</p>
        //     </div>
        // )
    }

    async function ConfirmPayment(event) {
        event.preventDefault();

        if (card.number.length !== 16) {
            setError("Please enter a valid 16 digit card number");
            return;
        }
        if (card.expMonth > 12 || card.expMonth < 1 || card.expMonth.length !== 2) {
            setError("Please enter a valid 2 digit month (eg: 05)");
            return;
        }
        if (card.expYear > 50 || card.expYear < 21 || card.expYear.length !== 2) {
            setError("Please enter a valid year");
            return;
        }
        if (card.cvv.length < 3 || card.cvv.length > 4) {
            setError("Please enter a valid CVV number");
            return;
        }
        if (card.postal.length !== 5) {
            setError("Please enter a valid postal code");
            return;
        }

        axios.post("http://localhost:8888/checkout-api/?cartid=" + location.state.items._id).then((res) => {
            if (res.status === 200) {
                setError("Success! Your payment has been processed");
            }
        }, (err) => {
            setError("Error processing payment");
            console.log(err);
        } );

        await wait(2000);
        window.location.href = "/shop";
    }

    if (cookies.token === undefined) {
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }

    return (
        <div>
            <h1>Shop Checkout</h1>
            <div className={"outer_receipt"}>
                <MakeReceipt/>
            </div>

            <h2>Total: ${location.state.total}</h2>
            <div className="form_checkout">
                {(error !== "") ? (<div className="error">{error}</div>) : ""}
                <form onSubmit={ConfirmPayment}>
                    <div className="input-container_checkout">
                        <label>Card Number </label>
                        <input type="number" maxLength={4} name="cardNum1" id="cardNum1"
                               onChange={e => setCard({...card, number: e.target.value.slice(0, 16)})}
                               value={card.number} required/>
                    </div>
                    <div className="input-container_checkout">
                        <label>Name on Card </label>
                        <input type="text" name="cardName" onChange={e => setCard({...card, name: e.target.value})}
                               value={card.name} required/>
                    </div>
                    <div className="dateAndCvv">
                        <div className="input-container_checkout_small">
                            <label>Expiration Date </label>
                            <div className="expDate">
                                <input type="number" name="name"
                                       onChange={e => setCard({...card, expMonth: e.target.value.slice(0, 2)})}
                                       value={card.expMonth} required/>
                                {/*<h2 className="slash">/</h2>*/}
                                <input type="number" name="name"
                                       onChange={e => setCard({...card, expYear: e.target.value.slice(0, 2)})}
                                       value={card.expYear} required/>
                            </div>
                        </div>
                        <div className="input-container_checkout_small">
                            <label>CVV </label>
                            <input type="number" className="cvv" name="cvv"
                                   onChange={e => setCard({...card, cvv: e.target.value.slice(0, 4)})} value={card.cvv}
                                   required/>
                        </div>

                        <div className="input-container_checkout_small">
                            <label>Postal Code</label>
                            <input type="number" className="postal" name="postal"
                                   onChange={e => setCard({...card, postal: e.target.value.slice(0, 5)})}
                                   value={card.postal} required/>
                        </div>
                    </div>
                    <div className="button-container input-container_checkout button">
                        <button type="submit">Confirm Payment</button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default ShopCheckout;