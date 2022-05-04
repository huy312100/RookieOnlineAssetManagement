import React, { lazy } from "react";
import { Route, Switch } from "react-router-dom";

import { ASSIGNMENT, CREATE_ASSIGNMENT,EDIT_ASSIGNMENT } from "../../Constants/pages";
import CreateAssignment from "./Create";
import ListAssignment from "./List";
import EditAssignment from "./Update";

const Assignment = () => {
  return (
    <Switch>
      <Route exact path={ASSIGNMENT}>
        <ListAssignment />
      </Route>
      <Route exact path={CREATE_ASSIGNMENT}>
        <CreateAssignment />
      </Route>
      <Route exact path={EDIT_ASSIGNMENT}>
        <EditAssignment />
      </Route>

    </Switch>
  );
};

export default Assignment;
