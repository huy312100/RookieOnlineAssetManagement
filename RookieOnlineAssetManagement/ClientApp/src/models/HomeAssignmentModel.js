export default class HomeAssignmentModel {
    constructor(
      assetCode,
      assetName,
      category,
      assignedDate,
      state,
      specification,
      note,
      assignedBy,
      assignedTo,
      returnAsset,
      id,


    ) {
      this.assetCode = assetCode;
      this.assetName = assetName;
      this.category = category;
      this.assignedDate = assignedDate;
      this.state = state;
      this.assignedBy= assignedBy;
      this.assignedTo= assignedTo;
      this.note= note;
      this.specification=specification;
      this.returnAsset = returnAsset;
      this.id = id;
    }
  }