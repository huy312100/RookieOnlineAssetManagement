import axios from "axios";
import React, { useState, useEffect } from "react";
import "./index.css";
import { NavLink } from "react-router-dom";

const Sidebar = () => {
  const [userData, setUserData] = useState({});

  useEffect(() => {
    axios.get("/api/users/GetUserLogin").then((response) => {
      console.log(response.data);
      setUserData(response.data);
    });
  }, []);

  return (
    <>
      <div id="Logo_mjb">
        <img
          id="Logo_mjd"
          src={process.env.PUBLIC_URL + "/logo-nashtech.png"}
          alt=""
        />
        <div id="Online_Asset_Management_mjc">
          <span>Online Asset Management</span>
        </div>
      </div>

      <div id="sizeBar">
        <ul className="SidebarList">
          <li className="row active">
            <NavLink to="/" exact activeClassName="active" className="link">
              <div className="title">Home</div>
            </NavLink>
          </li>
          {userData.type === "Admin" ? (
            <>
              <li className="row">
                <NavLink to="/user" activeClassName="active" className="link">
                  <div className="title">Manage User</div>
                </NavLink>
              </li>
              <li className="row">
                <NavLink to="/asset" activeClassName="active" className="link">
                  <div className="title">Manage Asset</div>
                </NavLink>
              </li>
              <li className="row">
                <NavLink
                  to="/assignment"
                  activeClassName="active"
                  className="link"
                >
                  <div className="title">Manage Assignment</div>
                </NavLink>
              </li>
              <li className="row">
                <NavLink
                  to="/returnasset"
                  activeClassName="active"
                  className="link"
                >
                  <div className="title">Request for Returning</div>
                </NavLink>
              </li>
              <li className="row">
                <NavLink to="/report" activeClassName="active" className="link">
                  <div className="title">Report</div>
                </NavLink>
              </li>
            </>
          ) : null}
        </ul>
      </div>
    </>
  );
};

export default Sidebar;
