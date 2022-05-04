import React from "react";
import { Route, Switch } from "react-router-dom";

import { REPORT } from "../../Constants/pages";
import ListReport from "./List";

const Report = () => {
  return (
    <Switch>
      <Route exact path={REPORT}>
        <ListReport />
      </Route>
    </Switch>
  );
};

export default Report;
