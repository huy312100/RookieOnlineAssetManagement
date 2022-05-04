import React, { lazy } from "react";
import { Route, Switch } from "react-router-dom";

import { USER, CREATE_USER, EDIT_USER } from "../../Constants/pages";
import ListUser from "./List";
import CreateUser from "./Create";
import EditUser from "./Update";

const User = () => {
  return (
    <Switch>
      <Route exact path={USER}>
        <ListUser />
      </Route>
      <Route exact path={CREATE_USER}>
        <CreateUser />
      </Route>
      <Route exact path={CREATE_USER}>
        <CreateUser />
      </Route>
      <Route exact path={EDIT_USER}>
        <EditUser />
      </Route>
    </Switch>
  );
};

export default User;
