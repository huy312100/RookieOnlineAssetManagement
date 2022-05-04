import React from "react";

import CreateAssignmentFormContainer from "../CreateAssignmentForm";

const CreateAssignmentContainer = () => {
  return (
    <div className="ml-5 container">
      <br />
      <h4 className="fw-bold text-danger">Create New Assignment</h4>
      <div className="row mt-2">
        <CreateAssignmentFormContainer />
      </div>
    </div>
  );
};

export default CreateAssignmentContainer;