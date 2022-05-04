import React from "react";
import { Modal } from "react-bootstrap";
import CloseSharpOutlinedIcon from "@mui/icons-material/CloseSharp";
import Button from "@mui/material/Button";
import "../../../shared-components/ConfirmModal.css";
import {getFormatDateTime} from "../../../utils/Date";

const Info = ({ user, handleClose }) => {
    return (
        <>
            <Modal
                show={true}
                onHide={handleClose}
                dialogClassName="containerModalErr"
            >
                <div className="header">
                    Detailed User Information
          <Button
                        sx={{ top: 22, right: 30, color: "#CF2338", position: "absolute" }}
                        onClick={handleClose}
                    >
                        <CloseSharpOutlinedIcon
                            sx={{
                                border: 3,
                            }}
                        />
                    </Button>
                </div>
                <div className="body">
                    <div className="container-fluid">
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Staff Code:</div>
                            <div className="col-8 text-wrap text-break">{user.staffCode}</div>
                        </div>

                        <div className="row -intro-y mt-2">
                            <div className="col-4">Full Name:</div>
                            <div className="col-8 text-wrap text-break">{user.fullName}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Username:</div>
                            <div className="col-8 text-wrap text-break">{user.userName}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Date of Birth:</div>
                            <div className="col-8 text-wrap text-break">
                                {
                                    getFormatDateTime(user.birthDay)
                                }
                            </div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Gender:</div>
                            <div className="col-8 text-wrap text-break">
                                {
                                    (user.gender === 1 ? "Female" : "Male")
                                }
                            </div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Joined Date:</div>
                            <div className="col-8 text-wrap text-break">{user.joinedDate}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Type:</div>
                            <div className="col-8 text-wrap text-break">{user.type}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Location:</div>
                            <div className="col-8 text-wrap text-break">{user.location.toUpperCase()}</div>
                        </div>
                    </div>
                </div>
            </Modal>
        </>
    );
};

export default Info;
