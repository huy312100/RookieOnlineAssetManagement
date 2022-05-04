import React from "react";
import "./ConfirmModal.css";
import { Modal } from 'react-bootstrap';
import CloseSharpOutlinedIcon from "@mui/icons-material/CloseSharp";
import Button from "@mui/material/Button";
const ConfirmModal = ({ title, isShow, onHide, children }) => {
  return (
    <Modal show={isShow} onHide={onHide} dialogClassName="containerModal">
      <div></div>
        <div className="header">
          <span>{title}</span>
        </div>
        <div className="body">
          <p>{children}</p>
        </div>
    </Modal>
  );
};

export default ConfirmModal;
