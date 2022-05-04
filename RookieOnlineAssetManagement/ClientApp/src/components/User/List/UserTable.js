import React, { useEffect, useState } from "react";
import { FunnelFill, PencilFill, Search, XCircle } from "react-bootstrap-icons";
import ButtonIcon from "../../../shared-components/ButtonIcon";
import ConfirmModal from "../../../shared-components/ConfirmModal";
import ErrorModal from "../../../shared-components/ErrorModal";
import Info from "../Info";
import DataTable from "react-data-table-component";
import { Pagination } from "../../../shared-components/Table/Pagination";
import { Button } from "react-bootstrap";
import { Link, useHistory } from "react-router-dom";
import { MultiSelect } from "react-multi-select-component";
import {
  getUsersRequest,
  DisableUserRequest,
  CheckDisableUserRequest,
} from "../services/request";
import UserRequestModel from "../../../models/UserRequestModel";
import Swal from "sweetalert2";
import ReactPaginate from "react-paginate";
import UserModel from "../../../models/UserModel";
import { createBrowserHistory } from "history";
import { CREATE_USER, EDIT_USER_ID } from "../../../Constants/pages";
import { set } from "nprogress";

const UserTable = () => {
  const [loading, setLoading] = useState(true);
  const [showDetail, setShowDetail] = useState(false);
  const [userDetail, setUserDetail] = useState(null);
  const [typeSelected, setTypeSelected] = useState([]);
  const [keyword, setKeyword] = useState("");
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("");
  const [orderMode, setOrderMode] = useState(1);
  const [users, setUsers] = useState([]);
  const [disableState, setDisable] = useState({
    isOpen: false,
    StaffCode: 0,
    title: "",
    message: "",
    isDisable: true,
  });
  const [disableErrState, setErrDisable] = useState({
    isOpen: false,
    title: "",
    message1: "",
    message2: "",
    isDisable: true,
  });
  const history = useHistory();
  var userUpdated = history.location.state?.userUpdated;
  var userCreated = history.location.state?.userCreated;

  useEffect(() => {
    async function fetchData() {
      if (userUpdated) {
        var pageIndexUpdated = history.location.state?.pageIndex;
        var orderNameUpdated = history.location.state?.orderName;
        var orderModeUpdated = history.location.state?.orderMode;
        await GetListUser(
          pageIndexUpdated + 1,
          orderNameUpdated,
          orderModeUpdated,
          userUpdated
        );
        history.replace({
          pageIndex: pageIndexUpdated,
          orderName: orderNameUpdated,
          orderMode: orderModeUpdated,
          userUpdated: null,
        });
      } else if (userCreated) {
        await GetListUser(1, null, 1, userCreated);
        history.replace();
      } else {
        await GetListUser(1, null, 1, null);
      }
    }
    fetchData();
  }, [userUpdated, userCreated]);

  // datatable
  const columns = [
    {
      name: "Staff Code",
      selector: (row) => row.staffCode,
      sortable: true,
      width: "15%",
    },
    {
      name: "Full Name",
      selector: (row) => row.fullName,
      sortable: true,
      width: "25%",
      wrap: true,
    },
    {
      name: "Username",
      selector: (row) => row.userName,
      width: "15%",
    },
    {
      name: "Joined Date",
      selector: (row) => row.joinedDate,
      sortable: true,
      width: "15%",
    },
    {
      name: "Type",
      selector: (row) => row.type,
      sortable: true,
      width: "10%",
    },
    {
      cell: (row, index, column, id) => (
        <div className="row">
          <div className="col-4" style={{ cursor: "pointer" }}>
            <ButtonIcon onClick={() => handleEdit(row, index)}>
              <PencilFill className="text-black" />
            </ButtonIcon>
          </div>
          <div className="col-6" style={{ cursor: "pointer" }}>
            <ButtonIcon onClick={() => checkDisable(row.userName)}>
              <XCircle className="text-danger mx-2" />
            </ButtonIcon>
          </div>
        </div>
      ),
      button: true,
      width: "20%",
    },
  ];

  const options = [
    { label: "Admin", value: "admin" },
    { label: "Staff", value: "staff" },
  ];

  const handleShowInfo = (row, event) => {
    if (row) {
      setUserDetail(row);
      setShowDetail(true);
    }
  };

  const handleEdit = (row, index) => {
    history.push(EDIT_USER_ID(users[index].id), {
      existUser: users[index],
      pageIndex: pageIndex,
      sortColumn: orderName,
      sortMode: orderMode,
    });
  };
  const handleCreateUser = () => {
    history.push(CREATE_USER, {
      pageIndex: pageIndex,
      sortColumn: orderName,
      sortMode: orderMode,
    });
  };

  const handleShowDisable = async (UserName) => {
    setDisable({
      UserName,
      isOpen: true,
      title: "Are you sure?",
      message: "Do you want to disable this User?",
      isDisable: true,
    });
  };

  const handleShowErrDisable = async () => {
    setErrDisable({
      isOpen: true,
      title: "Can not disable user",
      message1: "There are valid assignments belonging to this user.",
      message2: "Please close all assignments before disabling user.",
      isDisable: true,
    });
  };

  const handleCloseDisable = () => {
    setDisable({
      isOpen: false,
      UserName: "",
      title: "",
      message: "",
      isDisable: true,
    });
  };

  const handleCloseErrDisable = () => {
    setErrDisable({
      isOpen: false,
      UserName: "",
      title: "",
      message1: "",
      message2: "",
      isDisable: true,
    });
  };
  const handleConfirmDisable = async () => {
    let isSuccess = await DisableUserRequest(disableState.UserName);
    if (isSuccess) {
      handleCloseDisable();
      await GetListUser(1, orderName, orderMode);
    await Swal.fire({
      position: 'center',
      icon: 'success',
      title: 'Delete success',
      showConfirmButton: true
    });
    }
  };
  const checkDisable = async (UserName) => {
    // console.log(UserName);
    let check = await CheckDisableUserRequest(UserName);
    // console.log(check.data);
    if (check.data == true) {
      handleShowErrDisable();
    } else {
      handleShowDisable(UserName);
    }
  };

  const handleCloseDetail = () => {
    setShowDetail(false);
  };

  const GetListUser = async (page, column, mode, userPriority) => {
    try {
      setLoading(true);
      var listUser = [];
      var typeFilter = "";
      if (typeSelected.length === 1) {
        typeFilter = typeSelected[0].value;
      }

      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        Keyword: keyword ? keyword : "",
        Type: typeFilter,
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
      };
      let response = await getUsersRequest(params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var totalPages = responseData.pageCount ? responseData.pageCount : 0;
          setTotalPages(totalPages);
          setPageIndex(responseData.pageIndex - 1);
          responseData.items.forEach((element) => {
            var userRs = new UserModel();
            userRs.id = element.id ? element.id : "";
            userRs.fullName = element.fullName ? element.fullName : "";
            userRs.birthDay = element.birthDay ? element.birthDay : "";
            userRs.gender = element.gender ? element.gender : "";
            userRs.isNew = element.isNew ? element.isNew : "";
            // userRs.joinedDate = new Date(element.joinedDate).toLocaleDateString(
            //   "vi-VN"
            // );
            userRs.joinedDate = element.joinedDate
              ? new Date(element.joinedDate).toLocaleDateString("vi-VN")
              : "";
            userRs.location = element.location ? element.location : "";
            userRs.staffCode = element.staffCode ? element.staffCode : "";
            userRs.type = element.type ? element.type : "";
            userRs.userName = element.userName ? element.userName : "";
            listUser.push(userRs);
          });
          if (userPriority) {
            if (listUser.includes(userPriority)) {
              listUser.sort(function (x, y) {
                return x.id === userPriority.id
                  ? -1
                  : y === userPriority
                  ? 1
                  : 0;
              });
            } else {
              var userHead = new UserModel();
              userHead.id = userPriority.id ? userPriority.id : "";
              userHead.fullName = userPriority.fullName
                ? userPriority.fullName
                : "";
              userHead.birthDay = userPriority.birthDay
                ? userPriority.birthDay
                : "";
              userHead.gender = userPriority.gender ? userPriority.gender : "";
              userHead.isNew = userPriority.isNew ? userPriority.isNew : "";
              userHead.joinedDate = userPriority.joinedDate
                ? new Date(userPriority.joinedDate).toLocaleDateString("vi-VN")
                : "";
              userHead.location = userPriority.location
                ? userPriority.location
                : "";
              userHead.staffCode = userPriority.staffCode
                ? userPriority.staffCode
                : "";
              userHead.type = userPriority.type ? userPriority.type : "";
              userHead.userName = userPriority.userName
                ? userPriority.userName
                : "";
              listUser.unshift(userHead);
              listUser.pop();
            }
          }
          setUsers(listUser);
          setLoading(false);
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
      case "Joined Date":
        columnSort = "joinedDate";
        break;
      case "Type":
        columnSort = "type";
        break;

      default: {
        columnSort = "staffCode";
        break;
      }
    }
    var orderMode = 1;
    if (sortDirection === "asc") {
      orderMode = 1;
    } else {
      orderMode = 2;
    }
    await GetListUser(null, columnSort, orderMode, null);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  return (
    <>
      <div className="row" style={{ padding: "15px" }}>
        <div className="d-flex align-items-center col-md-12 col-lg-12 col-xl-5">
          <div className="input-group" style={{ zIndex: 9 }}>
            <MultiSelect
              options={options}
              value={typeSelected}
              onChange={setTypeSelected}
              disableSearch={true}
              valueRenderer={() => (
                <div
                  style={{
                    width: "100px",
                  }}
                >
                  <span>Type</span>
                </div>
              )}
              ClearSelectedIcon={() => ""}
              ArrowRenderer={() => ""}
            />
            <button
              className="btn border"
              onClick={() => GetListUser(1, null, null)}
            >
              <FunnelFill />
            </button>
          </div>
        </div>
        <div className="d-flex align-items-center col-md-6 col-lg-6 col-xl-4">
          <div className="input-group">
            <input
              onChange={(e) => setKeyword(e.target.value)}
              type="text"
              className="form-control mr3"
            />
            <div
              className="border p-2"
              style={{ cursor: "pointer" }}
              onClick={() => GetListUser(1, null, null)}
            >
              <Search />
            </div>
          </div>
        </div>

        <div className="d-flex col-md-6 col-lg-6 col-xl-3 justify-content-end">
          <Button onClick={handleCreateUser} className="btn btn-danger">
            Create new user
          </Button>
        </div>
      </div>
      <DataTable
        columns={columns}
        data={users}
        progressPending={loading}
        sortServer
        responsive={true}
        fixedHeader={true}
        fixedHeaderScrollHeight="50vh"
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
        onRowClicked={(row, event) => handleShowInfo(row, event)}
      />

      {userDetail && showDetail && (
        <Info user={userDetail} handleClose={handleCloseDetail} />
      )}
      <ConfirmModal
        title={disableState.title}
        isShow={disableState.isOpen}
        onHide={handleCloseDisable}
      >
        <div>
          <div className="">{disableState.message}</div>
          {disableState.isDisable && (
            <div className=" mt-3">
              <button
                className="btn btn-danger mr-3"
                onClick={handleConfirmDisable}
                type="button"
              >
                Disable
              </button>
              &emsp;&emsp;
              <button
                className="btn btn-outline-secondary"
                onClick={handleCloseDisable}
                type="button"
              >
                Cancel
              </button>
            </div>
          )}
        </div>
      </ConfirmModal>
      <ErrorModal
        title={disableErrState.title}
        isShow={disableErrState.isOpen}
        onHide={handleCloseErrDisable}
      >
        <div>
          <div>{disableErrState.message1}</div>
          <div className="mt-1">{disableErrState.message2}</div>
        </div>
      </ErrorModal>
    </>
  );
};

export default UserTable;
