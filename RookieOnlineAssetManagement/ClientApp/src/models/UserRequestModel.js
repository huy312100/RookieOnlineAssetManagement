export default class UserRequestModel {
  constructor(FirstName, LastName, BirthDay, Gender, JoinedDay, Type) {
    this.FirstName = FirstName;
    this.LastName = LastName;
    this.BirthDay = BirthDay;
    this.Gender = Gender;
    this.JoinedDay = JoinedDay;
    this.Type = Type;
  }

  GetCreateUserFormData(
    FirstName,
    LastName,
    BirthDay,
    Gender,
    JoinedDay,
    Type
  ) {
    var formData = new FormData();
    formData.append("FirstName", FirstName);
    formData.append("LastName", LastName);
    if (BirthDay !== new Date(1970, 1, 1).toJSON())
      formData.append("BirthDay", BirthDay);
    formData.append("Gender", Gender);
    if (JoinedDay !== new Date(1970, 1, 1).toJSON())
      formData.append("JoinedDate", JoinedDay);
    formData.append("Type", Type);

    return formData;
  }

  GetUpdateUserFormData(
    BirthDay,
    Gender,
    JoinedDay,
    Type
  ) {
    var formData = new FormData();
    formData.append("BirthDay", BirthDay);
    formData.append("Gender", Gender);
    formData.append("JoinedDate", JoinedDay);
    formData.append("Type", Type);

    return formData;
  }
}
