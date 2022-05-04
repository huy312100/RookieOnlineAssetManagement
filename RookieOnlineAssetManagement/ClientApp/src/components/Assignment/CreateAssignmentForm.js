import { Field, Form, Formik } from "formik";
import { useState, useEffect, useRef } from "react";
import { useHistory } from "react-router-dom";
import { ASSIGNMENT } from "../../Constants/pages";
import { BiSearch } from "react-icons/bi";
import SelectAsset from "./Create/SelectTable/SelectAsset";
import SelectUser from "./Create/SelectTable/SelectUser";
import UserModel from "../../models/UserModel";
import AssignmentRequestModel from "../../models/AssignmentRequestModel";
import { createAssignmentRequest } from "./services/request";
import Swal from "sweetalert2";
import AssignmentModel from "../../models/AssignmentModel";

const CreateAssignmentFormContainer = ({ initialUserForm = {} }) => {
  const [canSubmit, setCanSubmit] = useState(false);
  const [showSelectUser, setShowSelectUser] = useState(false);
  const [showSelectAsset, setShowSelectAsset] = useState(false);
  const [userSelected, setUserSelected] = useState(null);
  const [assetSelected, setAssetSelected] = useState(null);
  const [assignmentNote, setAssignmentNote] = useState("");
  const [assignedDate, setAssignedDate] = useState(new Date());
  const [errors, setErrors] = useState([]);
  const history = useHistory();
  const pageIndex = history.location.state?.pageIndex;
  const sortColumn = history.location.state?.sortColumn;
  const sortMode = history.location.state?.sortMode;

  var today = new Date();
  var dd = today.getDate();
  var mm = today.getMonth() + 1; //January is 0!
  var yyyy = today.getFullYear();

  if (dd < 10) {
    dd = "0" + dd;
  }

  if (mm < 10) {
    mm = "0" + mm;
  }

  today = yyyy + "-" + mm + "-" + dd;

  useEffect(() => {
    if (userSelected && assetSelected) {
      setCanSubmit(true);
    }
  }, [assetSelected, userSelected]);

  const handleCancelCreate = () => {
    history.push(ASSIGNMENT, {
      pageIndex: pageIndex,
      orderName: sortColumn,
      orderMode: sortMode,
    });
  };

  const handleChooseSelectUser = () => {
    setShowSelectUser(true);
    setShowSelectAsset(false);
  };

  const handleChooseSelectAsset = () => {
    setShowSelectAsset(true);
    setShowSelectUser(false);
  };

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

  const CreateAssignment = async () => {
    try {
      var request = new AssignmentRequestModel();

      var assignedDateSend = assignedDate;
      if (typeof assignedDate !== "string") {
        assignedDateSend = assignedDate.toDateString();
      }

      var formCreateData = request.GetCreateAssignmentFormData(
        userSelected.id,
        assetSelected.id,
        assignedDateSend,
        assignmentNote
      );

      let result = await createAssignmentRequest(formCreateData);
      if (result.status === 201) {
        var assignmentResponse = result.data.assignment
          ? result.data.assignment
          : null;
        if (assignmentResponse) {
          var createdAssignment = new AssignmentModel();
          createdAssignment.id = assignmentResponse.id
            ? assignmentResponse.id
            : "";
          createdAssignment.userIdAssigned = assignmentResponse.userIdAssigned
            ? assignmentResponse.userIdAssigned
            : "";
          createdAssignment.assetId = assignmentResponse.assetId
            ? assignmentResponse.assetId
            : "";
          createdAssignment.fullnameAssigned = assignmentResponse.fullnameAssigned
            ? assignmentResponse.fullnameAssigned
            : "";
          createdAssignment.assignedBy = assignmentResponse.assignedBy
            ? assignmentResponse.assignedBy
                : "";
            createdAssignment.assignedTo = assignmentResponse.assignedTo
                ? assignmentResponse.assignedTo
                : "";
          createdAssignment.assetCode = assignmentResponse.assetCode
            ? assignmentResponse.assetCode
            : "";
          createdAssignment.assetName = assignmentResponse.assetName
            ? assignmentResponse.assetName
            : "";
          createdAssignment.specification = assignmentResponse.specification
            ? assignmentResponse.specification
            : "";
          createdAssignment.assignedDate = assignmentResponse.assignedDate
            ? assignmentResponse.assignedDate
            : "";
          createdAssignment.modifiedDate = assignmentResponse.modifiedDate
            ? assignmentResponse.modifiedDate
            : "";
          createdAssignment.assignmentState = assignmentResponse.assignmentState
            ? assignmentResponse.assignmentState
            : "";
          createdAssignment.note = assignmentResponse.note
            ? assignmentResponse.note
            : "";
          createdAssignment.categoryName = assignmentResponse.categoryName
            ? assignmentResponse.categoryName
            : "";
          history.push(ASSIGNMENT, {
            createdAssignment: createdAssignment,
            pageIndex: pageIndex,
            orderName: sortColumn,
            orderMode: sortMode,
          });
        }

        return;
      }
      setErrors(result.errors);
    } catch (e) {
      if (e.response.status === 401 || e.response.status === 403) {
        await Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Permission Denied",
        });
      } else if (e.response.data.errors) {
        setErrors(e.response.data.errors);
      } else {
        await Swal.fire({
          icon: "error",
          title: "Oops...",
          text: e,
        });
      }
    }
  };

  return (
    <>
      <div className="container-fluid">
        <Formik initialValues={initialUserForm} enableReinitialize>
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
                          value={userSelected?.fullName || ""}
                          type="text"
                          className="form-control"
                          style={{
                            height: "30px",
                            backgroundColor: "white",
                            paddingInlineEnd: "30px",
                            writingMode: "horizontal-tb",
                          }}
                        />
                        <BiSearch
                          onClick={handleChooseSelectUser}
                          style={{
                            position: "absolute",
                            right: "5px",
                            top: "5px",
                            fontSize: "20px",
                          }}
                        />
                      </div>
                    )}
                  />
                  {showSelectUser ? (
                    <SelectUser
                      userSelected={userSelected}
                      //pref={refUser}
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
                          maxLength={25}
                          onClick={handleChooseSelectAsset}
                          readOnly={true}
                          type="text"
                          className="form-control"
                          value={assetSelected?.assetName || ""}
                          style={{
                            height: "30px",
                            backgroundColor: "white",
                            paddingInlineEnd: "30px",
                            writingMode: "horizontal-tb",
                          }}
                        />
                        <BiSearch
                          onClick={handleChooseSelectAsset}
                          style={{
                            position: "absolute",
                            right: "5px",
                            top: "5px",
                            fontSize: "20px",
                          }}
                        />
                      </div>
                    )}
                  />
                  {showSelectAsset ? (
                    <SelectAsset
                      assetSelected={assetSelected}
                      //pref={refAsset}
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
                    required
                    name="assignedDate"
                    type="date"
                    onKeyDown={(e) => e.preventDefault()}
                    className={
                      errors.AssignedDate
                        ? "w-100 form-control form-control-sm is-invalid"
                        : "w-100 form-control form-control-sm"
                    }
                    value={
                      typeof assignedDate !== "string"
                        ? assignedDate.getFullYear() +
                          "-" +
                          ("0" + (assignedDate.getMonth() + 1)).slice(-2) +
                          "-" +
                          ("0" + assignedDate.getDate()).slice(-2)
                        : assignedDate
                    }
                    min={today}
                    onChange={(e) => setAssignedDate(e.target.value)}
                  />
                  {errors.AssignedDate
                    ? errors.AssignedDate.map((element) => (
                        <div className="text-danger">{element}</div>
                      ))
                    : null}
                </div>
              </div>
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Note</label>
                </div>
                <div className="col-8">
                  <Field
                    name="note"
                    className={"w-80 form-control form-control-sm"}
                    rows="4"
                    component="textarea"
                    onChange={(e) => setAssignmentNote(e.target.value)}
                  />
                </div>
              </div>
              <div className="row mt-4 pr-2">
                <div className="col-lg-12 col-md-12 text-end">
                  {canSubmit ? (
                    <button
                      className="btn btn-danger px-4 me-3"
                      onClick={CreateAssignment}
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
          }
        </Formik>
        <div className="col-6"></div>
      </div>
    </>
  );
};

export default CreateAssignmentFormContainer;
