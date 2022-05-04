export default class UserModel {
  constructor(
    id,
    fullName,
    birthDay,
    gender,
    isNew,
    joinedDate,
    location,
    staffCode,
    type,
    userName
  ) {
    this.id = id;
    this.fullName = fullName;
    this.birthDay = birthDay;
    this.gender = gender;
    this.isNew = isNew;
    this.joinedDate = joinedDate;
    this.location = location;
    this.staffCode = staffCode;
    this.type = type;
    this.userName = userName;
  }
}
