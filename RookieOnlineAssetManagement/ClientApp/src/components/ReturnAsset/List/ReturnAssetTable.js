import React, { useEffect, useState } from "react";
import {
  FunnelFill,
  Calendar2CheckFill,
  Search,
  Check2,
  X,
} from "react-bootstrap-icons";
import ButtonIcon from "../../../shared-components/ButtonIcon";
import DataTable from "react-data-table-component";
import { Button } from "react-bootstrap";
import { Link, useHistory } from "react-router-dom";
import { MultiSelect } from "react-multi-select-component";
import DatePicker from "react-datepicker";
import ConfirmModal from "../../../shared-components/ConfirmModal";
import {
  getReturnAssetsRequest,
  AcceptRequestReturnAssetRequest,
  CancelRequestReturnAssetRequest
} from "../services/request";
import Swal from "sweetalert2";
import ReactPaginate from "react-paginate";
import ReturnAssetModel from "../../../models/ReturnAssetModel";
import { convertReturnAssetStateToString } from "../../../utils/State";
import { getFormatDateTime } from "../../../utils/Date";
import { grey } from "@mui/material/colors";

const ReturnAssetTable = () => {
  //State for returnAsset

  //State for date picker
  const [startDate, setStartDate] = useState(null);

  //State for returnAsset api
  const [keyword, setKeyword] = useState("");
  const [returnedDateFrom, setReturnedDateFrom] = useState(null);
  const [returnedDateTo, setReturnedDateTo] = useState(null);
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("assetCode");
  const [orderMode, setOrderMode] = useState(1);
  const [selectedState, setSelectedState] = useState([]);
  const [acceptState, setAccept] = useState({
    isOpen: false,
    id: 0,
    title: "",
    message: "",
    isDisable: true,
  });
  const [cancelState, setCancel] = useState({
    isOpen: false,
    id: 0,
    title: "",
    message: "",
    isDisable: true,
  });
  const [returnAssets, setReturnAssets] = useState([]);
  const history = useHistory();

  var returnAssetUpdated = history.location.state?.returnAssetUpdated;
  var returnAssetCreated = history.location.state?.createdReturnAsset;


  useEffect(() => {
    async function fetchData() {

      var pageIndexUpdated = history.location.state?.pageIndex;
      var orderNameUpdated = history.location.state?.orderName;
      var orderModeUpdated = history.location.state?.orderMode;

      if (returnAssetCreated) {
        await GetListReturnAsset(1, null, 1, null, returnAssetCreated);
        history.replace();
      } else if (returnAssetUpdated) {
        await GetListReturnAsset(
          pageIndexUpdated + 1,
          orderNameUpdated,
          orderModeUpdated,
          returnAssetUpdated
        );
        history.replace({
          pageIndex: pageIndexUpdated,
          orderName: orderNameUpdated,
          orderMode: orderModeUpdated,
          assetUpdated: null,
        });
      } else {
        await GetListReturnAsset(1, null, 1, null, null);
      }
    }
    fetchData();
  }, [returnAssetCreated, returnAssetUpdated]);

  // datatable
  const columns = [
    {
      name: "No.",
      selector: (row, index) => pageIndex * pageSize + index + 1,
      sortable: true,
      width:"7%"
    },
    {
      name: "Asset Code",
      selector: (row) => row.assetCode,
      sortable: true,
      width:"11%"
    },
    {
      name: "Asset Name",
      selector: (row) => row.assetName,
      sortable: true,
      width:"19%",
      wrap:true
    },
    {
      name: "Requested by",
      selector: (row) => row.requestedBy,
      sortable: true,
      width:"12%"
    },
    {
      name: "Assigned Date",
      selector: (row) => getFormatDateTime(row.assignedDate),
      sortable: true,
      width:"12%"
    },    
    {
        name: "Accepted By",
        selector: (row) => row.acceptedBy,
        sortable: true,
        width:"11%"
    },
    {
      name: "Returned Date",
      selector: (row) => getFormatDateTime(row.returnedDate)=="1/1/1"?null:getFormatDateTime(row.returnedDate),
      sortable: true,
      width:"12%"
    }, 
    {
      name: "State",
      selector: (row) => row.returnAssetState,
      sortable: true,
      width:"8%"
    },
    {
      cell: (row, index, column, id) => (
        <div className="row">
          <div className="col-2">
            <ButtonIcon
              onClick={() =>
                row.returnAssetState === "Completed"
                  ? null
                  : handleShowAcceptReturnAsset(row.id)
              }
            >
              {row.returnAssetState === "Completed" ? (
                <Check2 className="h5" style={{ color: "silver" }} />
              ):(
                <Check2
                  className="text-danger h5"
                  style={{ cursor: "pointer" }}
                />
              ) }
            </ButtonIcon>
          </div>
          <div className="col-2">
            <ButtonIcon
             onClick={() =>
              row.returnAssetState === "Completed"
                ? null
                : handleShowCancelReturnAsset(row.id)
            }
            >
              {row.returnAssetState === "Completed" ? 
              (
                <X className="h5" style={{ color: "silver" }} size={20} />
              ):
              (
                <X
                  className="text-black h5"
                  size={20}
                  style={{ cursor: "pointer" }}
                />
              )}
            </ButtonIcon>
          </div>
        </div>
      ),
      button: true,
      width:"6%",
      right:true
    },
  ];

  const handlePageChange = async (page) => {
    var currentPage = page.selected + 1;
    await GetListReturnAsset(currentPage, orderName, orderMode);
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
      case "Requested by":
        columnSort = "requestedBy";
        break;
      case "Assigned Date":
        columnSort = "assignedDate";
        break;
      case "Accepted By":
        columnSort = "acceptedBy";
        break;
        case "Returned Date":
          columnSort = "returnedDate";
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
    await GetListReturnAsset(null, columnSort, orderMode);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  const handleShowAcceptReturnAsset = async (id) => {
    setAccept({
      id,
      isOpen: true,
      title: "Are you sure?",
      message: "Do you want to mark this returning request as 'Completed'?",
      isDisable: true,
    });
  };
  const handleCloseAcceptReturnAsset = () => {
    setAccept({
      isOpen: false,
      id: 0,
      title: "",
      message: "",
      isDisable: true,
    });
  };
  const handleConfirmAcceptReturnAsset = async () => {
    let isSuccess = await AcceptRequestReturnAssetRequest(acceptState.id);
    if (isSuccess) {
      handleCloseAcceptReturnAsset();
      handlePageChange(0);
    }
  };

  const handleShowCancelReturnAsset = async (id) => {
    setCancel({
      id,
      isOpen: true,
      title: "Are you sure?",
      message: "Do you want to cancel this returning request?",
      isDisable: true,
    });
  };
  const handleCloseCancelReturnAsset = () => {
    setCancel({
      isOpen: false,
      id: 0,
      title: "",
      message: "",
      isDisable: true,
    });
  };
  const handleConfirmCancelReturnAsset = async () => {
    let isSuccess = await CancelRequestReturnAssetRequest(cancelState.id);
    if (isSuccess) {
      handleCloseCancelReturnAsset();
      handlePageChange(0);
    }
  };
  const options = [
    { label: "Completed", value: 2 },
    { label: "Waiting for returning", value: 1 },
  ];

  const GetListReturnAsset = async (
    page,
    column,
    mode,
  ) => {
    try {
      var listReturnAsset = [];
      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        Keyword: keyword ? keyword : "",
        ReturnedDateFrom: returnedDateFrom,
        ReturnedDateTo: returnedDateTo,
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
        States: selectedState.map((state) => state.value),
      };
      let response = await getReturnAssetsRequest(params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var totalPages = responseData.pageCount ? responseData.pageCount : 0;
          setTotalPages(totalPages);
          setPageIndex(responseData.pageIndex - 1);
          if (responseData.items.length > 0) {
            responseData.items.forEach((element) => {
              var returnAssetResponse = new ReturnAssetModel();
              returnAssetResponse.id = element.id;
              returnAssetResponse.assetCode = element.assetCode;
              returnAssetResponse.assetName = element.assetName;
              returnAssetResponse.requestedBy = element.requestedBy;
              returnAssetResponse.assignedDate = element.assignedDate;
              returnAssetResponse.acceptedBy = element.acceptedBy;
              returnAssetResponse.returnedDate = element.returnedDate;
              returnAssetResponse.returnAssetState =
              convertReturnAssetStateToString(element.returnAssetState);
              listReturnAsset.push(returnAssetResponse);
            });
          }
         
          
          setReturnAssets(listReturnAsset);
        } else {
          await Swal.fire({
            icon: "error",
            title: "Can not get list returnAsset",
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
              onClick={() => GetListReturnAsset(1, orderName, orderMode)}
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
                  if(date === null){
                    setReturnedDateFrom(null);
                    setReturnedDateTo(null);
                  }
                  else{
                    setReturnedDateFrom(date?.toDateString());
                    var nextDate = new Date(date);
                    nextDate.setDate(nextDate.getDate() + 1);
                    setReturnedDateTo(nextDate.toDateString());
                  }
                  
                }}
                isClearable
                placeholderText="Returned Date"
                className="form-control startdate"
                onFocus={(e) => e.target.blur()}
               
              />
            </div>
            <button
              className="btn border w-25"
              onClick={() => {
                GetListReturnAsset(1, orderName, orderMode);
              }}
            >
              <Calendar2CheckFill />
            </button>
          </div>
        </div>

        <div className="d-flex col-md-6 col-lg-6 col-xl-3 mt-2 mb-2">
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
                GetListReturnAsset(1, orderName, orderMode);
              }}
            >
              <Search />
            </div>
          </div>
        </div>

      </div>
      <DataTable
        columns={columns}
        data={returnAssets}
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
      />
<ConfirmModal
        title={acceptState.title}
        isShow={acceptState.isOpen}
        onHide={handleCloseAcceptReturnAsset}
      >
        <div>
          <div className="head-popup">{acceptState.message}</div>
          {acceptState.isDisable && (
            <div className=" mt-3">
              <button
                className="btn btn-danger mr-3"
                onClick={handleConfirmAcceptReturnAsset}
                type="button"
              >
                Yes
              </button>
              &emsp;&emsp;
              <button
                className="btn btn-outline-secondary"
                onClick={handleCloseAcceptReturnAsset}
                type="button"
              >
                No
              </button>
            </div>
          )}
        </div>
      </ConfirmModal>

      <ConfirmModal
        title={cancelState.title}
        isShow={cancelState.isOpen}
        onHide={handleCloseCancelReturnAsset}
      >
        <div>
          <div className="head-popup">{cancelState.message}</div>
          {cancelState.isDisable && (
            <div className=" mt-3">
              <button
                className="btn btn-danger mr-3"
                onClick={handleConfirmCancelReturnAsset}
                type="button"
              >
                Yes
              </button>
              &emsp;&emsp;
              <button
                className="btn btn-outline-secondary"
                onClick={handleCloseCancelReturnAsset}
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

export default ReturnAssetTable;
