import React, {useState, useEffect} from 'react';
import axios from "axios";

function CartItemFeature ({id, name, description, price, userId, amount, index}) {
    const [img, setImg] = React.useState();
    const [hide, setHide] = React.useState(false);

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


    function SetImage() {
        if (img !== undefined) {
            return (
                <div className={"itemImage"}>
                    <img src={img} alt={"test"}/>
                </div>
            );
        }
    }

    function RemoveFromCart(){
        axios.delete("http://localhost:8888/cart-api/remove?item=" + index + "&userid=" + userId).then((res) => {
            if (res.status === 200){
                window.location.reload();
            }
        }, (err) => {
            console.log(err);
        });
    }

    return (
        <div className="itemFeature" hidden={hide}>
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
                <p>Total: ${price * amount}</p>
            </div>
            <div>
                <p>Amount: {amount}</p>
            </div>
            <div className={"add_to_cart_button"}>
                <button onClick={RemoveFromCart}>Remove</button>
            </div>

        </div>
    );
}

export default CartItemFeature;