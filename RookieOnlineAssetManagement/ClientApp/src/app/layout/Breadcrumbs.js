import { Typography } from "@mui/material";
import React, { useEffect } from "react";
import { Link, useHistory } from "react-router-dom";
import { useLocation, useParams } from "react-router-dom";
import Breadcrumbs from "@mui/material/Breadcrumbs";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
const breadcrumbNameMap = {
  "/": "Home",
  "/user": "Manage User",
  "/user/create": "Create New User",
  "/user/edit": "Edit User",
  "/asset": "Manage Asset",
  "/asset/create": "Create New Asset",
  "/asset/edit": "Edit Asset",
  "/assignment": "Manage Assignment",
  "/assignment/create": "Create New Assignment",
  "/assignment/edit": "Edit Assignment",
  "/returnasset": "Request for Returning",
  "/report":"Report"
};

const HeaderBreadcrumbs = () => {
  const history = useHistory();
  var User = history.location.state?.existUser;
  var Asset = history.location.state?.existAsset;
  var Assignment = history.location.state?.existAssignment;

  const location = useLocation();
  const breadCrumbView = () => {
    const { pathname } = location;
    const pathnames = pathname.split("/").filter((item) => item);
    return (
      <div>
        <Breadcrumbs
          separator={
            <NavigateNextIcon fontSize="small" className="link-light" />
          }
          aria-label="breadcrumb"
        >
          {pathnames.length === 0 ? (
            <Typography className="link-light text-decoration-none fw-bold">
              Home
            </Typography>
          ) : null}
          {pathnames.map((value, index) => {
            // const last = index === pathnames.length - 1;
            const to = `/${pathnames.slice(0, index + 1).join("/")}`;

            if (User || Asset || Assignment) {
              const last = index === pathnames.length - 2;
              
              if (last) {
                return (
                  <Typography
                    className="link-light text-decoration-none fw-bold"
                    key={to}
                  >
                    {breadcrumbNameMap[to]}
                  </Typography>
                );
              }
              
              else if (index === pathnames.length - 1) {
                return null;
              } 
              
              else {
                return (
                  <Link
                    className="link-light text-decoration-none fw-bold"
                    to={to}
                    key={to}
                  >
                    {breadcrumbNameMap[to]}
                  </Link>
                );
              }
            } 
            
            else {
              const last = index === pathnames.length - 1;           
              
              if (last) {
                return (
                  <Typography
                    className="link-light text-decoration-none fw-bold"
                    key={to}
                  >
                    {breadcrumbNameMap[to]}
                  </Typography>
                );
              } 
              
              else {
                return (
                  <Link
                    className="link-light text-decoration-none fw-bold"
                    to={to}
                    key={to}
                  >
                    {breadcrumbNameMap[to]}
                  </Link>
                );
              }
            }
          })}
        </Breadcrumbs>
      </div>
    );
  };
  return <>{breadCrumbView()}</>;
};

export default HeaderBreadcrumbs;
