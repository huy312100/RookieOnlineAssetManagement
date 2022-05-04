import React, { useEffect, useState } from "react";
import "./index.css";
import DataTable from "react-data-table-component";
import ReactPaginate from "react-paginate";
import { BiSearch } from "react-icons/bi";
import UserModel from "../../../../models/UserModel";
import { getUsersRequest } from "../../../User/services/request";
import Swal from "sweetalert2";

const SelectUser = ({ pref, userSelected, sendUserSelected }) => {
  const [users, setUsers] = useState([]);
  const [selectedUser, setSelectedUser] = useState(userSelected);
  const [selectedNewUser, setSelectedNewUser] = useState(null);
  const [loading, setLoading] = useState(false);
  const [keyword, setKeyword] = useState("");
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("");
  const [orderMode, setOrderMode] = useState(1);
  const [canSubmit, setCanSubmit] = useState(false);

  // useEffect(() => {
  //   return () => sendUserSelected(selectedUser);
  // }, [selectedUser]);

  useEffect(() => {
    if (selectedNewUser) {
      setCanSubmit(true);
    } else setCanSubmit(false);
  }, [selectedNewUser]);

  useEffect(() => {
    GetListUser(1, null, 1);
  }, []);

  // datatable
  const columns = [
    {
      cell: (row, index, column, id) => (
        <input
          className="form-check-input custom-control-input"
          type="radio"
          name="selectUser"
          id={"select_user_" + row.id}
          onClick={() => handleSelectNewUser(row)}
          value={row.id}
        />
      ),
      button: true,
      width: "5%",
    },
    {
      name: "Staff Code",
      selector: (row) => row.staffCode,
      sortable: true,
      width: "25%",
    },
    {
      name: "Full Name",
      selector: (row) => row.fullName,
      sortable: true,
      width: "50%",
      wrap: true,
    },
    {
      name: "Type",
      selector: (row) => row.type,
      sortable: true,
      width: "20%",
    },
  ];

  const GetListUser = async (page, column, mode) => {
    try {
      setLoading(true);
      setSelectedNewUser(null);

      var listUser = [];
      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        Keyword: keyword ? keyword : "",
        Type: "",
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
      };
      let response = await getUsersRequest(params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var pageCount = responseData.pageCount ? responseData.pageCount : 0;
          setTotalPages(pageCount);
          setPageIndex(responseData.pageIndex - 1);
          responseData.items.forEach((element) => {
            var userRs = new UserModel();
            userRs.id = element.id ? element.id : "";
            userRs.fullName = element.fullName ? element.fullName : "";
            userRs.birthDay = element.birthDay ? element.birthDay : "";
            userRs.gender = element.gender ? element.gender : "";
            userRs.isNew = element.isNew ? element.isNew : "";
            userRs.joinedDate = new Date(element.joinedDate).toLocaleDateString(
              "vi-VN"
            );
            userRs.location = element.location ? element.location : "";
            userRs.staffCode = element.staffCode ? element.staffCode : "";
            userRs.type = element.type ? element.type : "";
            userRs.userName = element.userName ? element.userName : "";
            listUser.push(userRs);
          });
          setUsers(listUser);
          setLoading(false);
          if (userSelected && userSelected.id) {
            var userChosed = document.getElementById(
              "select_user_" + userSelected.id
            );
            if (userChosed) {
              userChosed.checked = true;
            }
          }
        }
      } else {
        await Swal.fire({
          icon: "error",
          title: "Can not get list user",
          showConfirmButton: true,
        });
      }
    } catch (error) {
      if (error.message.includes("401")) {
        await Swal.fire({
          icon: "error",
          title: "Permission Denied",
          showConfirmButton: true,
        }).then(function () {
          window.location.href =
            "/Identity/Account/Login?returnUrl=" + window.location.pathname;
        });
      } else {
        await Swal.fire({
          icon: "error",
          title: error,
          showConfirmButton: true,
        });
      }
    }
  };

  const handlePageChange = async (page) => {
    var currentPage = page.selected + 1;
    await GetListUser(currentPage);
  };

  const handleSort = async (column, sortDirection) => {
    setLoading(true);
    var columnSort = "";
    switch (column.name) {
      case "Full Name":
        columnSort = "fullName";
        break;
      case "Type":
        columnSort = "type";
        break;
      case "Staff Code":
        columnSort = "staffCode";
        break;

      default:
        break;
    }
    var orderMode = 1;
    if (sortDirection === "asc") {
      orderMode = 1;
    } else {
      orderMode = 2;
    }
    await GetListUser(null, columnSort, orderMode);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  const handleSelectNewUser = (row) => {
    //setCanSubmit(true);
    setSelectedNewUser(row);
  };

  const SaveSelectedUser = () => {
    setSelectedUser(selectedNewUser);
    sendUserSelected(selectedNewUser);
  };

  const handleCancelSelectUser = () => {
    sendUserSelected(selectedUser);
  };

  return (
    <>
      <div ref={pref} className="containerModalSelect">
        <div className="body bodySelect">
          <div className="container-fluid">
            <div className="row" style={{ padding: "5px" }}>
              <div className="d-flex align-items-left col-md-6 col-lg-6 col-xl-6">
                <h5 className="fw-bold text-danger">Select User</h5>
              </div>
              <div className="d-flex align-items-right col-md-6 col-lg-6 col-xl-6">
                <div className="input-group">
                  <input
                    onChange={(e) => setKeyword(e.target.value)}
                    type="text"
                    className="form-control mr3"
                    style={{ height: "32px" }}
                  />
                  <button
                    type="button"
                    className="border"
                    style={{ cursor: "pointer" }}
                    onClick={() => GetListUser(1, null, null)}
                  >
                    <BiSearch
                      style={{
                        fontSize: "20px",
                      }}
                    />
                  </button>
                </div>
              </div>
            </div>
            <div className="row" style={{ padding: "5px" }}>
              <DataTable
                columns={columns}
                data={users}
                progressPending={loading}
                sortServer
                responsive={true}
                fixedHeader={true}
                fixedHeaderScrollHeight="40vh"
                onSort={handleSort}
                pagination
                paginationServer
                paginationComponent={() => (
                  <ReactPaginate
                    nextLabel="Next"
                    pageRangeDisplayed={3}
                    marginPagesDisplayed={2}
                    pageCount={totalPages}
                    previousLabel="Previous"
                    pageClassName="page-item"
                    pageLinkClassName="page-link"
                    previousClassName="page-item"
                    previousLinkClassName="page-link"
                    nextClassName="page-item"
                    nextLinkClassName="page-link"
                    breakLabel="..."
                    breakClassName="page-item"
                    breakLinkClassName="page-link "
                    containerClassName="pagination mt-3"
                    activeClassName="active"
                    onPageChange={handlePageChange}
                    forcePage={pageIndex}
                  />
                )}
              />
            </div>
            <div className="row mt-4 mb-4 pr-2">
              <div className="col-lg-12 col-md-12 text-end">
                {canSubmit ? (
                  <button
                    className="btn btn-danger px-4 me-3"
                    onClick={SaveSelectedUser}
                  >
                    Save
                  </button>
                ) : (
                  <button className="btn btn-danger px-4 me-3" disabled>
                    Save
                  </button>
                )}

                <button
                  onClick={handleCancelSelectUser}
                  className="btn btn-outline-dark px-4 ms-3"
                >
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default SelectUser;
