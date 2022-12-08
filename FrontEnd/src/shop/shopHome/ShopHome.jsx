import React, {useState, useEffect} from 'react';
import "./shopHome.css";
import axios from "axios";
import {ItemFeature} from "../index";
import Navbar from "../../userPages/userNavbar/Navbar";
import {decodeToken} from "react-jwt";
import {useCookies} from "react-cookie";

function ShopHome() {
    const [items, setItems] = useState(undefined);
    const [error, setError] = useState("");
    const [cookies, setCookie] = useCookies(['user']);
    const [userId, setUserId] = useState(undefined);

    useEffect(() => {
        const decodedToken = decodeToken(cookies.token);

        async function FetchItems() {
            axios.get("http://localhost:8888/inventory-api/").then((res) => {
                if (res.status === 200) {
                    setItems(res.data.value);
                }
            }, (err) => {
                setError("Error fetching items");
                console.log(err);
            });
        }

        async function FetchUserId(){
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

        FetchItems();
        FetchUserId();
    }, []);

    const ShowItems = () => {
        if (items === undefined) {
            return (
                <div>
                    <p>Loading...</p>
                </div>
            );
        } else {
            const itemsList = [];

            for (let i = 0; i < items.length; i++) {
                itemsList.push(<ItemFeature name={items[i].name} description={items[i].description}
                                            price={items[i].price} id={items[i].id} userId={userId}/>);
            }
            return (
                <div className={"items_display"}>
                    {itemsList}
                </div>
            );
        }
    }

    const SearchForItem = (e) => {
        e.preventDefault();

        const search = document.getElementById("search").value;

        if (search === "") {
            window.location.reload();
        }
        else {
            axios.get("http://localhost:8888/inventory-api/search/" + search).then(async (res) => {
                if (res.status === 200) {
                    console.log(res.data.value);
                    if (res.data.value.length === 0){
                        setError("No items found");
                    }
                    else {
                        setError("");
                        setItems(res.data.value);
                    }
                }
                else {
                    setError("No Items Found");
                }
            }, (err) => {
                console.log(err);
                setError("Error searching for item");
            });
        }

    }

    if (cookies.token === undefined) {
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }

    // console.log(items);
    return (
        <div>
            <div className={"gradient__bg__logInSignUp"}>
                <Navbar/>
            </div>
            {(error !== "") ? (<div className="error">{error}</div>) : ""}
            <div className={"search_cart"}>
                <div className={"searchDiv"}>
                    <form onSubmit={SearchForItem}>
                        <input id={"search"} type="search" placeholder={"Search"}/>
                        <button className={"buttonSearch"} type="submit"/>
                    </form>
                </div>
                <div className={"cart_link"}>
                    <p><a href="/shop/cart">Cart</a></p>
                </div>
            </div>
            <ShowItems/>
        </div>
    );
}

export default ShopHome;