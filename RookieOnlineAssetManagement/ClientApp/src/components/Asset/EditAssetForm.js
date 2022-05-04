import React, { useEffect, useState } from "react";
import { Formik, Form, Field } from "formik";
import Swal from "sweetalert2";
import { MdOutlineArrowDropDown } from "react-icons/md";
import { useHistory } from "react-router-dom";
import { ASSET } from "../../Constants/pages";
import AssetRequestModel from "../../models/AssetRequestModel";
import AssetModel from "../../models/AssetModel";
import { updateAssetRequest } from "./services/request";

const EditAssetFormContainer = ({ initialAssetForm = {} }) => {
  //History
  const history = useHistory();
  const asset = history.location.state?.existAsset;
  const pageIndex = history.location.state?.pageIndex;
  const sortColumn = history.location.state?.sortColumn;
  const sortMode = history.location.state?.sortMode;

  //State for update asset
  const [assetName, setAssetName] = useState(asset.assetName);
  const [assetSpecification, setAssetSpecification] = useState(
    asset.specification
  );
  const [assetInstalledDate, setAssetInstalledDate] = useState(
    new Date(asset.installedDate)
  );
  const [assetState, setAssetState] = useState(1);

  useEffect(() => {
    switch (asset.state) {
      case "Available":
        setAssetState(1);
        break;
      case "Not Available":
        setAssetState(2);
        break;
      case "Waiting for recycling":
        setAssetState(4);
        break;
      case "Recycled":
        setAssetState(5);
        break;
      default:
        setAssetState(1);
    }
  }, []);

  const handleCancelEdit = () => {
    history.push(ASSET, {
      pageIndex: pageIndex,
      orderName: sortColumn,
      orderMode: sortMode,
    });
  };

  const UpdateAsset = async () => {
    try {
      console.log(assetInstalledDate);
      var request = new AssetRequestModel();
      var assetInstalledDateSend = assetInstalledDate;
      if (typeof assetInstalledDate !== "string") {
        assetInstalledDateSend = assetInstalledDate.toDateString();
      }
      var formUpdateData = request.GetUpdateAssetFormData(
        assetName,
        assetSpecification,
        assetInstalledDateSend,
        assetState
      );

      let result = await updateAssetRequest(asset.id, formUpdateData);
      if (result.status === 200) {
        var updatedAsset = new AssetModel();
        updatedAsset.id = asset.id;
        updatedAsset.assetCode = asset.assetCode;
        updatedAsset.assetName = assetName;
        updatedAsset.category = asset.category;
        updatedAsset.specification = assetSpecification;
        updatedAsset.installedDate = assetInstalledDate;
        updatedAsset.location = asset.location;
        updatedAsset.state = assetState;

        history.push(ASSET, {
          pageIndex: pageIndex,
          orderName: sortColumn,
          orderMode: sortMode,
          assetUpdated: updatedAsset,
        });
        return;
      }
    } catch (err) {
      if (err.response.status === 401 || err.response.status === 403) {
        Swal.fire({
          title: "Error",
          text: "You are not authorized to access this page",
          icon: "error",
          confirmButtonText: "OK",
        });
      }
      else if(err.response.data.errors){
        if(err.response.data.errors.AssetName){
          Swal.fire({
            title: "Error",
            text: err.response.data.errors.AssetName[0],
            icon: "error",
            confirmButtonText: "OK",
          });
        }
      }
      else {
        Swal.fire({
          title: "Error",
          text: "Something went wrong",
          icon: "error",
          confirmButtonText: "OK",
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
                  value={assetName}
                  onChange={(e) => setAssetName(e.target.value)}
                />
              </div>
            </div>

            <div className="row mt-4">
              <div className="col-4">
                <label className="form-label">Category</label>
              </div>

              <div className="col-8">
                <div
                  style={Object.assign(
                    {
                      display: "flex",
                      height: "30px",
                      backgroundColor: "silver",
                    },
                    styleInputField
                  )}
                >
                  <span
                    style={{
                      fontSize: "16px",
                      opacity: "0.5",
                      marginLeft: "10px",
                      marginRight: "auto",
                    }}
                  >
                    {asset.category}
                  </span>

                  <MdOutlineArrowDropDown
                    style={{
                      fontSize: "25px",
                      color: "grey",
                      marginLeft: "auto",
                    }}
                  />
                </div>
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
                  value={assetSpecification}
                  onChange={(e) => {
                    setAssetSpecification(e.target.value);
                  }}
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
                  name="assetInstalledDate"
                  style={styleInputField}
                  className={"w-80 form-control form-control-sm"}
                  onKeyDown={(e) => e.preventDefault()}
                  value={
                    new Date(asset.installedDate).getTime() ===
                    new Date(assetInstalledDate).getTime()
                      ? assetInstalledDate.getFullYear() +
                        "-" +
                        ("0" + (assetInstalledDate.getMonth() + 1)).slice(-2) +
                        "-" +
                        ("0" + assetInstalledDate.getDate()).slice(-2)
                      : assetInstalledDate
                  }
                  onChange={(e) => {
                    setAssetInstalledDate(e.target.value);
                  }}
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
                      checked={assetState === 1}
                      onClick={() => setAssetState(1)}
                    />
                    Available
                  </label>
                </div>
                <div style={{ marginBottom: "5px" }}>
                  <label>
                    <Field
                      className="form-check-input custom-control-input"
                      type="radio"
                      style={{ marginRight: "5px" }}
                      //   name="stateAsset"
                      value="2"
                      checked={assetState === 2}
                      onChange={() => setAssetState(2)}
                    />
                    Not available
                  </label>
                </div>
                <div style={{ marginBottom: "5px" }}>
                  <label>
                    <Field
                      className="form-check-input custom-control-input"
                      type="radio"
                      style={{ marginRight: "5px" }}
                      // name="stateAsset"
                      value="4"
                      checked={assetState === 4}
                      onChange={() => setAssetState(4)}
                    />
                    Waiting for recycling
                  </label>
                </div>
                <div>
                  <label>
                    <Field
                      className="form-check-input custom-control-input"
                      type="radio"
                      style={{ marginRight: "5px" }}
                      // name="stateAsset"
                      value="5"
                      checked={assetState === 5}
                      onChange={() => setAssetState(5)}
                    />
                    Recycled
                  </label>
                </div>
              </div>
            </div>

            <div className="row mt-4 pr-2">
              <div className="col-lg-12 col-md-12 text-end">
                <button
                  className="btn btn-danger px-4 me-3"
                  onClick={UpdateAsset}
                >
                  Save
                </button>

                <button
                  onClick={handleCancelEdit}
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
  color: "#000",
  borderRadius: "5px",
  border: "1px solid #ced4da",
};

export default EditAssetFormContainer;
