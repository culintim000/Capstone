import React, {useState, useEffect} from 'react';
import axios from "axios";
import "./itemFeature.css";

const ItemFeature = ({id, name, description, price}) => {
    const [itemDeleted, setItemDeleted] = useState(false);

    const DeleteItem = (e) => {
      e.preventDefault();
        axios.delete("http://localhost:5052/" + id).then((res) => {
            setItemDeleted(true);
        }, (err) => {
            console.log(err);
        });

        axios.delete("http://localhost:5261/pic/deleteItem?id=" + id).then((res) => {},
            (err) => {console.log(err)});
    }

    return (
        <div className={"outer_admin_item_feature"}>
            <div hidden={itemDeleted} className={"item_feature_admin"}>
                <p>ID: {id}</p>
                <p>Name: {name}</p>
                <p>description: {description}</p>
                <p>price: {price}</p>
                <div className={"item_feature_admin_btn"}>
                    <button onClick={DeleteItem}>Delete</button>
                </div>

            </div>
        </div>

    )
}

export default ItemFeature;