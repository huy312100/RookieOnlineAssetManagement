import React from "react";
import { Modal } from "react-bootstrap";
import CloseSharpOutlinedIcon from "@mui/icons-material/CloseSharp";
import Button from "@mui/material/Button";
import "../../../shared-components/ConfirmModal.css";
const Info = ({ assignment, handleClose }) => {
    
    const getFormatDateTime=(date)=>{
        const DATE_OPTIONS = { day: 'numeric', month: 'numeric', year: 'numeric' };
        return new Date(date).toLocaleDateString('vi-VN', DATE_OPTIONS);
      }

    return (
        <>
            <Modal
                show={true}
                onHide={handleClose}
                dialogClassName="containerModalErr"
            >
                <div className="header">
                    Detailed Assignment Information
          <Button
                        sx={{ top: 22, right: 8, color: "#CF2338", position: "absolute" }}
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
                            <div className="col-4">Asset Code</div>
                            <div className="col-8 text-wrap text-break">{assignment.assetCode}</div>
                        </div>

                        <div className="row -intro-y mt-2">
                            <div className="col-4">Asset Name</div>
                            <div className="col-8 text-wrap text-break">{assignment.assetName}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Specification</div>
                            <div className="col-8 text-wrap text-break">{assignment.specification}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Assigned to</div>
                            <div className="col-8 text-wrap text-break">{assignment.assignedTo}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Assigned by</div>
                            <div className="col-8 text-wrap text-break">{assignment.assignedBy}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Assigned Date </div>
                            <div className="col-8 text-wrap text-break">
                                {
                                    getFormatDateTime(assignment.assignedDate)
                                }
                            </div>
                        </div>                      
                        <div className="row -intro-y mt-2">
                            <div className="col-4">State</div>
                            <div className="col-8 text-wrap text-break">{assignment.state}</div>
                        </div>
                        <div className="row -intro-y mt-2">
                            <div className="col-4">Note</div>
                            <div className="col-8 text-wrap text-break">{assignment.category}</div>
                        </div>
                    </div>
                </div>
            </Modal>
        </>
    );
};

export default Info;