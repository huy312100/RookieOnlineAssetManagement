import React, { useEffect, useState } from "react";
import {
  FunnelFill,
  Calendar2CheckFill,
  PencilFill,
  Search,
  XCircle,
  ArrowClockwise,
} from "react-bootstrap-icons";
import ButtonIcon from "../../../shared-components/ButtonIcon";
import ConfirmModal from "../../../shared-components/ConfirmModal";
import ErrorModal from "../../../shared-components/ErrorModal";
import Info from "../Info";
import DataTable from "react-data-table-component";
import { Button } from "react-bootstrap";
import { Link, useHistory } from "react-router-dom";
import { MultiSelect } from "react-multi-select-component";
import DatePicker from "react-datepicker";
import {
  returnAssetRequest,
  getAssignmentsRequest,
  DeleteAssignmentRequest,
} from "../services/request";
import Swal from "sweetalert2";
import ReactPaginate from "react-paginate";
import { createBrowserHistory } from "history";
import {
  CREATE_ASSIGNMENT,
  EDIT_ASSIGNMENT_ID,
} from "../../../Constants/pages";
import AssignmentModel from "../../../models/AssignmentModel";
import { convertAssignmentStateToString } from "../../../utils/State";
import { getFormatDateTime } from "../../../utils/Date";
import "./index.css";
import { grey } from "@mui/material/colors";

