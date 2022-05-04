import React, { useEffect, useState } from "react";
import "./index.css";
import DataTable from "react-data-table-component";
import ReactPaginate from "react-paginate";
import { BiSearch } from "react-icons/bi";
import AssetModel from "../../../../models/AssetModel";
import { getAssetsRequest } from "../../../Asset/services/request";
import Swal from "sweetalert2";

const SelectAsset = ({ pref, assetSelected, sendAssetSelected }) => {
  const [assets, setAssets] = useState([]);
  const [loading, setLoading] = useState(false);
  const [totalPages, setTotalPages] = useState(0);
  const [selectedAsset, setSelectedAsset] = useState(assetSelected);
  const [selectedNewAsset, setSelectedNewAsset] = useState(null);
  const [keyword, setKeyword] = useState("");
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("");
  const [orderMode, setOrderMode] = useState(1);
  const [canSubmit, setCanSubmit] = useState(false);

  //   useEffect(() => {
  //     return () => sendAssetSelected(selectedAsset);
  //   }, [selectedAsset]);

  useEffect(() => {
    if (selectedNewAsset) {
      setCanSubmit(true);
    } else setCanSubmit(false);
  }, [selectedNewAsset]);

  useEffect(() => {
    GetListAsset(1, null, 1);
  }, []);
  // datatable
  const columns = [
    {
      cell: (row, index, column, id) => (
        <input
          className="form-check-input custom-control-input"
          type="radio"
          name="selectUser"
          id={"select_asset_" + row.id}
          onClick={() => handleSelectNewAsset(row)}
          value={id}
        />
      ),
      button: true,
      width: "5%",
    },
    {
      name: "Asset Code",
      selector: (row) => row.assetCode,
      sortable: true,
      width: "20%",
    },
    {
      name: "Asset Name",
      selector: (row) => row.assetName,
      sortable: true,
      width: "50%",
      wrap: true,
    },
    {
      name: "Category",
      selector: (row) => row.category,
      sortable: true,
      width: "25%",
    },
  ];

  const GetListAsset = async (page, column, mode) => {
    try {
      setLoading(true);
      setSelectedNewAsset(null);
      var listAsset = [];

      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        Keyword: keyword ? keyword : "",
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
        States: ["1"],
        Categories: [],
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
              if (element.assetState === 1) {
                assetResponse.state = "Available";
              } else if (element.assetState === 2) {
                assetResponse.state = "Not Available";
              } else if (element.assetState === 3) {
                assetResponse.state = "Assigned";
              } else if (element.assetState === 4) {
                assetResponse.state = "Waiting for recycling";
              } else if (element.assetState === 5) {
                assetResponse.state = "Recycled";
              } else {
                assetResponse.state = "";
              }
              listAsset.push(assetResponse);
            });
          }
          setAssets(listAsset);
          setLoading(false);
          if (selectedAsset && selectedAsset.id) {
            var assetChosed = document.getElementById(
              "select_asset_" + selectedAsset.id
            );
            if (assetChosed) {
              assetChosed.checked = true;
            }
          }
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

  const handlePageChange = async (page) => {
    var currentPage = page.selected + 1;
    await GetListAsset(currentPage);
  };

  const handleSort = async (column, sortDirection) => {
    setLoading(true);
    var columnSort = "";
    switch (column.name) {
      case "Asset Code":
        columnSort = "assetCode";
        break;
      case "Asset Name":
        columnSort = "assetName";
        break;
      case "Category":
        columnSort = "category";
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
    await GetListAsset(null, columnSort, orderMode);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  const handleSelectNewAsset = (row) => {
    setSelectedNewAsset(row);
  };

  const SaveSelectedAsset = () => {
    setSelectedAsset(selectedNewAsset);
    sendAssetSelected(selectedNewAsset);
  };

  const handleCancelSelectAsset = () => {
    sendAssetSelected(selectedAsset);
  };

  return (
    <>
      <div ref={pref} className="containerModalSelect">
        <div className="body bodySelect">
          <div className="container-fluid">
            <div className="row" style={{ padding: "5px" }}>
              <div className="d-flex align-items-left col-md-6 col-lg-6 col-xl-6">
                <h5 className="fw-bold text-danger">Select Asset</h5>
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
                    onClick={() => GetListAsset(1, null, null)}
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
                data={assets}
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
                    breakLinkClassName="page-link"
                    containerClassName="pagination mt-3"
                    activeClassName="active"
                    onPageChange={handlePageChange}
                    forcePage={pageIndex}
                  />
                )}
                //onRowClicked={(row, event) => handleShowInfo(row, event)}
              />
            </div>
            <div className="row mt-4 mb-4 pr-2">
              <div className="col-lg-12 col-md-12 text-end">
                {canSubmit ? (
                  <button
                    className="btn btn-danger px-4 me-3"
                    onClick={SaveSelectedAsset}
                  >
                    Save
                  </button>
                ) : (
                  <button className="btn btn-danger px-4 me-3" disabled>
                    Save
                  </button>
                )}

                <button
                  onClick={handleCancelSelectAsset}
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

export default SelectAsset;
