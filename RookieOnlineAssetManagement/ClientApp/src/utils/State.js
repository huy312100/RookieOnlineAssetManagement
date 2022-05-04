export const convertAssetStateToString = (number) => {
  switch (number) {
    case 1:
      return "Available";
    case 2:
      return "Not Available";
    case 3:
      return "Assigned";
    case 4:
      return "Waiting for recycling";
    case 5:
      return "Recycled";
    default:
      return "";
  }
};

export const convertAssignmentStateToString = (number) => {
  switch (number) {
    case 1:
      return "Waiting for acceptance";
    case 2:
      return "Accepted";
    default:
      return "";
  }
};

export const convertReturnAssetStateToString = (number) =>{
  switch (number){
    case 1:
      return "Waiting for returning"
    case 2:
      return "Completed"
    default:
      return "";
  }
}