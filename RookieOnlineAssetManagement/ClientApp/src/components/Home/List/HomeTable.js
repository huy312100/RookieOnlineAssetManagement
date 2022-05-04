import React, { useEffect, useState } from "react";
import { FunnelFill, PencilFill, Search, X, ArrowClockwise, Check2 } from "react-bootstrap-icons";
import ButtonIcon from "../../../shared-components/ButtonIcon";
import ConfirmModal from "../../../shared-components/ConfirmModal";
import ErrorModal from "../../../shared-components/ErrorModal";
import Info from "../Info";
import DataTable from "react-data-table-component";
import { Pagination } from "../../../shared-components/Table/Pagination";
import { Link, useHistory } from "react-router-dom";
import {
  getHomesRequest, 
  ChangeAssignmentStateRequest,
  DeleteAssignmentRequest,
  ReturnAssetRequest
} from "../services/request";
import Swal from "sweetalert2";
import ReactPaginate from "react-paginate";
import HomeAssignmentModel from "../../../models/HomeAssignmentModel";


const HomeTable = () => {
  const [loading, setLoading] = useState(true);
  const [showDetail, setShowDetail] = useState(false);
  const [assignmentDetail, setAssignmentDetail] = useState(null);
  const [typeSelected, setTypeSelected] = useState([]);
  const [keyword, setKeyword] = useState("");
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("");
  const [orderMode, setOrderMode] = useState(1);
  const [assignments, setAssignments] = useState([]);
  const [acceptState, setAccept] = useState({
    isOpen: false,
    id: 0,
    title: "",
    message: "",
    isDisable: true,
  });
  const [deleteState, setDelete] = useState({
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
  var pageIndexUpdated = history.location.state?.pageIndex;
  var orderNameUpdated = history.location.state?.orderName;
  var orderModeUpdated = history.location.state?.orderMode;
  var assignmentCreated = history.location.state?.createdUser;
  var pageIndexCreated = history.location.state?.pageIndex;
  var orderNameCreated = history.location.state?.orderName;
  var orderModeCreated = history.location.state?.orderMode;


  const getFormatDateTime=(date)=>{
    const DATE_OPTIONS = { day: 'numeric', month: 'numeric', year: 'numeric' };
    return new Date(date).toLocaleDateString('vi-VN', DATE_OPTIONS);
  }
  useEffect(()=>{
    GetListAssignment(1,null,1,null);
  },[]);

   // datatable
  const columns = [
    {
      name: "Asset Code",
      selector: (row) => row.assetCode,
      sortable: true,
      width: "15%",
    },
    {
      name: "Asset Name",
      selector: (row) => row.assetName,
      sortable: true,
      minWidth: "50px",
      maxWidth: "200px"
    },
    {
      name: "Category",
      selector: (row) => row.category,
      sortable: true,
      minWidth: "50px",
      maxWidth: "120px"
    },
    {
      name: "Assigned Date",
      selector: (row) => getFormatDateTime(row.assignedDate),
      sortable: true,
      width: "15%",
    },
    {
      name: "State",
      selector: (row) => row.state,
      sortable: true,
      width: "20%",
    },
    {
      cell: (row, index, column, id) => (
        <div className="row inline-block" >
           <div className="col-3" style={{ cursor: "pointer" }}>
            <ButtonIcon onClick={() => 
                row.state === "Waiting for acceptance" ?  handleShowAcceptAssignment(row.id):null
              }>
                {row.state === "Waiting for acceptance" ?                 
                 (<Check2 className="text-danger h5" style={{ cursor: "pointer" }}/>)
                 :(<Check2 className=" h5" style={{color: "silver"}} />)
              }
              {/* <Check2 className="text-danger h5 " /> */}
            </ButtonIcon>
          </div> 
          <div className="col-3" style={{ cursor: "pointer" }}>
            <ButtonIcon onClick={() => 
                row.state === "Waiting for acceptance" ?  handleShowDeleteAssignment(row.id):null
              }>
                {row.state === "Waiting for acceptance" ?                 
                 (<X className="text-black h5" style={{ cursor: "pointer" }} size={20}/>)
                 :(<X className=" h5" style={{color: "silver"}} size={20} />)
              }

              {/* <X className="text-black h5" size={20} /> */}
            </ButtonIcon>
          </div>
          <div className="col-3">
            <ButtonIcon onClick={() => 
                row.state === "Waiting for acceptance" || row.returnAsset!=null ?  console.log(row) : handleShowReturnAsset(row.id)                
              }
            >
                {row.state === "Waiting for acceptance" || row.returnAsset!=null ?   
                (<ArrowClockwise className=" h5" style={{color: "silver"}} size={17} />):              
                 (<ArrowClockwise className="text-primary h5" style={{ cursor: "pointer" }} size={17}/>)
            }  
            </ButtonIcon>
          </div>
        </div>
      ),
      button: true,
      width: "10%",
    },
  ];


  const handleShowInfo = (row, event) => {
    if (row) {
      setAssignmentDetail(row);
      setShowDetail(true);
    }
  };


  const handleCloseDetail = () => {
    setShowDetail(false);
  };

  const GetListAssignment = async (page, column, mode, assignmentPriority) => {
    try {
      var listHome = [];
      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        Keyword: keyword ? keyword : "",
        States: 1,
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
      };
      let response = await getHomesRequest(params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var totalPages = responseData.pageCount ? responseData.pageCount : 0;
          setTotalPages(totalPages);
          setPageIndex(responseData.pageIndex - 1);
          responseData.items.forEach((element) => {
            console.log(element);
            var assignments = new HomeAssignmentModel();
            assignments.id = element.id 
            assignments.assetCode = element.assetCode ? element.assetCode : "";
            assignments.assetName = element.assetName ? element.assetName : "";
            assignments.category = element.categoryName ? element.categoryName : "";
            assignments.assignedDate = element.assignedDate;
            assignments.specification = element.specification;
            assignments.note = element.note;
            assignments.assignedBy = element.assignedBy;
            assignments.assignedTo = element.assignedTo;
            assignments.returnAsset = element.returnAsset;

            // assignments.state = element.assignmentState ? element.assignmentState : "";
            if(element.assignmentState==1){
              assignments.state="Waiting for acceptance"
            }
            else if(element.assignmentState==2){
              assignments.state="Accepted"
            }

            listHome.push(assignments);
          });
          if (assignmentPriority) {
            listHome.sort(function (x, y) {
              return x.id === assignmentPriority.id ? -1 : y === assignmentPriority ? 1 : 0;
            });
          }
          setAssignments(listHome);
          // setLoading(false);
        }
      } else {
        await Swal.fire({
          icon: "error",
          title: "Can not get list assignment",
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
    await GetListAssignment(currentPage);
  };

  const handleShowAcceptAssignment = async (id) => {
    setAccept({
      id,
      isOpen: true,
      title: "Are you sure?",
      message: "Do you want to accept this assignment?",
      isDisable: true,
    });
  };
  const handleCloseAcceptAssignment = () => {
    setAccept({
      isOpen: false,
      id: 0,
      title: "",
      message: "",
      isDisable: true,
    });
  };
  const handleConfirmAcceptAssignment = async () => {
    let isSuccess = await ChangeAssignmentStateRequest(acceptState.id);
    if (isSuccess) {
      handleCloseAcceptAssignment();
      handlePageChange(0);
    }
  };

//////
const handleShowDeleteAssignment = async (id) => {
  setDelete({
    id,
    isOpen: true,
    title: "Are you sure?",
    message: "Do you want to decline this assignment?",
    isDisable: true,
  });
};
const handleCloseDeleteAssignment = () => {
  setDelete({
    isOpen: false,
    id: 0,
    title: "",
    message: "",
    isDisable: true,
  });
};
const handleConfirmDeleteAssignment = async () => {
  let isSuccess = await DeleteAssignmentRequest(deleteState.id);
  if (isSuccess) {
    handleCloseDeleteAssignment();
    await GetListAssignment(1, orderName, orderMode);
    await Swal.fire({
      position: 'center',
      icon: 'success',
      title: 'Delete success',
      showConfirmButton: true
    });
  }
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
  let isSuccess = await ReturnAssetRequest(returnAssetState.id);
  if (isSuccess) {
    handleCloseReturnAsset();
    handlePageChange(0);
    await Swal.fire({
      position: 'center',
      icon: 'success',
      title: 'Returning asset success',
      showConfirmButton: true
    });
  }
};

  const handleSort = async (column, sortDirection) => {
    setLoading(true);
    var columnSort = "";
    switch (column.name) {
      case "Asset Name":
        columnSort = "assetName";
        break;
      case "Category":
        columnSort = "category";
        break;
      case "Assigned Date":
        columnSort = "assignedDate";
        break;
      case "State":
          columnSort = "state";
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
    await GetListAssignment(null, columnSort, orderMode, null);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  return (
    <>
      <DataTable
        columns={columns}
        data={assignments}
        // progressPending={loading}
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

      {assignmentDetail && showDetail && (
        <Info assignment={assignmentDetail} handleClose={handleCloseDetail} />
      )}
        <ConfirmModal
        title={acceptState.title}
        isShow={acceptState.isOpen}
        onHide={handleCloseAcceptAssignment}
      >
        <div>
          <div className="">{acceptState.message}</div>
          {acceptState.isDisable && (
            <div className=" mt-3">
              <button
                className="btn btn-danger mr-3"
                onClick={handleConfirmAcceptAssignment}
                type="button"
              >
                Accept
              </button>
              &emsp;&emsp;
              <button
                className="btn btn-outline-secondary"
                onClick={handleCloseAcceptAssignment}
                type="button"
              >
                Cancel
              </button>
            </div>
          )}
        </div>
      </ConfirmModal>


      <ConfirmModal
        title={deleteState.title}
        isShow={deleteState.isOpen}
        onHide={handleCloseDeleteAssignment}
      >
        <div>
          <div className="">{deleteState.message}</div>
          {deleteState.isDisable && (
            <div className=" mt-3">
              <button
                className="btn btn-danger mr-3"
                onClick={handleConfirmDeleteAssignment}
                type="button"
              >
                Decline
              </button>
              &emsp;&emsp;
              <button
                className="btn btn-outline-secondary"
                onClick={handleCloseDeleteAssignment}
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

export default HomeTable;

