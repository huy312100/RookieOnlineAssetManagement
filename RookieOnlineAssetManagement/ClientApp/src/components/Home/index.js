import React, { lazy } from "react";
import { Route, Switch } from "react-router-dom";

import { HOME } from "../../Constants/pages";
import Home from "../Home";
import ListHome from "./List";


const HomePage = () => {
  return (
    <Switch>
      <Route exact path={HOME}>
        <ListHome />
      </Route>
    </Switch>
  );
};

export default HomePage;