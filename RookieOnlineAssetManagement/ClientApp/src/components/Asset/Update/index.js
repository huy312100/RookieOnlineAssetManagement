import React from "react";

import EditAssetFormContainer from "../EditAssetForm";

const EditAssetContainer = () => {
  return (
    <div className="ml-5 container">
      <br />
      <h4 className="fw-bold text-danger">Edit Asset</h4>
      <div className="row">
        <EditAssetFormContainer />
      </div>
    </div>
  );
};

export default EditAssetContainer;
