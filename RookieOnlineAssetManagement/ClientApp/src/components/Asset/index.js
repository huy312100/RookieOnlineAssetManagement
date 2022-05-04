import React, { lazy } from "react";
import { Route, Switch } from "react-router-dom";

import { ASSET,CREATE_ASSET,EDIT_ASSET } from "../../Constants/pages";
import CreateAsset from "./Create";
import ListAsset from "./List";
import EditAsset from "./Update";

const Asset = () => {
  return (
    <Switch>
      <Route exact path={ASSET}>
        <ListAsset />
      </Route>
      
      <Route exact path={CREATE_ASSET}>
        <CreateAsset />
      </Route>

      <Route exact path={EDIT_ASSET}>
        <EditAsset />
      </Route>
    </Switch>
  );
};

export default Asset;
