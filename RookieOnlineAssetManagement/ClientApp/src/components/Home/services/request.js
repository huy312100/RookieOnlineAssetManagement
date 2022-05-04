import { AxiosResponse } from "axios";
import qs from "qs";

import RequestService from "../../../services/requests";
import EndPoints from "../../../Constants/endpoints";


export function getHomesRequest(query) {
  return RequestService.axios.get(EndPoints.ListHome, {
    params: query,
    paramsSerializer: (params) => qs.stringify(params),
  });
}
export function ChangeAssignmentStateRequest(assignmentId ) {
  return RequestService.axios.put(EndPoints.ChangeAssignmentState(assignmentId));
}
export function DeleteAssignmentRequest(assignmentId) {
  return RequestService.axios.delete(EndPoints.DeleteAssignment(assignmentId));
}
export function ReturnAssetRequest(assignmentId ) {
  return RequestService.axios.post(EndPoints.RequestReturnAsset(assignmentId));
}