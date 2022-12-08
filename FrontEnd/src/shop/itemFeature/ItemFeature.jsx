import React, {useState, useEffect} from 'react';
import "./itemFeature.css";
import axios from "axios";

const BoardingFeature = ({id, name, description, price, userId}) => {
    const [img, setImg] = React.useState();
    const [amount, setAmount] = React.useState(1);
    const [error, setError] = React.useState("");

    useEffect(() => {
        const getImg = async () => {
            const res = await fetch("http://localhost:5261/pic/getItem?name=" + id);

            if (res.status === 200) {
                const imageBlob = await res.blob();

                let base64data = undefined;
                var reader = new FileReader();
                reader.readAsDataURL(imageBlob);
                reader.onloadend = function () {
                    base64data = reader.result;
                    setImg(base64data);
                }
            } else setImg(undefined);
        }
        getImg();
    }, []);

    const AddToCart = (e) => {
        e.preventDefault();
        if (isNaN(amount) || amount < 1){
            setError("Amount must be a number greater than 0");
            return;
        }
        setError("");
        axios.post("http://localhost:8888/cart-api/?itemid=" + id + "&userid=" + userId + "&amount=" + amount).then((res) => {
            // console.log("res")
            // console.log(res);
            if (res.status === 200){
                setError("Item added to cart");
            }
        }, (err) => {
            console.log(err);
            setError("Error adding item to cart");
        });
    }

    function SetImage() {
        if (img !== undefined) {
            return (
                <div className={"itemImage"}>
                    <img src={img} alt={"test"}/>
                </div>
            );
        }
    }

    return (
        <div className="itemFeature">
            {(error !== "") ? (<div className="error">{error}</div>) : ""}
            <div className={"outer_item_image"}>
                {SetImage()}
            </div>
            <div className={"item_name"}>
                <p>{name}</p>
            </div>
            <div className={"item_description"}>
                <p id={"text"}>{description}</p>
            </div>

            <div className={"item_price"}>
                <p>${price}</p>
            </div>
            <div className={"add_to_cart_container"}>
                <div className={"add_to_cart_button"}>
                    <button className={"item_button"} onClick={event => AddToCart(event)}>Add to cart</button>
                </div>
                <div className={"add_to_cart_quantity"}>
                    <input type={"number"} min={1} max={10} onChange={e => setAmount(parseInt(e.target.value.slice(0, 2)))} value={amount}/>
                </div>
            </div>

        </div>
    );
}

export default BoardingFeature;