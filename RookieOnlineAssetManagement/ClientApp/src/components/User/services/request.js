import { AxiosResponse } from "axios";
import qs from "qs";

import RequestService from "../../../services/requests";
import EndPoints from "../../../Constants/endpoints";

export function createUserRequest(data) {
  return RequestService.axios.post(EndPoints.CreateUser, data);
}

export function getUsersRequest(query) {
  return RequestService.axios.get(EndPoints.ListUser, {
    params: query,
    paramsSerializer: (params) => qs.stringify(params),
  });
}

export function UpdateUserRequest(userId, formData) {
  return RequestService.axios.put(EndPoints.userId(userId ?? -1), formData);
}

export function DisableUserRequest(userName) {
  return RequestService.axios.delete(EndPoints.DeleteUser(userName));
}
export function CheckDisableUserRequest(userName) {
  return RequestService.axios.get(EndPoints.CheckAssigment(userName));
}
