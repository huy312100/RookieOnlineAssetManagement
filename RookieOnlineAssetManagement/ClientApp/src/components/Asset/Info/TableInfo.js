import React, { useEffect, useState } from "react";

import DataTable from "react-data-table-component";
import {
  getHistoryRequest,
} from "../services/request";
import Swal from "sweetalert2";
import ReactPaginate from "react-paginate";
import AssetHistoryModel from "../../../models/AssetHistoryModel";
import {getFormatDateTime} from "../../../utils/Date";

const TableInfo = ({assetId}) => {

  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("");
  const [orderMode, setOrderMode] = useState(1);
  const [historyAsset, setHistoryAsset] = useState([]);

  // datatable
  const columns = [
    {
      name: "Date",
      selector: (row) => row.assignedDate,
      sortable: true,
      width: "25%",
    },
    {
      name: "Assigned to",
      selector: (row) => row.assignedTo,
      sortable: true,
      width: "25%",
    },
    {
      name: "Assigned by",
      selector: (row) => row.assignedBy,
      width: "25%",
    },
    {
      name: "Returned Date",
      selector: (row) => row.returnedDate==="1/1/1"?null:row.returnedDate,
      sortable: true,
      width: "25%",
    },
  ];

  useEffect(() => {
    GetListHistoryAsset(1,null,1);
  },[]);

  const handlePageChange = async (page) => {
    var currentPage = page.selected + 1;
    await GetListHistoryAsset(currentPage);
  };

  const GetListHistoryAsset = async (page, column, mode) => {
    try{
      var listAssetHistory = [];
      var params = {
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
      };
      let response = await getHistoryRequest(assetId,params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var totalPages = responseData.pageCount ? responseData.pageCount : 0;
          setTotalPages(totalPages);
          setPageIndex(responseData.pageIndex - 1);
          if(responseData.items.length > 0) {
            responseData.items.forEach((element) => {
              var assetHistoryResponse = new AssetHistoryModel();
              assetHistoryResponse.assignedDate=element.assignedDate === null ? "" : getFormatDateTime(element.assignedDate);
              assetHistoryResponse.returnedDate=element.returnedDate === null ? "" : getFormatDateTime(element.returnedDate);
              assetHistoryResponse.assignedBy=element.assignedBy;
              assetHistoryResponse.assignedTo=element.assignedTo;
              listAssetHistory.push(assetHistoryResponse);
            });
          }
          setHistoryAsset(listAssetHistory);
        }
        else{
          await Swal.fire({
            icon: "error",
            title: "Can not get list asset history",
            // showConfirmButton: true,
          });
        }
      }
    }
    catch(err){
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
  }

  return (
    <>
      <DataTable
        columns={columns}
        data={historyAsset}
        sortServer
        responsive={true}
        fixedHeader={true}
        fixedHeaderScrollHeight="20vh"
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
    </>
  );
};

export default TableInfo;
