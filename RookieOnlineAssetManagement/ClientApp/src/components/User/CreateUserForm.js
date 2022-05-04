import React, { useState, useEffect } from "react";
import { Formik, Form, Field, ErrorMessage } from "formik";
import "../../App.css";
import Swal from "sweetalert2";
import { createUserRequest } from "./services/request";
import UserRequestModel from "../../models/UserRequestModel";
import { useHistory } from "react-router-dom";
import { USER } from "../../Constants/pages";
import UserModel from "../../models/UserModel";

const UserFormContainer = ({ initialUserForm = {} }) => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [birthDay, setBirthDay] = useState(new Date(1970, 1, 1).toJSON());
  const [gender, setGender] = useState(1);
  const [joinedDay, setJoinedDay] = useState(new Date(1970, 1, 1).toJSON());
  const [userType, setUserType] = useState("staff");
  const [errors, setErrors] = useState([]);
  const [canSubmit, setCanSubmit] = useState(false);
  const history = useHistory();
  const pageIndex = history.location.state?.pageIndex;
  const sortColumn = history.location.state?.sortColumn;
  const sortMode = history.location.state?.sortMode;

  useEffect(() => {
    if (
      firstName !== "" &&
      lastName !== "" &&
      birthDay !== new Date(1970, 1, 1).toJSON() &&
      joinedDay !== new Date(1970, 1, 1).toJSON()
    ) {
      setCanSubmit(true);
    }
  }, [firstName, lastName, birthDay, joinedDay]);

  const CreateUser = async () => {
    try {
      var request = new UserRequestModel();
      var formCreateData = request.GetCreateUserFormData(
        firstName,
        lastName,
        birthDay,
        gender,
        joinedDay,
        userType
      );

      let result = await createUserRequest(formCreateData);
      if (result.status === 201) {
        var userResponse = result.data.user ? result.data.user : null;
        if (userResponse) {
          var createdUser = new UserModel();
          createdUser.id = userResponse.id;
          createdUser.birthDay = userResponse.birthDay;
          createdUser.fullName = userResponse.fullName;
          createdUser.gender = userResponse.gender;
          createdUser.isNew = userResponse.isNew;
          createdUser.joinedDate = userResponse.joinedDate;
          createdUser.location = userResponse.location;
          createdUser.staffCode = userResponse.staffCode;
          createdUser.type = userResponse.type;
          createdUser.userName = userResponse.userName;
          history.push(USER, {
            userCreated: createdUser,
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

  const handleCancelCreate = () => {
    history.push(USER, {
      pageIndex: pageIndex,
      orderName: sortColumn,
      orderMode: sortMode,
    });
  };

  return (
    <>
      <div className="container-fluid">
        <Formik initialValues={initialUserForm} enableReinitialize>
          {
            <Form className="col-6">
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">First Name</label>
                </div>
                <div className="col-8">
                  <Field
                    name="firstName"
                    type="text"
                    className={
                      errors.FirstName
                        ? "w-100 form-control form-control-sm is-invalid"
                        : "w-100 form-control form-control-sm"
                    }
                    maxLength={100}
                    onChange={(e) => setFirstName(e.target.value)}
                  />
                  {errors.FirstName
                    ? errors.FirstName.map((element) => (
                        <div className="text-danger">{element}</div>
                      ))
                    : null}
                  {/* <ErrorMessage name="firstName" /> */}
                </div>
              </div>
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Last Name</label>
                </div>
                <div className="col-8">
                  <Field
                    name="lastName"
                    type="text"
                    className={
                      errors.LastName
                        ? "w-100 form-control form-control-sm is-invalid"
                        : "w-100 form-control form-control-sm"
                    }
                    maxLength={100}
                    onChange={(e) => setLastName(e.target.value)}
                  />
                  {errors.LastName
                    ? errors.LastName.map((element) => (
                        <div className="text-danger">{element}</div>
                      ))
                    : null}
                </div>
              </div>
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Date of Birth</label>
                </div>
                <div className="col-8">
                  <Field
                    name="birthDay"
                    type="date"
                    onKeyDown={(e) => e.preventDefault()}
                    className={
                      errors.BirthDay
                        ? "w-100 form-control form-control-sm is-invalid"
                        : "w-100 form-control form-control-sm"
                    }
                    onChange={(e) => setBirthDay(e.target.value)}
                  />
                  {errors.BirthDay
                    ? errors.BirthDay.map((element) => (
                        <div className="text-danger">{element}</div>
                      ))
                    : null}
                </div>
              </div>
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Gender</label>
                </div>
                <div className="col-8">
                  <div role="group" aria-labelledby="gender-radio-group">
                    <label className="mr-3">
                      <Field
                        className="form-check-input custom-control-input"
                        type="radio"
                        name="gender"
                        value="1"
                        checked={gender === 1}
                        onClick={(e) => setGender(1)}
                      />
                      &ensp;Female&ensp;&ensp;&ensp;&ensp;
                    </label>
                    <label>
                      <Field
                        className="form-check-input custom-control-input"
                        type="radio"
                        name="gender"
                        value="2"
                        checked={gender === 2}
                        onChange={(e) => setGender(2)}
                      />
                      &ensp;Male
                    </label>
                  </div>
                </div>
              </div>
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Joined Date</label>
                </div>
                <div className="col-8">
                  <Field
                    name="joinedDate"
                    type="date"
                    onKeyDown={(e) => e.preventDefault()}
                    className={
                      errors.JoinedDate
                        ? "w-100 form-control form-control-sm is-invalid"
                        : "w-100 form-control form-control-sm"
                    }
                    onChange={(e) => setJoinedDay(e.target.value)}
                  />
                  {errors.JoinedDate
                    ? errors.JoinedDate.map((element) => (
                        <div className="text-danger">{element}</div>
                      ))
                    : null}
                </div>
              </div>
              <div className="row mt-4">
                <div className="col-4">
                  <label className="form-label">Type</label>
                </div>
                <div className="col-8">
                  <Field
                    value={userType}
                    name="type"
                    as="select"
                    className="w-100 form-select form-select-sm"
                    onChange={(e) => setUserType(e.target.value)}
                  >
                    <option value="staff">Staff</option>
                    <option value="admin">Admin</option>
                  </Field>
                </div>
              </div>

              <div className="row mt-4 pr-2">
                <div className="col-lg-12 col-md-12 text-end">
                  {canSubmit ? (
                    <button
                      className="btn btn-danger px-4 me-3"
                      onClick={CreateUser}
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
export default UserFormContainer;
