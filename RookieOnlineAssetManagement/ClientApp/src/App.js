import axios from "axios";
import InLineLoader from "./shared-components/InlineLoader";
import "bootstrap/dist/css/bootstrap.min.css";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import React, { lazy, Suspense, useEffect } from "react";
import Sidebar from "./components/Sidebar/index";
import { CssBaseline, Grid } from "@mui/material";
import Header from "./app/layout/Header";
import { HOME, USER, ASSET, ASSIGNMENT, REPORT, RETURNASSET } from "./Constants/pages.js";
import Home from "./components/Home";
import User from "./components/User";
import Asset from "./components/Asset";
import Assignment from "./components/Assignment";
import Report from "./components/Report";
import ReturnAsset from "./components/ReturnAsset";
axios.interceptors.request.use((config) => {
  return config;
});
axios.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (401 === error.response.status) {
      window.location.href =
        "/Identity/Account/Login?returnUrl=" + window.location.pathname;
    } else {
      return Promise.reject(error);
    }
  }
);

axios.get("/api/users/1").then((response) => console.table(response.data));

const SusspenseLoading = ({ children }) => (
  <Suspense fallback={<InLineLoader />}>{children}</Suspense>
);

function App() {
  useEffect(() => {
    document.title = "Online Assset Management";
  }, []);

  return (
    <>
      <CssBaseline />
      <Router>
        <Header />
        <Grid container spacing={0} sx={{ marginTop: 8, paddingLeft:5 }}>
          {/* <Grid item md={1} lg={1}></Grid> */}
          <Grid item xs={12} md={3} lg={3} xl={3}>
            <Sidebar />
          </Grid>
        <Grid item xs={12} md={8} lg={8} xl={8} sx={{ marginTop: 4, marginLeft:10 }}>
          <SusspenseLoading>
            <Switch>
              <Route exact path={HOME}>
                <Home />
              </Route>
              <Route path={USER}>
                <User />
              </Route>
              <Route path={ASSET}>
                <Asset />
              </Route>
              <Route path = {ASSIGNMENT}>
                <Assignment />
              </Route>
              <Route path = {RETURNASSET}>
                <ReturnAsset />
              </Route>
                <Route path={REPORT}>
                  <Report />
                </Route>
            </Switch>
          </SusspenseLoading>
        </Grid>
        </Grid>
      </Router>
    </>
  );
}

export default App;
