import React, { useState } from "react";

import UserFormContainer from "../CreateUserForm";

const CreateUserContainer = () => {
  return (
    <div className="ml-5 container">
      <br />
      <h4 className="fw-bold text-danger">Create New User</h4>
      <div className="row mt-2">
        <UserFormContainer />
      </div>
    </div>
  );
};

export default CreateUserContainer;
