import React from "react";
import "./ConfirmModal.css";
import { Modal } from 'react-bootstrap';
import CloseSharpOutlinedIcon from "@mui/icons-material/CloseSharp";
import Button from "@mui/material/Button";
const ErorrModel = ({ title, isShow, onHide, children }) => {
    return (
        <Modal show={isShow} onHide={onHide} className="containerModal">
            <div></div>
            <div className="header">
                <span>{title}</span>
                <Button
                    sx={{ top: 20, right: 30, color: "#CF2338", position: "absolute" }}
                    onClick={onHide}
                >
                    <CloseSharpOutlinedIcon
                        sx={{
                            border: 3,
                        }}
                    />
                </Button>
            </div>
            <div className="body">
                <p>{children}</p>
            </div>
        </Modal>
    );
};

export default ErorrModel;
