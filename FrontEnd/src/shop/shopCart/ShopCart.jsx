import React, {useState, useEffect} from 'react';
import "./shopCart.css";
import {decodeToken} from "react-jwt";
import axios from "axios";
import {useCookies} from "react-cookie";
import {wait} from "@testing-library/user-event/dist/utils";
import {ItemFeature} from "../index";
import CartItemFeature from "../cartItemFeature/CartItemFeature";
import Navbar from "../../userPages/userNavbar/Navbar";
import {useNavigate} from "react-router-dom";

function ShopCart(){
    const [userId, setUserId] = useState(undefined);
    const [cookies, setCookie] = useCookies(['user']);
    const [items, setItems] = useState(undefined);
    const [error, setError] = useState("");
    const [total, setTotal] = useState(0);
    const [hideCheckOut, setHideCheckOut] = useState(true);
    const navigate = useNavigate();


    useEffect(() => {
        async function FetchUserId(){
            const decodedToken = decodeToken(cookies.token);

            axios.get("http://localhost:8888/bookingservice/book/getUserId?email=" + decodedToken.Email).then((res) => {
                if (res.status === 200) {
                    // console.log(res.data);
                    setUserId(res.data);
                }
            }, (err) => {
                setError("Error fetching user id");
                console.log(err);
            });
        }
        FetchUserId();
    }, []);


    useEffect(() => {
        async function FetchItems() {
            axios.get("http://localhost:8888/cart-api/all?userid=" + userId).then((res) => {
                if (res.status === 200) {
                    if (res.data.value !== "Exception has been thrown by the target of an invocation."){
                        setItems(res.data.value);
                        // console.log(res)
                    }
                }
            }, (err) => {
                setError("Error fetching items");
                console.log(err);
            });
        }

        FetchItems();
    }, [userId]);


    const ShowItems = () => {
        // console.log(items);
        if (items === undefined) {
            return (
                <div>
                    <p>Loading...</p>
                </div>
            );
        } else {
            const itemsList = [];
            for (let i = 0; i < items.lines.length; i++) {
                itemsList.push(<CartItemFeature name={items.lines[i].itemReturn.name} description={items.lines[i].itemReturn.description}
                                            price={items.lines[i].itemReturn.price} id={items.lines[i].itemReturn._id} userId={userId}
                                                amount={items.lines[i].quantity} index={i}/>);
            }
            return (
                <div className={"items_display"}>
                    {itemsList}
                </div>
            );
        }
    }

    function GetTotal(){
        if (items === undefined){
            return;
        } else {
            // let total = 0;

            axios.get("http://localhost:8888/cart-api/total?userid=" + userId + "&item=all").then((res) => {
                if (res.status === 200) {
                    if (res.data.value !== "No cart for user"){
                        setTotal(res.data.value);
                        setHideCheckOut(false);
                    }
                }
            }, (err) => {
                setError("Error fetching total");
                console.log(err);
            });

            if(total === 0){
                return;
            }
            return (
                <div className={"total"}>
                    <p>Total: {total}</p>
                </div>
            );
        }
    }

    function NoItems(){
        if (items === undefined || items.lines.length === 0){
            return (
                <div className={"no_items"}>
                    <p>No items in cart</p>
                </div>
            );
        } else {
            return;
        }
    }

    function GoToCheckout(){
        navigate("/shop/checkout", {state: {items: items, total: total}});
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
            <div className={"gradient__bg__logInSignUp"}>
                <Navbar/>
            </div>
            {(error !== "") ? (<div className="error">{error}</div>) : ""}
            <NoItems/>
            <ShowItems/>
            <GetTotal/>
            <div className={"checkout_btn"} hidden={hideCheckOut}>
                <button onClick={GoToCheckout}>Checkout</button>
            </div>
        </div>
    );
}

export default ShopCart;