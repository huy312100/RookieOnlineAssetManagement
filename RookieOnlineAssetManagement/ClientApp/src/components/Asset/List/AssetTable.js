import React, { useEffect, useState } from "react";
import { FunnelFill, PencilFill, Search, XCircle } from "react-bootstrap-icons";
import ButtonIcon from "../../../shared-components/ButtonIcon";
import ConfirmModal from "../../../shared-components/ConfirmModal";
import ErrorModal from "../../../shared-components/ErrorModal";
import Info from "../Info";
import DataTable from "react-data-table-component";
import { Button } from "react-bootstrap";
import { Link, useHistory } from "react-router-dom";
import { MultiSelect } from "react-multi-select-component";
import {
  getAssetsRequest,
  CheckAssetHistory,
  DeleteAssetRequest,
} from "../services/request";
import { getCategoriesRequest } from "../../Category/services/request";
import Swal from "sweetalert2";
import ReactPaginate from "react-paginate";
import { CREATE_ASSET, EDIT_ASSET_ID } from "../../../Constants/pages";
import AssetModel from "../../../models/AssetModel";
import {convertAssetStateToString} from "../../../utils/State";

const AssetTable = () => {
  //State for category
  const [category, setCategory] = useState([]);
  const [dropdownCategory, setDropdownCategory] = useState([]);
  //State for asset
  const [showDetail, setShowDetail] = useState(false);
  const [assetDetail, setAssetDetail] = useState(null);
  const [selectedState, setSelectedState] = useState([
    { 
      label: 'Available',
      value: 1,
    },
    {
      label: 'Not Available',
      value: 2,
    },
    {
      label: 'Assigned',
      value: 3,
    }
  ]);
  const [selectedCategory, setSelectedCategory] = useState([]);
  const [keyword, setKeyword] = useState("");
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("assetCode");
  const [orderMode, setOrderMode] = useState(1);
  const [assets, setAssets] = useState([]);
  const [disableState, setDisable] = useState({
    isOpen: false,
    id: 0,
    title: "",
    message: "",
    isDisable: true,
  });
  const [disableErrState, setErrDisable] = useState({
    isOpen: false,
    id: 0,
    title: "",
    message1: "",
    message2: "",
    isDisable: true,
  });
  const history = useHistory();
  var assetCreated = history.location.state?.createdAsset;
  var assetUpdated = history.location.state?.assetUpdated;

  useEffect(() => {
    async function fetchData() {

      var pageIndexUpdated = history.location.state?.pageIndex;
      var orderNameUpdated = history.location.state?.orderName;
      var orderModeUpdated = history.location.state?.orderMode;
      GetListCategory();
      if (assetUpdated) {
        await GetListAsset(pageIndexUpdated + 1,orderNameUpdated,orderModeUpdated,assetUpdated);
        history.replace({
          pageIndex: pageIndexUpdated,
          orderName: orderNameUpdated,
          orderMode: orderModeUpdated,
          assetUpdated: null,
        });
      } else if (assetCreated) {
        await GetListAsset(1, orderNameUpdated, orderModeUpdated, null ,assetCreated);
        history.replace();
      } else {  
        await GetListAsset(1, orderName , orderMode, null,null);
      }
    }
    fetchData();
  }, [assetUpdated, assetCreated]);

  // datatable
  const columns = [
    {
      name: "Asset Code",
      selector: (row) => row.assetCode,
      sortable: true,
      width: "17%",
    },
    {
      name: "Asset Name",
      selector: (row) => row.assetName,
      sortable: true,
      minWidth: "50px",
      maxWidth: "350px",
    },
    {
      name: "Category",
      selector: (row) => row.category,
      sortable: true,
      maxWidth: "16%",
    },
    {
      name: "State",
      selector: (row) => row.state,
      sortable: true,
      width: "16%",
    },

    {
      cell: (row, index, column, id) => (
        <div className="row">
          <div className="col-4">
            <ButtonIcon
              onClick={() =>
                row.state === "Assigned" ? null : handleEdit(index)
              }
            >
              {row.state === "Assigned" ? (
                <PencilFill style={{ color: "silver" }} />
              ) : (
                <PencilFill
                  className="text-black"
                  style={{ cursor: "pointer" }}
                />
              )}
            </ButtonIcon>
          </div>
          <div className="col-6">
            <ButtonIcon
              onClick={() =>
                row.state === "Assigned" ? null : checkDisable(row.id, index)
              }
            >
              {row.state === "Assigned" ? (
                <XCircle className="mx-2" style={{ color: "silver" }} />
              ) : (
                <XCircle
                  className="text-danger mx-2 "
                  style={{ cursor: "pointer" }}
                />
              )}
            </ButtonIcon>
          </div>
        </div>
      ),
      button: true,
      width: "10%",
    },
  ];

  const handlePageChange = async (page) => {
    var currentPage = page.selected + 1;
    await GetListAsset(currentPage,orderName,orderMode);
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
      case "Category":
        columnSort = "category";
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
    await GetListAsset(null, columnSort, orderMode);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  const options = [
    { label: "Available", value: 1 },
    { label: "Not Available", value: 2 },
    { label: "Assigned", value: 3 },
    { label: "Waiting for recycling", value: 4 },
    { label: "Recycled", value: 5 },
  ];

  const handleShowInfo = (row, event) => {
    if (row) {
      setAssetDetail(row);
      setShowDetail(true);
    }
  };

  const handleEdit = (index) => {
    history.push(EDIT_ASSET_ID(assets[index].id), {
      existAsset: assets[index],
      pageIndex: pageIndex,
      sortColumn: orderName,
      sortMode: orderMode,
    });
  };
  const handleEditErr = () =>{
    history.push(EDIT_ASSET_ID(assets[disableErrState.id].id), {
      existAsset: assets[disableErrState.id],
      pageIndex: pageIndex,
      sortColumn: orderName,
      sortMode: orderMode,
    });
  }
  const handleCreateAsset = () => {
    history.push(CREATE_ASSET, {
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
      message: "Do you want to delete this Asset?",
      isDisable: true,
    });
  };

  const handleShowErrDisable = async (id) => {
    setErrDisable({
      isOpen: true,
      id,
      title: "Can not Delete Asset",
      message1:
        "Cannot delete the asset because it belongs to one or more historical assignments.",
      message2:
        "If the asset is not able to be used anymore, please update its state in ",
      isDisable: true,
    });
  };

  const handleCloseDisable = () => {
    setDisable({
      isOpen: false,
      id: "",
      title: "",
      message: "",
      isDisable: true,
    });
  };

  const handleCloseErrDisable = () => {
    setErrDisable({
      isOpen: false,
      id: "",
      title: "",
      message1: "",
      message2: "",
      isDisable: true,
    });
  };
  const handleConfirmDisable = async () => {
    let isSuccess = await DeleteAssetRequest(disableState.id);
    if (isSuccess) {
      handleCloseDisable();
      await GetListAsset(1, orderName, orderMode);
      
    await Swal.fire({
      position: 'center',
      icon: 'success',
      title: 'Delete success',
      showConfirmButton: true
    });
    }
  };
  const checkDisable = async (id, index) => {
    let check = await CheckAssetHistory(id);
    if (check.data == true) {
      handleShowErrDisable(index);
    } else {
      handleShowDisable(id);
    }
  };

  const handleCloseDetail = () => {
    setShowDetail(false);
  };

  const GetListAsset = async (page, column, mode, assetItemUpdated,assetItemCreated) => {
    try {
      var listAsset = [];

      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        Keyword: keyword ? keyword : "",
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
        States: selectedState.map((item) => item.value), 
        Categories: selectedCategory.map((item) => item.value) ,
      };
      let response = await getAssetsRequest(params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var totalPages = responseData.pageCount ? responseData.pageCount : 0;
          setTotalPages(totalPages);
          setPageIndex(responseData.pageIndex - 1);
          if (responseData.items.length > 0) {
            responseData.items.forEach((element) => {
              var assetResponse = new AssetModel();
              assetResponse.id = element.id;
              assetResponse.assetCode = element.assetCode;
              assetResponse.assetName = element.assetName;
              assetResponse.category = element.categoryName;
              assetResponse.specification = element.specification;
              assetResponse.installedDate = element.installedDate;
              assetResponse.location = element.locationId;
              assetResponse.state = convertAssetStateToString(element.assetState);
              listAsset.push(assetResponse);
            });

            if (assetItemUpdated) {
              listAsset.sort(function (x, y) {
                return x.id === assetItemUpdated.id
                  ? -1
                  : y === assetItemUpdated
                  ? 1
                  : 0;
              });
            }

            if (assetItemCreated) {
              assetItemCreated.state = convertAssetStateToString(assetItemCreated.state);
              listAsset.pop();
              listAsset.unshift(assetItemCreated);
            }
          }
          setAssets(listAsset);
        } else {
          await Swal.fire({
            icon: "error",
            title: "Can not get list asset",
            // showConfirmButton: true,
          });
        }
      }
    } catch (err) {
      if (err.message.includes("401")) {
        await Swal.fire({
          icon: "error",
          title: "Permission Denied",
          // showConfirmButton: true,
        }).then(function () {
          window.location.href =
            "/Identity/Account/Login?returnUrl=" + window.location.pathname;
        });
      } else {
        await Swal.fire({
          icon: "error",
          title: err,
          // showConfirmButton: true,
        });
      }
    }
  };

  const GetListCategory = async () => {
    try {
      var listCategory = [];
      let response = await getCategoriesRequest();
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          responseData.forEach((element) => {
            listCategory.push({
              label: element.categoryName,
              value: element.id,
            });
          });
        }
        setDropdownCategory(listCategory);
      } else {
        await Swal.fire({
          icon: "error",
          title: "Can not get list category",
          // showConfirmButton: true,
        });
      }
    } catch (err) {
      if (err.message.includes("401")) {
        await Swal.fire({
          icon: "error",
          title: "Permission Denied",
          // showConfirmButton: true,
        }).then(function () {
          window.location.href =
            "/Identity/Account/Login?returnUrl=" + window.location.pathname;
        });
      } else {
        await Swal.fire({
          icon: "error",
          title: err,
          // showConfirmButton: true,
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
            <button className="btn border w-25" onClick={() => GetListAsset(1,orderMode)}>
              <FunnelFill />
            </button>
          </div>
        </div>

        <div className="d-flex align-items-center col-md-12 col-lg-12 col-xl-3 mt-2 mb-2">
          <div className="input-group" style={{ zIndex: 9 }}>
            <MultiSelect
              options={dropdownCategory}
              value={selectedCategory}
              className="w-75"
              onChange={setSelectedCategory}
              disableSearch={true}
              valueRenderer={() => <span>Category</span>}
              ClearSelectedIcon={() => ""}
              ArrowRenderer={() => ""}
            />
            <button className="btn border w-25" onClick={() => GetListAsset(1,orderMode)}>
              <FunnelFill />
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
                GetListAsset(1,"assetCode",null,null,null,"Keyword");
              }}
            >
              <Search />
            </div>
          </div>
        </div>

        <div className="d-flex col-md-6 col-lg-6 col-xl-3 mt-2 mb-2">
          <Button onClick={handleCreateAsset} className="btn btn-danger">
            Create new asset
          </Button>
        </div>
      </div>
      <DataTable
        columns={columns}
        data={assets}
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

      {assetDetail && showDetail && (
        <Info asset={assetDetail} handleClose={handleCloseDetail} />
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
      <ErrorModal
        title={disableErrState.title}
        isShow={disableErrState.isOpen}
        onHide={handleCloseErrDisable}
      >
        <div>
          <div>{disableErrState.message1}</div>
          <div className="mt-1">
            {disableErrState.message2}
            <a onClick={handleEditErr} className={"link-primary"} style={{cursor:"pointer"}}>
              Edit Asset page
            </a>
          </div>
        </div>
      </ErrorModal>
    </>
  );
};

export default AssetTable;
