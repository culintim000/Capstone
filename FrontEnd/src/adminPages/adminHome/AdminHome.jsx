import React, {useEffect, useState} from 'react';
import Navbar from "../adminNavBar/AdminNavbar";
import {UserFeature} from "../index";
import "./adminHome.css";
import {decodeToken} from "react-jwt";
import {useCookies} from "react-cookie";

function AdminHome() {
    const [user, setUser] = useState("");
    const [cookies, setCookie] = useCookies(['user']);

    async function Search(e) {
        e.preventDefault();

        const search = document.getElementById("search").value;

        const requestOptions = {
            method: 'POST'
        };

        const response = await fetch(' http://localhost:8888/auth-service/auth/getUser?email=' + search, requestOptions)

        if (response.status === 200) {
            const data = await response.json();
            // console.log(data);
            setUser(data);
        } else setUser("User not found");

    }

    const ShowUsers = () => {
        // console.log(user);

        if (user === "") {
            return (
                <div>
                    <h2 id={"searchToStart"}>Search for a user to get started</h2>
                </div>
            )
        }
        if (user === "User not found") {
            return (
                <div>
                    <h2 id={"notFound"}>User not found</h2>
                </div>
            );
        }

        return (
            <div className={"userFeature"}>
                <UserFeature name={user.name} email={user.email} phone={user.phone} isWorker={user.isWorker}/>
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
            <Navbar/>

            <div className={"mainContent"}>
                <div className={"searchDiv"}>
                    <form onSubmit={Search}>
                        <input id={"search"} type="search" placeholder={"Search"}/>
                        <button className={"buttonSearch"} type="submit"/>
                    </form>
                </div>

                <ShowUsers/>
            </div>
        </div>
    );
}

export default AdminHome;