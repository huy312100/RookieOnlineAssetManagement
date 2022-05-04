export default class AssetHistoryModel {
    constructor(
      assignedDate,
      returnedDate,
      assignedTo,
      assignedBy,
    ) {
      this.assignedDate = assignedDate;
      this.returnedDate = returnedDate;
      this.assignedTo = assignedTo;
      this.assignedBy = assignedBy;
    }
  }