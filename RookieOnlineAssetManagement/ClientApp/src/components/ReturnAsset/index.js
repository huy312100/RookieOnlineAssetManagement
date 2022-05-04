import React, { lazy } from "react";
import { Route, Switch } from "react-router-dom";

import { RETURNASSET } from "../../Constants/pages";
import ListReturnAsset from "./List";

const ReturnAsset = () => {
  return (
    <Switch>
      <Route exact path={RETURNASSET}>
        <ListReturnAsset />
      </Route>

    </Switch>
  );
};

export default ReturnAsset;
