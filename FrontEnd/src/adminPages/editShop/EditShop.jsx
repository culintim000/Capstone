//import react
import React, {useState, useEffect} from 'react';
import axios from "axios";
import {wait} from "@testing-library/user-event/dist/utils";
import {ItemFeature} from "../index";
import BoardingFeature from "../../userPages/boardingFeature/BoardingFeature";
import Navbar from "../adminNavBar/AdminNavbar";
import "./editShop.css";
import {decodeToken} from "react-jwt";
import {useCookies} from "react-cookie";

function EditShop() {
    const [item, setItem] = useState({name: "", description: "", price: ""});
    const [pic, setPic] = useState(undefined);
    const [error, setError] = useState("");
    const [error2, setError2] = useState("");
    const [searchedItems, setSearchedItems] = useState(undefined);
    const [cookies, setCookie] = useCookies(['user']);

    async function AddItem(e){
        e.preventDefault();

        axios.post("http://localhost:8888/inventory-api/", item).then((res) => {
            if (res.status === 200){
                if (pic !== undefined){
                    const formData = new FormData();
                    formData.append("file", pic);

                    const splitted = pic.name.split(".");
                    const extension = splitted[splitted.length - 1];

                    try {
                        axios.post("http://localhost:5261/pic/setItem?name=" + res.data.value +"."+ extension, formData).then((res) => {
                            if (res.status !== 200) {
                                setError("Image upload failed");
                            }
                            window.location.href = "/admin/shop";
                        });
                    }catch (e) {
                        console.log(e);
                    }
                }
                setItem({name: "", description: "", price: ""});
                setPic(undefined);
            }
        }, (err) => {
            console.log(err);
            setError("Error adding item");
        });
    }

    async function SearchForItem(e) {
        e.preventDefault();

        const search = document.getElementById("search").value;

        axios.get("http://localhost:8888/inventory-api/search/" + search).then(async (res) => {
            if (res.status === 200) {
                setSearchedItems(res.data.value);
            }
        }, (err) => {
            console.log(err);
            setError2("Error searching for item");
        });
    }

    const ShowItems = () => {
        if (searchedItems === undefined) {
            return (
                <div>
                    <h2 id={"searchItemsToStart"}>Search for the item you want to remove</h2>
                </div>
            )
        }

        const itemDisplay = [];
        for (let i = 0; i < searchedItems.length; i++) {
            itemDisplay.push(<ItemFeature name={searchedItems[i].name} description={searchedItems[i].description}
                                            price={searchedItems[i].price} id={searchedItems[i].id}/>)
        }

        return (
            <div>
                {itemDisplay}
            </div>
        );
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

    if (decodedToken.Role !== "Admin"){
        console.log("Role is not Admin");
        return (
            <div className={"sign"}>
                <h2>Please </h2>
                <h2 className={"login"} style={{color: "#ADD8E6"}}><a href="/sign-in">Sign In</a></h2>
            </div>
        )
    }

    return (
        <div>
            <div className={"add_edit_shop"}>
                <Navbar/>
                <h2>Add to Shop</h2>
                {(error !== "") ? (<div className="error">{error}</div>) : ""}
                <div className={"add_edit_shop_form"}>
                    <form onSubmit={AddItem} >
                        <div className={"form_container"}>
                            <label> Name: </label>
                            <input type="text" name="name" placeholder="Name" required={true}
                                   onChange={e => setItem({...item, name: e.target.value})}
                                   value={item.name}/>
                        </div>
                        <div className={"form_container"}>
                            <label> Description: </label>
                            <input type="text" name="description" placeholder="Description" required={true}
                                   onChange={e => setItem({...item, description: e.target.value})}
                                   value={item.description}/>
                        </div>
                        <div className={"form_container"}>
                            <label> Price: </label>
                            <input type="number" name="price" placeholder="Price" required={true}
                                   onChange={e => setItem({...item, price: e.target.value})}
                                   value={item.price}/>
                        </div>
                        <div className={"fileUpload_container"}>
                            <label> Image: </label>
                            <input type="file" name="image" placeholder="Image" required={true}
                                   onChange={e => setPic(e.target.files[0])} accept=".jpg, .jpeg, .png"/>
                        </div>

                        <div className={"btn_edit_shop"}>
                            <button type="submit">Add</button>
                        </div>

                    </form>
                </div>

            </div>
            <hr/>
            <div>
                <h2>Remove from Shop</h2>
                {(error2 !== "") ? (<div className="error">{error2}</div>) : ""}
                <div className={"searchDiv"}>
                    <form onSubmit={SearchForItem}>
                        <input id={"search"} type="search" placeholder={"Search"}/>
                        <button className={"buttonSearch"} type="submit"/>
                    </form>
                </div>
                {ShowItems()}
            </div>
        </div>

    );
}

export default EditShop;