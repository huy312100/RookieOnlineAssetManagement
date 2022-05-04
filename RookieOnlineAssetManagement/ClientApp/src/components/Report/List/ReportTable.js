import React, { useEffect, useState, useRef } from "react";
import { Button } from "react-bootstrap";
import ReactPaginate from "react-paginate";
import DataTable from "react-data-table-component";
import ReportModel from "../../../models/ReportModel";
import Swal from "sweetalert2";
import { getReportRequest } from "../services/request";
import * as FileSaver from "file-saver";
import * as XLSX from "xlsx";

const ReportTable = () => {
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);
  const [orderName, setOrderName] = useState("");
  const [orderMode, setOrderMode] = useState(1);
  const [reports, setReports] = useState([]);

  const fileType =
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8";
  const fileExtension = "xlsx";
  const fileName = "ReportOnlineAssetManagement";
  const columns = [
    {
      name: "Category",
      selector: (row) => row.category,
      sortable: true,
      width: "20%",
      wrap: true,
    },
    {
      name: "Total",
      selector: (row) => row.total,
      sortable: true,
      width: "10%",
    },
    {
      name: "Assigned",
      selector: (row) => row.assigned,
      sortable: true,
      width: "12%",
    },
    {
      name: "Available",
      selector: (row) => row.available,
      sortable: true,
      width: "12%",
    },
    {
      name: "Not available",
      selector: (row) => row.notAvailable,
      sortable: true,
      width: "15%",
    },
    {
      name: "Waiting for recycling",
      selector: (row) => row.waitingForRecycling,
      sortable: true,
      width: "19%",
    },
    {
      name: "Recycled",
      selector: (row) => row.recycled,
      sortable: true,
      width: "12%",
    },
  ];

  const headers = [
    { label: "Category", key: "category" },
    { label: "Total", key: "total" },
    { label: "Assigned", key: "assigned" },
    { label: "Available", key: "available" },
    { label: "Not available", key: "notAvailable" },
    { label: "Waiting for recycling", key: "waitingForRecycling" },
    { label: "Recycled", key: "recycled" },
  ];

  const csvInstance = useRef();

  useEffect(() => {
    GetListReport(1, null, 1, false);
  }, []);

  const GetListReport = async (page, column, mode, isGetAll) => {
    try {
      var listReport = [];

      var params = {
        GetAll: isGetAll ? isGetAll : false,
        PageIndex: page ? page : pageIndex + 1,
        PageSize: pageSize,
        OrderName: column ? column : orderName,
        OrderMode: mode ? mode : orderMode,
      };
      let response = await getReportRequest(params);
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        if (responseData) {
          var totalPages = responseData.pageCount ? responseData.pageCount : 0;
          if (!isGetAll) setTotalPages(totalPages);
          if (!isGetAll) setPageIndex(responseData.pageIndex - 1);
          responseData.items.forEach((element) => {
            var reportRs = new ReportModel();
            reportRs.assigned = element.assigned ? element.assigned : 0;
            reportRs.available = element.available ? element.available : 0;
            reportRs.category = element.category ? element.category : "";
            reportRs.notAvailable = element.notAvailable
              ? element.notAvailable
              : 0;
            reportRs.recycled = element.recycled ? element.recycled : 0;
            reportRs.total = element.total ? element.total : 0;
            reportRs.waitingForRecycling = element.waitingForRecycling
              ? element.waitingForRecycling
              : 0;

            listReport.push(reportRs);
          });
          if (isGetAll) {
            exportToCSV(listReport);
          } else setReports(listReport);
        }
      } else {
        await Swal.fire({
          icon: "error",
          title: "Can not get list report",
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
    await GetListReport(currentPage);
  };

  const handleSort = async (column, sortDirection) => {
    var columnSort = "";
    switch (column.name) {
      case "Category":
        columnSort = "category";
        break;
      case "Total":
        columnSort = "total";
        break;
      case "Assigned":
        columnSort = "assigned";
        break;
      case "Available":
        columnSort = "available";
        break;
      case "Not available":
        columnSort = "notAvailable";
        break;
      case "Waiting for recycling":
        columnSort = "waitingForRecycling";
        break;
      case "Recycled":
        columnSort = "recycled";
        break;

      default: {
        columnSort = "category";
        break;
      }
    }
    var orderMode = 1;
    if (sortDirection === "asc") {
      orderMode = 1;
    } else {
      orderMode = 2;
    }
    await GetListReport(null, columnSort, orderMode, false);
    setOrderName(columnSort);
    setOrderMode(orderMode);
  };

  const getAllReport = async () => {
    await GetListReport(1, orderName, orderMode, true);
  };

  const exportToCSV = (exportData) => {
    //const ws = XLSX.utils.json_to_sheet(exportData);
    var Heading = [
      [
        "Category",
        "Total",
        "Assigned",
        "Available",
        "Not available",
        "Waiting for recycling",
        "Recycled",
      ],
    ];

    //Had to create a new workbook and then add the header
    const ws = XLSX.utils.book_new();
    XLSX.utils.sheet_add_aoa(ws, Heading);

    //Starting in the second row to avoid overriding and skipping headers
    XLSX.utils.sheet_add_json(ws, exportData, {
      origin: "A2",
      skipHeader: true,
    });
    const wb = { Sheets: { Report: ws }, SheetNames: ["Report"] };
    const excelBuffer = XLSX.write(wb, {
      bookType: fileExtension,
      type: "array",
    });
    const data = new Blob([excelBuffer], { type: fileType });
    FileSaver.saveAs(data, fileName + "." + fileExtension);
  };

  return (
    <>
      <div className="row" style={{ padding: "15px" }}>
        <div className="d-flex col-md-12 col-lg-12 col-xl-12 justify-content-end">
          <Button className="btn btn-danger" onClick={getAllReport}>
            Export
          </Button>
        </div>
      </div>
      <DataTable
        columns={columns}
        data={reports}
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
      />
    </>
  );
};
export default ReportTable;
