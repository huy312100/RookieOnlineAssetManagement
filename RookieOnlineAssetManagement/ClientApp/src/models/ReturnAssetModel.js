export default class HomeAssignmentModel {
    constructor(
      assetCode,
      assetName,
     requestedBy,
     assignedDate,
     acceptedBy,
     returnedDate,
     returnAssetState
    ) {
      this.assetCode = assetCode;
      this.assetName = assetName;
      this.requestedBy =requestedBy;
      this.assignedDate = assignedDate;
      this.acceptedBy = acceptedBy;
      this.returnedDate = returnedDate;
      this.returnAssetState = returnAssetState;
    }
  }