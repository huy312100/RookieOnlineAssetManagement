import React, { useState, useEffect } from "react";
import { Formik, Form, Field, ErrorMessage } from "formik";
import "../../App.css";
import "./Dropdown.css";
import { MdOutlineArrowDropDown } from "react-icons/md";
import { ImCross } from "react-icons/im";
import { FaCheck } from "react-icons/fa";
import { Dropdown } from "react-bootstrap";
import Swal from "sweetalert2";
import { useHistory } from "react-router-dom";
import { ASSET } from "../../Constants/pages";
import {
  getCategoriesRequest,
  createCategoryRequest,
} from "../Category/services/request";
import { createAssetRequest } from "./services/request";
import CategoryModel from "../../models/CategoryModel";
import CategoryRequestModel from "../../models/CategoryRequestModel";
import AssetRequestModel from "../../models/AssetRequestModel";
import AssetModel from "../../models/AssetModel";

const AssetFormContainer = ({initialAssetForm = {}}) => {
  const [categoryDropdownShow, setCategoryDropdownShow] = useState(false);
  const [addCategory, setAddCategory] = useState(false);
  const [categoryAsset, setCategoryAsset] = useState("");
  const [categoryDisplayDropdown, setCategoryDisplayDropdown] = useState("");
  const [disableBtnCreateCategory, setDisableBtnCreateCategory] =
    useState(true);

  //State for create api category
  const [dataCreateCategory,setDataCreateCategory] = useState({
    categoryName: "",
    categoryPrefix: "",
  });

  //State for create api actions
  const [dataCreateAsset,setDataCreateAsset] = useState({
    nameAsset:"",
    selectedCategoryId:"",
    specificationAsset:"",
    installedDateAsset:"",
    stateAsset:1,
  });
  const [canSubmit, setCanSubmit] = useState(false);

  //History
  const history = useHistory();
  const pageIndex = history.location.state?.pageIndex;
  const sortColumn = history.location.state?.sortColumn;
  const sortMode = history.location.state?.sortMode;

  useEffect(() => {
    GetListCategory();
    if (dataCreateCategory.categoryName !== "" && dataCreateCategory.categoryPrefix !== "") {
      setDisableBtnCreateCategory(false);
    }
    else{
      setDisableBtnCreateCategory(true);
    }
  }, [dataCreateCategory]);

  useEffect(() => {
    if (
      dataCreateAsset.nameAsset !== "" &&
      dataCreateAsset.selectedCategoryId !== "" &&
      dataCreateAsset.specificationAsset !== "" &&
      dataCreateAsset.installedDateAsset !== ""
    ) {
      setCanSubmit(true);
    }
    else{
      setCanSubmit(false);  
    }
  }, [dataCreateAsset]);

  const handleCancelCreate = () => {
    history.push(ASSET, {
      pageIndex: pageIndex,
      orderName: sortColumn,
      orderMode: sortMode,
    });
  };

  const GetListCategory = async () => {
    try {
      var listCategory = [];
      let response = await getCategoriesRequest();
      if (response.status === 200) {
        let responseData = response.data ? response.data : "";
        console.log(responseData);
        if (responseData) {
          responseData.forEach((element) => {
            var categoryResponse = new CategoryModel();
            categoryResponse.id = element.id;
            categoryResponse.categoryName = element.categoryName;
            categoryResponse.prefix = element.prefix;
            listCategory.push(categoryResponse);
          });
        }
        setCategoryAsset(listCategory);
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
        }).then(function () {
          window.location.href =
            "/Identity/Account/Login?returnUrl=" + window.location.pathname;
        });
      } else {
        await Swal.fire({
          icon: "error",
          title: err,
        });
      }
    }
  };

  const CreateCategory = async () => {
    try {
      var request = new CategoryRequestModel();
      var formCreateData = request.GetCreateCategoryFormData(
        dataCreateCategory.categoryName,
        dataCreateCategory.categoryPrefix
      );
      let result = await createCategoryRequest(formCreateData);
      if (result.status === 201) {
        var categoryResponse = result.data.category
          ? result.data.category
          : null;
        if (categoryResponse) {
        }
        setDisableBtnCreateCategory(true);
        return;
      }
    } catch (err) {
      console.log(err);
      if (err.response.status === 401 || err.response.status === 403) {
        await Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Permission Denied",
        });
      } else if (err.response.status === 400) {
        // setErrors(e.response.data.errors);
        if (err.response.data.errors) {
          await Swal.fire({
            icon: "error",
            title: "Error",
            text: err.response.data.errors.Prefix,
          });
        } else {
          await Swal.fire({
            icon: "error",
            title: "Error",
            text: err.response.data,
          });
        }
      } else {
        await Swal.fire({
          icon: "error",
          title: "Oops...",
          text: err,
        });
      }
      setDisableBtnCreateCategory(true);
    }
  };

  const CreateAsset = async () => {
    try {
      var request = new AssetRequestModel();
      var formCreateData = request.GetCreateAssetFormData(
        dataCreateAsset.nameAsset,
        dataCreateAsset.selectedCategoryId,
        dataCreateAsset.specificationAsset,
        dataCreateAsset.installedDateAsset,
        dataCreateAsset.stateAsset
      );
      let result = await createAssetRequest(formCreateData);
      if (result.status === 201) {
        var assetResponse = result.data.asset ? result.data.asset : null;
        var createdAsset = new AssetModel();
        createdAsset.id = assetResponse.id;
        createdAsset.assetCode = assetResponse.assetCode;
        createdAsset.assetName=assetResponse.assetName;
        createdAsset.category = assetResponse.categoryName;
        createdAsset.specification = assetResponse.specification;
        createdAsset.installedDate = assetResponse.installedDate;
        createdAsset.location = assetResponse.locationId;
        createdAsset.state = assetResponse.assetState;

        if (assetResponse) {
          history.push(ASSET, {
            createdAsset: createdAsset,
            pageIndex: pageIndex,
            orderName: sortColumn,
            orderMode: sortMode,
          });
        }
        return;
      }
    } catch (err) {
      console.log(err);
      if (err.response.status === 401 || err.response.status === 403) {
        await Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Permission Denied",
        });
      } else if (err.response.status === 400) {
        if(err.response.data.errors.AssetName){
          await Swal.fire({
            icon: "error",
            title: "Error",
            text: err.response.data.errors.AssetName,
          });
        }
        else if(err.response.data.errors.Specification){
          await Swal.fire({
            icon: "error",
            title: "Error",
            text: err.response.data.errors.Specification,
          });
        }
        else{
          await Swal.fire({
            icon: "error",
            title: "Error",
            text: "Create asset failed. Please try again !",
          });
        }
      } else {
        await Swal.fire({
          icon: "error",
          title: "Oops...",
          text: err,
        });
      }
    }
  };

  return (
    <>
      <div>
        <Formik initialValues={initialAssetForm} enableReinitialize>
          <Form>
            <div className="row mt-4">
              <div className="col-4">
                <label className="form-label">Name</label>
              </div>
              <div className="col-8">
                <Field
                  name="assetName"
                  type="text"
                  className={"w-80 form-control form-control-sm"}
                  style={styleInputField}
                  onChange={(e) => 
                    setDataCreateAsset({
                      ...dataCreateAsset,
                      nameAsset:e.target.value,
                    })
                  }
                />
              </div>
            </div>

            <div className="row mt-4">
              <div className="col-4">
                <label className="form-label">Category</label>
              </div>
              <div className="col-8">
                <Dropdown
                  onToggle={() => {
                    setCategoryDropdownShow(!categoryDropdownShow);
                    setAddCategory(false);
                  }}
                  show={categoryDropdownShow}
                  collapseOnSel
                  autoClose={false}
                >
                  <Dropdown.Toggle style={styleInputField}>
                    <div
                      style={{
                        display: "flex",
                        height: "14px",
                        width: "100%",
                      }}
                    >
                      <span
                        style={{
                          fontSize: "14px",
                          marginRight: "auto",
                        }}
                      >
                        {categoryDisplayDropdown}
                      </span>

                      <MdOutlineArrowDropDown
                        style={{
                          fontSize: "18px",
                          color: "grey",
                          marginLeft: "auto",
                        }}
                      />
                    </div>
                  </Dropdown.Toggle>
                  <Dropdown.Menu
                    style={{
                      overflowY: "scroll",
                      width: "80%",
                    }}
                  >
                    {categoryAsset &&
                      categoryAsset.map((item) => (
                        <Dropdown.Item
                          key={item.id}
                          onClick={() => {
                            setDataCreateAsset({
                              ...dataCreateAsset,
                              selectedCategoryId:item.id,
                            })
                            setCategoryDisplayDropdown(item.categoryName);
                            setCategoryDropdownShow(false);
                          }}
                        >
                          {item.categoryName}
                        </Dropdown.Item>
                      ))}

                    <Dropdown.Divider />

                    {addCategory ? (
                      <Dropdown.Item
                        onClick={() => {
                          setCategoryDropdownShow(true);
                        }}
                      >
                        <div>
                          <input
                            type="text"
                            id="categoryInput"
                            style={{ width: "50%" }}
                            onChange={(e) => {
                              e.preventDefault();
                              console.log(e.target.value);
                              setDataCreateCategory({
                                ...dataCreateCategory,
                                categoryName: e.target.value,
                              });
                            }}
                            onKeyDown={(e) => e.stopPropagation()}
                            onKeyUp={(e) => e.stopPropagation()}
                            onKeyPress={(e) => e.stopPropagation()}
                          />
                          <input
                            type="text"
                            style={{ width: "20%" }}
                            maxLength="2"
                            onKeyDown={(e) => {
                              if (/^[0-9]$/.test(e.key)) {
                                e.preventDefault();
                              }
                            }}
                            onChange={(e) => {
                              setDataCreateCategory({
                                ...dataCreateCategory,
                                categoryPrefix: e.target.value,
                              })
                            }}
                          />
                          {disableBtnCreateCategory ? (
                            <FaCheck
                              style={{
                                color: "grey",
                                margin: "0 10px 0 20px",
                              }}
                            />
                          ) : (
                            <FaCheck
                              onClick={async () => {
                                await CreateCategory();
                                setAddCategory(false);
                                setDataCreateCategory({
                                  categoryName: "",
                                  categoryPrefix: "",
                                });
                              }}
                              style={{
                                color: "red",
                                margin: "0 10px 0 20px",
                              }}
                            />
                          )}
                          <ImCross
                            onClick={() => {
                              setAddCategory(false);
                              setDataCreateCategory({
                                categoryName: "",
                                categoryPrefix: "",
                              });
                            }}
                            style={{
                              fontSize: "12px",
                            }}
                          />
                        </div>
                      </Dropdown.Item>
                    ) : (
                      <Dropdown.Item
                        onClick={() => {
                          setAddCategory(true);
                          //   setCategoryDropdownShow(true);
                        }}
                      >
                        <span
                          style={{
                            color: "red",
                            fontSize: "14px",
                            cursor: "pointer",
                            textDecoration: "underline",
                            fontStyle: "italic",
                          }}
                        >
                          Add new category
                        </span>
                      </Dropdown.Item>
                    )}
                  </Dropdown.Menu>
                </Dropdown>
              </div>
            </div>

            <div className="row mt-4">
              <div className="col-4">
                <label className="form-label">Specification</label>
              </div>
              <div className="col-8">
                <Field
                  style={styleInputField}
                  name="specification"
                  className={"w-80 form-control form-control-sm"}
                  rows="4"
                  component="textarea"
                  classname="form-control"
                  onChange={(e) => 
                    setDataCreateAsset({
                      ...dataCreateAsset,
                      specificationAsset:e.target.value,
                    })
                  }
                />
              </div>
            </div>

            <div className="row mt-4">
              <div className="col-4">
                <label className="form-label">Installed Date</label>
              </div>
              <div className="col-8">
                <Field
                  type="date"
                  name="installedDate"
                  style={styleInputField}
                  className={"w-80 form-control form-control-sm"}                  
                  onKeyDown={(e) => e.preventDefault()}
                  onChange={(e) => 
                    setDataCreateAsset({
                      ...dataCreateAsset,
                      installedDateAsset:e.target.value,
                    })
                  }
                />
              </div>
            </div>

            <div className="row mt-4">
              <div className="col-4">
                <label className="form-label">State</label>
              </div>
              <div className="col-8">
                <div style={{ marginBottom: "5px" }}>
                  <label>
                    <Field
                      className="form-check-input custom-control-input"
                      type="radio"
                      style={{ marginRight: "5px" }}
                      // name="state"
                      value="1"
                      checked={dataCreateAsset.stateAsset === 1}
                      onClick={(e) => 
                        setDataCreateAsset({
                          ...dataCreateAsset,
                          stateAsset:1,
                        })
                      }
                    />
                    Available
                  </label>
                </div>
                <div>
                  <label>
                    <Field
                      className="form-check-input custom-control-input"
                      type="radio"
                      style={{ marginRight: "5px" }}
                      // name="stateAsset"
                      value="2"
                      checked={dataCreateAsset.stateAsset === 2}
                      onChange={(e) => 
                        setDataCreateAsset({
                          ...dataCreateAsset,
                          stateAsset:2,
                        })
                      }
                    />
                    Not available
                  </label>
                </div>
              </div>
            </div>

            <div className="row mt-4 pr-2">
              <div className="col-lg-12 col-md-12 text-end">
                {canSubmit ? (
                  <button
                    className="btn btn-danger px-4 me-3"
                      onClick={CreateAsset}
                  >
                    Save
                  </button>
                ) : (
                  <button className="btn btn-danger px-4 me-3" disabled>
                    Save
                  </button>
                )}

                <button
                  onClick={handleCancelCreate}
                  className="btn btn-outline-dark px-4 ms-3"
                >
                  Cancel
                </button>
              </div>
            </div>
          </Form>
        </Formik>
      </div>
    </>
  );
};

const styleInputField = {
  width: "80%",
  backgroundColor: "#FFF",
  color: "#000",
  borderRadius: "5px",
  border: "1px solid #ced4da",
};

export default AssetFormContainer;
