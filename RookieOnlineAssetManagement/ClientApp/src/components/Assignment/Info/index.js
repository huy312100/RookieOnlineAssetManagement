import React from "react";
import { Modal } from "react-bootstrap";
import CloseSharpOutlinedIcon from "@mui/icons-material/CloseSharp";
import Button from "@mui/material/Button";
import "../../../shared-components/ConfirmModal.css";
import { getFormatDateTime } from "../../../utils/Date";
const Info = ({ assignment, handleClose }) => {
  return (
    <>
      <Modal
        show={true}
        onHide={handleClose}
        dialogClassName="containerModalInfor"
      >
        <div className="header headerInfo">
          Detailed Assignment Information
          <Button
            sx={{ right: 0, color: "#CF2338", float: "right" }}
            onClick={handleClose}
          >
            <CloseSharpOutlinedIcon
              sx={{
                border: 3,
              }}
            />
          </Button>
        </div>
        <div className="body bodyInfo">
          <div className="container-fluid">
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Asset Code:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {assignment.assetCode}
              </div>
            </div>

            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Asset Name:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {assignment.assetName}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Specification:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {assignment.specification}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Assigned to:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {assignment.assignedTo}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Assigned by:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {assignment.assignedBy}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Assigned Date:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {getFormatDateTime(assignment.assignedDate)}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">State:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {assignment.assignmentState}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Note:</div>
              <div className="col-sm-9 col-md-9 col-lg-9 text-wrap text-break">
                {assignment.note}
              </div>
            </div>
          </div>
        </div>
      </Modal>
    </>
  );
};

export default Info;
