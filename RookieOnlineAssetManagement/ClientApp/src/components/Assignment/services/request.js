import RequestService from "../../../services/requests";
import EndPoints from "../../../Constants/endpoints";
import qs from "qs";

export function createAssignmentRequest(data) {
  return RequestService.axios.post(EndPoints.CreateAssignment, data);
}

export function getAssignmentsRequest(query) {
  return RequestService.axios.get(EndPoints.ListAssignment, {
    params: query,
    paramsSerializer: (params) => qs.stringify(params),
  });
}

export function updateAssignmentRequest(assignmentId, data) {
  return RequestService.axios.put(EndPoints.UpdateAssignment(assignmentId), data);
}

export function returnAssetRequest(assignmentId) {
  return RequestService.axios.post(EndPoints.AdminReturnRequestAsset(assignmentId));
}

export function DeleteAssignmentRequest(assignmentId) {
  return RequestService.axios.delete(EndPoints.DeleteAssignment(assignmentId));
}
