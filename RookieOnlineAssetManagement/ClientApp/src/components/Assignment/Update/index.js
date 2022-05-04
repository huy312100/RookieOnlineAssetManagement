import React from "react";

import EditAssignmentFormContainer from "../EditAssignmentForm";

const EditAssignmentContainer = () => {
  return (
    <div className="ml-5 container">
      <br />
      <h4 className="fw-bold text-danger">Edit Assignment</h4>
      <div className="row mt-2">
        <EditAssignmentFormContainer />
      </div>
    </div>
  );
};

export default EditAssignmentContainer;