const AssignmentTable = () => {
  //State for assignment
  const [showDetail, setShowDetail] = useState(false);
  const [assignmentDetail, setAssignmentDetail] = useState(null);

  //State for date picker
  const [startDate, setStartDate] = useState(null);

  //State for assignment api
  const [keyword, setKeyword] = useState("");
  const [assignDateFrom, setAssignDateFrom] = useState(null);
  const [assignDateTo, setAssignDateTo] = useState(null);
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("assetCode");
  const [orderMode, setOrderMode] = useState(1);
  const [selectedState, setSelectedState] = useState([]);

  const [assignments, setAssignments] = useState([]);
  const [disableState, setDisable] = useState({
    isOpen: false,
    id: 0,
    title: "",
    message: "",
    isDisable: true,
  });
  const [returnAssetState, setReturnAsset] = useState({
    isOpen: false,
    id: 0,
    title: "",
    message: "",
    isDisable: true,
  });
  const history = useHistory();

  var assignmentUpdated = history.location.state?.assignmentUpdated;
  var assignmentCreated = history.location.state?.createdAssignment;

  useEffect(() => {
    async function fetchData() {
      var pageIndexUpdated = history.location.state?.pageIndex;
      var orderNameUpdated = history.location.state?.orderName;
      var orderModeUpdated = history.location.state?.orderMode;

      if (assignmentCreated) {
        await GetListAssignment(1, null, 1, null, assignmentCreated);
        history.replace();
      } else if (assignmentUpdated) {
        await GetListAssignment(
          pageIndexUpdated + 1,
          orderNameUpdated,
          orderModeUpdated,
          assignmentUpdated
        );
        history.replace({
          pageIndex: pageIndexUpdated,
          orderName: orderNameUpdated,
          orderMode: orderModeUpdated,
          assetUpdated: null,
        });
      } else {
        await GetListAssignment(1, null, 1, null, null);
      }
    }
    fetchData();
  }, [assignmentCreated, assignmentUpdated]);

  // datatable
  const columns = [
    {
      name: "No.",
      selector: (row, index) => pageIndex * pageSize + index + 1,
      sortable: true,
      width: "70px",
    },
    {
      name: "Asset Code",
      selector: (row) => row.assetCode,
      sortable: true,
      width: "12%",
    },
    {
      name: "Asset Name",
      selector: (row) => row.assetName,
      sortable: true,
      minWidth: "50px",
      maxWidth: "190px",
    },
    {
      name: "Assigned To",
      selector: (row) => row.assignedTo,
      sortable: true,
      width: "13%",
    },
    {
      name: "Assigned By",
      selector: (row) => row.assignedBy,
      sortable: true,
      width: "13%",
    },
    {
      name: "Assigned Date",
      selector: (row) => getFormatDateTime(row.assignedDate),
      sortable: true,
      width: "14%",
    },
    {
      name: "State",
      selector: (row) => row.assignmentState,
      sortable: true,
      width: "11%",
    },
    {
      cell: (row, index, column, id) => (
        <div className="row">
          <div className="col-2">
            <ButtonIcon
              onClick={() =>
                row.assignmentState === "Waiting for acceptance"
                  ? handleEdit(row, index)
                  : null
              }
            >
              {row.assignmentState === "Waiting for acceptance" ? (
                <PencilFill
                  className="text-black"
                  style={{ cursor: "pointer" }}
                />
              ) : (
                <PencilFill style={{ color: "silver" }} />
              )}
            </ButtonIcon>
          </div>
          <div className="col-2" style={{ cursor: "pointer" }}>
            <ButtonIcon
              onClick={() =>
                row.assignmentState === "Waiting for acceptance"
                  ? handleShowDisable(row.id)
                  : null
              }
            >
              {row.assignmentState === "Waiting for acceptance" ? (
                <XCircle
                  className="text-danger"
                  style={{ cursor: "pointer" }}
                />
              ) : (
                <XCircle style={{ color: "silver" }} />
              )}
            </ButtonIcon>
          </div>
          <div className="col-2">
            <ButtonIcon
              onClick={() =>
                row.assignmentState === "Waiting for acceptance" || row.returnAsset !== null ? null : handleShowReturnAsset(row.id)
              }
            >
              {row.assignmentState === "Waiting for acceptance" || row.returnAsset !== null ? 
              (
                <ArrowClockwise className="h6" style={{ color: "silver" }} />
              ) : (
                <ArrowClockwise
                  className="text-primary h6"
                  style={{ cursor: "pointer" }}
                />
              )}
            </ButtonIcon>
          </div>
        </div>
      ),
      button: true,
      width: "8%",
      right: true,
    },
  ];

  const handlePageChange = async (page) => {
    var currentPage = page.selected + 1;
    await GetListAssignment(currentPage, orderName, orderMode);
  };

  const handleShowReturnAsset = async (id) => {
    setReturnAsset({
      id,
      isOpen: true,
      title: "Are you sure?",
      message: "Do you want to create a returning request for this asset?",
      isDisable: true,
    });
  };

  const handleCloseReturnAsset = () => {
    setReturnAsset({
      isOpen: false,
      id: 0,
      title: "",
      message: "",
      isDisable: true,
    });
  };

  const handleConfirmReturnAsset = async () => {
    let isSuccess = await returnAssetRequest(returnAssetState.id);
    if (isSuccess) {
      handleCloseReturnAsset();
      handlePageChange(0);
      await Swal.fire({
        position: "center",
        icon: "success",
        title: "Returning asset success",
        showConfirmButton: true,
      });
    }
  };

  const handleSort = async (column, sortDirection) => {
    var columnSort = "";
    switch (column.name) {
      case "State":
        columnSort = "state";
        break;
      case "Asset Name":
        columnSort = "assetName";
        break;
      case "Assigned To":
        columnSort = "assignedTo";
        break;
      case "Assigned By":
        columnSort = "assignedBy";
        break;
      case "Assigned Date":
        columnSort = "assignedDate";
        break;
      default: {
        columnSort = "assetCode";
        break;
      }
    }
    var orderMode = 1;
    if (sortDirection === "asc") {
      orderMode = 1;
    } else {
      orderMode = 2;
    }
    await GetListAssignment(null, columnSort, orderMode);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  const options = [
    { label: "Accepted", value: 2 },
    { label: "Waiting for acceptance", value: 1 },
  ];

  const handleShowInfo = (row, event) => {
    if (row) {
      setAssignmentDetail(row);
      setShowDetail(true);
    }
  };

  const handleEdit = (row, index) => {
    history.push(EDIT_ASSIGNMENT_ID(assignments[index].id), {
      existAssignment: assignments[index],
      pageIndex: pageIndex,
      sortColumn: orderName,
      sortMode: orderMode,
    });
  };

  const handleCreateAssignment = () => {
    history.push(CREATE_ASSIGNMENT, {
      pageIndex: pageIndex,
      sortColumn: orderName,
      sortMode: orderMode,
    });
  };

  const handleShowDisable = async (id) => {
    setDisable({
      id,
      isOpen: true,
      title: "Are you sure?",
      message: "Do you want to delete this Assignment?",
      isDisable: true,
    });
  };

  const handleCloseDisable = () => {
    setDisable({
      isOpen: false,
      id: 0,
      title: "",
      message: "",
      isDisable: true,
    });
  };

  const handleConfirmDisable = async () => {
    let isSuccess = await DeleteAssignmentRequest(disableState.id);
    if (isSuccess) {
      await GetListAssignment(1, orderName, orderMode);
      handleCloseDisable();

      await Swal.fire({
        position: "center",
        icon: "success",
        title: "Delete success",
        showConfirmButton: true,
      });
    }
  };

  const handleCloseDetail = () => {
    setShowDetail(false);
  };

  const GetListAssignment = async (
    page,
    column,
    mode,
    assignmentItemUpdated,
    assignmentItemCreated
  ) => {
    try {
      var listAssignment = [];
      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        Keyword: keyword ? keyword : "",
        AssignedDateFrom: assignDateFrom,
        AssignedDateTo: assignDateTo,
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
        States: selectedState.map((state) => state.value),
      };
      let response = await getAssignmentsRequest(params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var totalPages = responseData.pageCount ? responseData.pageCount : 0;
          setTotalPages(totalPages);
          setPageIndex(responseData.pageIndex - 1);
          if (responseData.items.length > 0) {
            responseData.items.forEach((element) => {
              var assignmentResponse = new AssignmentModel();
              assignmentResponse.id = element.id;
              assignmentResponse.userIdAssigned = element.userIdAssigned;
              assignmentResponse.assetId = element.assetId;
              assignmentResponse.fullnameAssigned = element.fullnameAssigned;
              assignmentResponse.assignedTo = element.assignedTo;
              assignmentResponse.assignedBy = element.assignedBy;
              assignmentResponse.assetCode = element.assetCode;
              assignmentResponse.assetName = element.assetName;
              assignmentResponse.specification = element.specification;
              assignmentResponse.assignedDate = element.assignedDate;
              assignmentResponse.modifiedDate = element.modifiedDate;
              assignmentResponse.assignmentState =
                convertAssignmentStateToString(element.assignmentState);
              assignmentResponse.note = element.note;
              assignmentResponse.returnAsset=element.returnAsset;
              assignmentResponse.categoryName = element.categoryName;
              listAssignment.push(assignmentResponse);
            });
          }

          if (assignmentItemUpdated) {
            listAssignment.sort(function (x, y) {
              return x.id === assignmentItemUpdated.id
                ? -1
                : y === assignmentItemUpdated
                ? 1
                : 0;
            });
          }

          if (assignmentItemCreated) {
            for (var i = 0; i < listAssignment.length; i++) {
              if (listAssignment[i].id === assignmentCreated.id) {
                var temp = listAssignment[0];
                listAssignment[0] = assignmentItemCreated;
                listAssignment[i] = temp;
                break;
              }
              if (
                i === listAssignment.length - 1 &&
                listAssignment[i].id !== assignmentCreated.id
              ) {
                var assignmentHead = new AssignmentModel();
                assignmentHead.id = assignmentItemCreated.id;
                assignmentHead.userIdAssigned =
                  assignmentItemCreated.userIdAssigned;
                assignmentHead.assetId = assignmentItemCreated.assetId;
                assignmentHead.fullnameAssigned =
                  assignmentItemCreated.fullnameAssigned;
                assignmentHead.assignedTo = assignmentItemCreated.assignedTo;
                assignmentHead.assignedBy = assignmentItemCreated.assignedBy;
                assignmentHead.assetCode = assignmentItemCreated.assetCode;
                assignmentHead.assetName = assignmentItemCreated.assetName;
                assignmentHead.specification =
                  assignmentItemCreated.specification;
                assignmentHead.assignedDate =
                  assignmentItemCreated.assignedDate;
                assignmentHead.modifiedDate =
                  assignmentItemCreated.modifiedDate;
                assignmentHead.assignmentState = convertAssignmentStateToString(
                  assignmentItemCreated.assignmentState
                );
                assignmentHead.note = assignmentItemCreated.note;
                assignmentHead.categoryName =
                  assignmentItemCreated.categoryName;

                listAssignment.unshift(assignmentHead);
                listAssignment.pop();
              }
            }
          }
          setAssignments(listAssignment);
        } else {
          await Swal.fire({
            icon: "error",
            title: "Can not get list assignment",
            // showConfirmButton: true,
          });
        }
      }
    } catch (err) {
      if (err.message.includes("401")) {
        await Swal.fire({
          icon: "error",
          title: "Permission Denied",
          // showConfirmButton
        });
      }
    }
  };
  return (
    <>
      <div className="row" style={{ padding: "15px" }}>
        <div className="d-flex align-items-center col-md-12 col-lg-12 col-xl-3 mt-2 mb-2">
          <div className="input-group" style={{ zIndex: 10 }}>
            <MultiSelect
              options={options}
              value={selectedState}
              onChange={setSelectedState}
              className="w-75"
              disableSearch={true}
              valueRenderer={() => <span>State</span>}
              ClearSelectedIcon={() => ""}
              ArrowRenderer={() => ""}
            />
            <button
              className="btn border w-25"
              onClick={() => GetListAssignment(1, orderName, orderMode)}
            >
              <FunnelFill />
            </button>
          </div>
        </div>
        <div className="d-flex align-items-center col-md-12 col-lg-12 col-xl-3 mt-2 mb-2 ">
          <div className="input-group" style={{ zIndex: 10, color: grey }}>
            <div className="w-75">
              <DatePicker
                selected={startDate}
                onChange={(date) => {
                  setStartDate(date);
                  if (date === null) {
                    setAssignDateFrom(null);
                    setAssignDateTo(null);
                  } else {
                    setAssignDateFrom(date?.toDateString());
                    var nextDate = new Date(date);
                    nextDate.setDate(nextDate.getDate() + 1);
                    setAssignDateTo(nextDate.toDateString());
                  }
                }}
                isClearable
                placeholderText="Assigned Date"
                className="form-control startdate"
                onFocus={(e) => e.target.blur()}
              />
            </div>
            <button
              className="btn border w-25"
              onClick={() => {
                GetListAssignment(1, orderName, orderMode);
              }}
            >
              <Calendar2CheckFill />
            </button>
          </div>
        </div>

        <div className="d-flex align-items-center col-md-6 col-lg-6 col-xl-3 mt-2 mb-2">
          <div className="input-group">
            <input
              onChange={(e) => setKeyword(e.target.value)}
              type="text"
              className="form-control mr3"
            />
            <div
              className="border p-2"
              style={{ cursor: "pointer" }}
              onClick={() => {
                GetListAssignment(1, orderName, orderMode);
              }}
            >
              <Search />
            </div>
          </div>
        </div>

        <div className="d-flex col-md-6 col-lg-6 col-xl-3 mt-2 mb-2">
          <Button onClick={handleCreateAssignment} className="btn btn-danger">
            Create new assignment
          </Button>
        </div>
      </div>
      <DataTable
        columns={columns}
        data={assignments}
        sortServer
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

      {assignmentDetail && showDetail && (
        <Info assignment={assignmentDetail} handleClose={handleCloseDetail} />
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
                Delete
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

      <ConfirmModal
        title={returnAssetState.title}
        isShow={returnAssetState.isOpen}
        onHide={handleCloseReturnAsset}
      >
        <div>
          <div className="">{returnAssetState.message}</div>
          {returnAssetState.isDisable && (
            <div className=" mt-3">
              <button
                className="btn btn-danger mr-3"
                onClick={handleConfirmReturnAsset}
                type="button"
              >
                Yes
              </button>
              &emsp;&emsp;
              <button
                className="btn btn-outline-secondary"
                onClick={handleCloseReturnAsset}
                type="button"
              >
                No
              </button>
            </div>
          )}
        </div>
      </ConfirmModal>
    </>
  );
};

export default AssignmentTable;
