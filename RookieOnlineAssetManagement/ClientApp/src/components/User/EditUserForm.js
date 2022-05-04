import React, { useEffect, useState } from "react";
import { Formik, Form, Field } from "formik";
import * as Yup from "yup";
import "../../App.css";
import { useHistory } from "react-router-dom";
import UserRequestModel from "../../models/UserRequestModel";
import { UpdateUserRequest } from "./services/request";
import { USER } from "../../Constants/pages";
import UserModel from "../../models/UserModel";
import Swal from "sweetalert2";
import "react-datepicker/dist/react-datepicker.css";

const validationSchema = Yup.object().shape({});
const EditUserFormContainer = ({ initialUserForm = {} }) => {
  const [birthDay, setBirthDay] = useState(new Date());
  const [gender, setGender] = useState(1);
  const [joinedDate, setJoinedDate] = useState(new Date());
  const [userType, setUserType] = useState("staff");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [errors, setErrors] = useState([]);
  const history = useHistory();
  const user = history.location.state?.existUser;
  const pageIndex = history.location.state?.pageIndex;
  const sortColumn = history.location.state?.sortColumn;
  const sortMode = history.location.state?.sortMode;

  useEffect(() => {
    if (user) {
      var name = user.fullName.split(" ");
      setFirstName(name[0]);
      name.shift();
      setLastName(name.join(" "));
      setBirthDay(new Date(user.birthDay));
      setGender(user.gender);
      var dateParts = user.joinedDate.split("/");
      let jDay = dateParts[0].padStart(2, "0");
      let jMonth = (parseInt(dateParts[1]) - 1).toString().padStart(2, "0");
      let jYear = dateParts[2].split(" ")[0];
      setJoinedDate(new Date(jYear, jMonth, jDay));
      setUserType(user.type);
    }
  }, [user]);

  const UpdateUser = async () => {
    try {
      var request = new UserRequestModel();
      var birthDateSend = birthDay;
      if (typeof birthDay !== "string") {
        birthDateSend = birthDay.toDateString();
      }
      var joinedDateSend = joinedDate;
      if (typeof joinedDate !== "string")
        joinedDateSend = joinedDate.toDateString();
      var formUpdateData = request.GetUpdateUserFormData(
        birthDateSend,
        gender,
        joinedDateSend,
        userType
      );

      let result = await UpdateUserRequest(user.id, formUpdateData);
      if (result.status === 200) {
        var updatedUser = new UserModel();
        updatedUser.id = user.id;
        updatedUser.birthDay = birthDay;
        updatedUser.fullName = user.fullName;
        updatedUser.gender = gender;
        updatedUser.isNew = user.isNew;
        updatedUser.joinedDate = joinedDate;
        updatedUser.location = user.location;
        updatedUser.staffCode = user.staffCode;
        updatedUser.type = userType;
        updatedUser.userName = user.userName;

        history.push(USER, {
          userUpdated: updatedUser,
          pageIndex: pageIndex,
          orderName: sortColumn,
          orderMode: sortMode,
        });
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

  const handleCancelEdit = () => {
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
                    value={firstName}
                    className="w-100 form-control form-control-sm"
                    disabled
                  />
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
                    value={lastName}
                    className="w-100 form-control form-control-sm"
                    disabled
                  />
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
                    value={
                      new Date(user.birthDay).getTime() ===
                      new Date(birthDay).getTime()
                        ? birthDay.getFullYear() +
                          "-" +
                          ("0" + (birthDay.getMonth() + 1)).slice(-2) +
                          "-" +
                          ("0" + birthDay.getDate()).slice(-2)
                        : birthDay
                    }
                    className={
                      errors.BirthDay
                        ? "w-100 form-control form-control-sm is-invalid"
                        : "w-100 form-control form-control-sm"
                    }
                    onChange={(e) => {
                      setBirthDay(e.target.value);
                      console.log(e.target.value);
                    }}
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
                    value={
                      typeof joinedDate !== "string"
                        ? joinedDate.getFullYear() +
                          "-" +
                          ("0" + (joinedDate.getMonth() + 1)).slice(-2) +
                          "-" +
                          ("0" + joinedDate.getDate()).slice(-2)
                        : joinedDate
                    }
                    onKeyDown={(e) => e.preventDefault()}
                    className={
                      errors.JoinedDate
                        ? "w-100 form-control form-control-sm is-invalid"
                        : "w-100 form-control form-control-sm"
                    }
                    onChange={(e) => setJoinedDate(e.target.value)}
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
                    name="type"
                    as="select"
                    value={userType}
                    className="w-100 form-select form-select-sm"
                    onChange={(e) => setUserType(e.target.value)}
                  >
                    <option value="Staff">Staff</option>
                    <option value="Admin">Admin</option>
                  </Field>
                </div>
              </div>

              <div className="row mt-4 pr-2">
                <div className="col-lg-12 col-md-12 text-end">
                  <button
                    className="btn btn-danger px-4 me-3"
                    onClick={UpdateUser}
                  >
                    Save
                  </button>

                  <button
                    onClick={() => {
                      history.push(USER);
                      return;
                    }}
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
export default EditUserFormContainer;
