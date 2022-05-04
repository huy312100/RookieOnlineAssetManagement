export default class AssignmentModel {
  constructor(
    id,
    userIdAssigned,
    assetId,
    fullnameAssigned,
    assignedTo,
    assignedBy,
    assetCode,
    assetName,
    specification,
    assignedDate,
    modifiedDate,
    assignmentState,
    note,
    returnAsset,
    categoryName,
  ) {
    this.id = id;
    this.userIdAssigned=userIdAssigned;
    this.assetId = assetId;
    this.fullnameAssigned = fullnameAssigned;
    this.assignedTo = assignedTo;
    this.assignedBy = assignedBy;
    this.assetCode = assetCode;
    this.assetName = assetName;
    this.specification = specification;
    this.assignedDate = assignedDate;
    this.modifiedDate = modifiedDate;
    this.assignmentState = assignmentState;
    this.note = note;
    this.returnAsset=returnAsset;
    this.categoryName = categoryName;
  }
}
