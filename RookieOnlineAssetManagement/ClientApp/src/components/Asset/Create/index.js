import React from "react";
import AssetFormContainer from "../CreateAssetForm";

const CreateAssetContainer = () => {
  return (
    <div className="ml-5 container">
      <br />
      <h4 className="fw-bold text-danger">Create New Asset</h4>
      <div className="row mt-2">
        <AssetFormContainer />
      </div>
    </div>
  );
};

export default CreateAssetContainer;