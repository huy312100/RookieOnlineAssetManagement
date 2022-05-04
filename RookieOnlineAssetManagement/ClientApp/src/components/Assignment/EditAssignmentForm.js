import { useState, useEffect, useRef } from "react";
import { Field, Form, Formik } from "formik";
import Swal from "sweetalert2";
import { MdOutlineArrowDropDown } from "react-icons/md";
import { useHistory } from "react-router-dom";
import { ASSIGNMENT } from "../../Constants/pages";
import SelectAsset from "./Update/SelectTable/SelectAsset";
import SelectUser from "./Update/SelectTable/SelectUser";
import AssignmentRequestModel from "../../models/AssignmentRequestModel";
import AssignmentModel from "../../models/AssignmentModel";
import { updateAssignmentRequest} from "./services/request";

const EditAssignmentFormContainer = ({ initialAssignmentForm = {} }) => {
  //History
  const history = useHistory();
  const assignment = history.location.state?.existAssignment;
  const pageIndex = history.location.state?.pageIndex;
  const sortColumn = history.location.state?.sortColumn;
  const sortMode = history.location.state?.sortMode;

  //State for selected
  const [showSelectUser, setShowSelectUser] = useState(false);
  const [showSelectAsset, setShowSelectAsset] = useState(false);
  const [userSelected, setUserSelected] = useState(null);
  const [assetSelected, setAssetSelected] = useState(null);
  
  //State for display in main view
  const [showSelectUserMainDisplay, setShowSelectUserMainDisplay] = useState(assignment.fullnameAssigned);
  const [showSelectAssetMainDisplay, setShowSelectAssetMainDisplay] = useState(assignment.assetName);

  //State for edit assignment
  const [dataEditAssignment, setDataEditAssignment] = useState({
    assignmentUserId:assignment.userIdAssigned,
    assignmentAssetId:assignment.assetId,
    assignmentNote:assignment.note,
    assignedDate:new Date(assignment.assignedDate),
  });

  const handleCancelEdit = () => {
    history.push(ASSIGNMENT, {
      pageIndex: pageIndex,
      orderName: sortColumn,
      orderMode: sortMode,
    });
  };

  //Send selected
  const sendUserSelected = (user) => {
    if (user) {
      setUserSelected(user);
    } else {
      setUserSelected(null);
    }
    setShowSelectUser(false);
  };

  const sendAssetSelected = (asset) => {
    if (asset) setAssetSelected(asset);
    else {
      setAssetSelected(null);
    }
    setShowSelectAsset(false);
  };

  //Handle choose selected
  const handleChooseSelectUser = () => {
    setShowSelectUser(true);
    setShowSelectAsset(false);
  };

  const handleChooseSelectAsset = () => {
    setShowSelectAsset(true);
    setShowSelectUser(false);
  };

  //useEffect
  useEffect(() => {
    if(userSelected) {
      setShowSelectUserMainDisplay(userSelected?.fullName);
      setDataEditAssignment({
        ...dataEditAssignment,
        assignmentUserId:userSelected?.id,
      });
      setUserSelected(userSelected);
    }

    if(assetSelected) {
      setShowSelectAssetMainDisplay(assetSelected?.assetName);
      setDataEditAssignment({
        ...dataEditAssignment,
        assignmentAssetId:assetSelected?.id,
      });
      setAssetSelected(assetSelected);
    }
  } , [assetSelected, userSelected]);

  useEffect(() => {
    setDataEditAssignment({
      assignmentUserId:assignment.userIdAssigned,
      assignmentAssetId:assignment.assetId,
      assignmentNote:assignment.note,
      assignedDate:new Date(assignment.assignedDate),
    });
  },[]);

  //Call api
  const UpdateAssignment = async () => {
    try{
      var request = new AssignmentRequestModel();
      var formUpdateData = request.GetUpdateAssignmentFormData(
        dataEditAssignment.assignmentUserId,
        dataEditAssignment.assignmentAssetId,
        assignment.assetId,
        typeof dataEditAssignment.assignedDate !== "string" ?
          dataEditAssignment.assignedDate.toDateString() : dataEditAssignment.assignedDate,
        dataEditAssignment.assignmentNote
      );

      let result = await updateAssignmentRequest(assignment.id,formUpdateData);
      if(result.status === 200){
        var updatedAssignment= new AssignmentModel();
        updatedAssignment.id = assignment.id;
        updatedAssignment.userIdAssigned = dataEditAssignment.assignmentUserId;
        updatedAssignment.assetId = dataEditAssignment.assignmentAssetId;
        updatedAssignment.fullnameAssigned = userSelected?.fullName;
        updatedAssignment.assignedTo = userSelected?.id;
        updatedAssignment.assignedBy = assignment.assignedBy;
        updatedAssignment.assetCode = assetSelected?.assetCode;
        updatedAssignment.assetName = assetSelected?.assetName;
        updatedAssignment.specification = assetSelected?.specification;
        updatedAssignment.assignedDate= dataEditAssignment.assignedDate;
        updatedAssignment.modifiedDate = Date.now();
        updatedAssignment.assignmentState = assignment.assignmentState;
        updatedAssignment.note = dataEditAssignment.assignmentNote;
        updatedAssignment.categoryName = assetSelected?.category;

        history.push(ASSIGNMENT, {
          pageIndex: pageIndex,
          orderName: sortColumn,
          orderMode: sortMode,
          assignmentUpdated: updatedAssignment,
        });
        return;
      }
    }
    catch(err){
      if (err.response.status === 401 || err.response.status === 403) {
        Swal.fire({
          title: "Error",
          text: "You are not authorized to access this page",
          icon: "error",
          confirmButtonText: "OK",
        });
      }
      else if(err.response.data.errors){
        if(err.response.data.errors.UserId){
          Swal.fire({
            title: "Error",
            text: err.response.data.errors.UserId[0],
            icon: "error",
            confirmButtonText: "OK",
          });
        }
        else if(err.response.data.errors.AssetId){
          Swal.fire({
            title: "Error",
            text: err.response.data.errors.AssetId[0],
            icon: "error",
            confirmButtonText: "OK",
          });
        }
        else{
          Swal.fire({
            title: "Error",
            text: err.response.data.errors.AssignedDate[0],
            icon: "error",
            confirmButtonText: "OK",
          });
        }
      }
      else{
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
      <div className="container-fluid">
        <Formik initialValues={initialAssignmentForm} enableReinitialize>
          {
            <Form className="col-6">
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">User</label>
                </div>

                <div className="col-8" style={{ position: "relative" }}>
                  <Field
                    name="user"
                    type="text"
                    render={({
                      field /* { name, value, onChange, onBlur } */,
                    }) => (
                      <div style={{ position: "relative" }}>
                        <input
                          onClick={handleChooseSelectUser}
                          readOnly={true}
                          value={showSelectUserMainDisplay}
                          type="text"
                          className="form-control"
                          style={{ 
                            height: "30px", 
                            backgroundColor: "white",
                            paddingInlineEnd: "30px",
                            writingMode: "horizontal-tb",
                           }}
                        />
                        <MdOutlineArrowDropDown
                          onClick={handleChooseSelectUser}
                          style={{
                            position: "absolute",
                            right: "5px",
                            top: "5px",
                            fontSize: "20px",
                            color: "grey",
                          }}
                        />
                      </div>
                    )}
                  />
                  {showSelectUser ? (
                    <SelectUser
                      userSelected={userSelected}
                      // pref={refUser}
                      sendUserSelected={sendUserSelected}
                    />
                  ) : null}
                </div>
              </div>

              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Asset</label>
                </div>

                <div className="col-8" style={{ position: "relative" }}>
                  <Field
                    name="asset"
                    type="text"
                    render={({
                      field /* { name, value, onChange, onBlur } */,
                    }) => (
                      <div style={{ position: "relative" }}>
                        <input
                          onClick={handleChooseSelectAsset}
                          readOnly={true}
                          value={showSelectAssetMainDisplay}
                          type="text"
                          className="form-control"
                          style={{ 
                            height: "30px", 
                            backgroundColor: "white",
                            paddingInlineEnd: "30px",
                            writingMode: "horizontal-tb",
                          }}
                        />
                        <MdOutlineArrowDropDown
                          onClick={handleChooseSelectAsset}
                          style={{
                            position: "absolute",
                            right: "5px",
                            top: "5px",
                            fontSize: "20px",
                            color: "grey",
                          }}
                        />
                      </div>
                    )}
                  />
                  {showSelectAsset ? (
                    <SelectAsset
                      assetSelected={assetSelected}
                      // pref={refAsset}
                      sendAssetSelected={sendAssetSelected}
                    />
                  ) : null}
                </div>
              </div>

              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Assigned Date</label>
                </div>
                <div className="col-8">
                  <Field
                    name="assignedDate"
                    type="date"
                    onKeyDown={(e) => e.preventDefault()}
                    className={"w-100 form-control form-control-sm"}
                    value={
                      new Date(assignment.assignedDate).getTime() ===
                      new Date(dataEditAssignment.assignedDate).getTime()
                        ? dataEditAssignment.assignedDate.getFullYear() +
                          "-" +
                          ("0" + (dataEditAssignment.assignedDate.getMonth() + 1)).slice(-2) +
                          "-" +
                          ("0" + dataEditAssignment.assignedDate.getDate()).slice(-2)
                        : dataEditAssignment.assignedDate
                    }
                    onChange={(e) => {
                        setDataEditAssignment({
                        ...dataEditAssignment,
                        assignedDate: e.target.value
                      })
                    }
                  }
                  />
                </div>
              </div>

              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Note</label>
                </div>
                <div className="col-8">
                  <Field
                    value={dataEditAssignment.assignmentNote}
                    name="note"
                    className={"w-80 form-control form-control-sm"}
                    rows="4"
                    component="textarea"
                    onChange={(e) => setDataEditAssignment({
                      ...dataEditAssignment,
                      assignmentNote: e.target.value,
                    })
                  }
                  />
                </div>
              </div>

              <div className="row mt-4 pr-2">
                <div className="col-lg-12 col-md-12 text-end">
                  <button
                    className="btn btn-danger px-4 me-3"
                    onClick={UpdateAssignment}
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
          }
        </Formik>
      </div>
    </>
  );
};

export default EditAssignmentFormContainer;
