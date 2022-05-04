import React from "react";
import { Modal } from "react-bootstrap";
import CloseSharpOutlinedIcon from "@mui/icons-material/CloseSharp";
import Button from "@mui/material/Button";
import "./info.css";
import TableInfo from "./TableInfo";
import {getFormatDateTime} from "../../../utils/Date";

const Info = ({ asset, handleClose }) => {

  return (
    <>
      <Modal
        show={true}
        onHide={handleClose}
        dialogClassName="containerModalInfo"
      >
        <div className="header headerInfo">
          Detailed Asset Information
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
              <div className="col-sm-6 col-md-6 col-lg-5 text-wrap text-break">
                {asset.assetCode}
              </div>
            </div>

            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Asset Name:</div>
              <div
                className="col-sm-6 col-md-6 col-lg-5 text-wrap text-break"
              >
                {asset.assetName}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Category:</div>
              <div
                className="col-sm-6 col-md-6 col-lg-5 text-wrap text-break"
              >
                {asset.category}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Installed Date:</div>
              <div className="col-sm-6 col-md-6 col-lg-5 text-wrap text-break">
                {
                  getFormatDateTime(asset.installedDate)
                }
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">State:</div>
              <div className="col-sm-6 col-md-6 col-lg-5 text-wrap text-break">{asset.state}</div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Location:</div>
              <div className="col-sm-6 col-md-6 col-lg-5 text-wrap text-break">{asset.location.toUpperCase()}</div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">Specification:</div>
              <div className="col-sm-6 col-md-6 col-lg-5 text-wrap text-break">
                {asset.specification}
              </div>
            </div>
            <div className="row -intro-y mt-2">
              <div className="col-sm-3 col-md-3 col-lg-3">History:</div>
              <div className="col-md-12 col-lg-9">
                <TableInfo assetId={asset.id}/>
              </div>
            </div>
          </div>
        </div>
      </Modal>
    </>
  );
};

export default Info;
