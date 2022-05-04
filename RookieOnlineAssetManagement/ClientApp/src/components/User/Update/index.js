
import React, { useState } from "react";

import EditUserFormContainer from "../EditUserForm";

const CreateUserContainer = () => {

  return (
    <div className='ml-5 container'>
        <br/>
        <h4 className="fw-bold text-danger">Edit User</h4>
      <div className='row'>
        <EditUserFormContainer />

      </div>

    </div>
  );
};

export default CreateUserContainer;