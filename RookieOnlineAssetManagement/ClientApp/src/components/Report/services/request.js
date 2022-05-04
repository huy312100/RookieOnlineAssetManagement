import { AxiosResponse } from "axios";
import qs from "qs";

import RequestService from "../../../services/requests";
import EndPoints from "../../../Constants/endpoints";

export function getReportRequest(query) {
  return RequestService.axios.get(EndPoints.ListReport, {
    params: query,
    paramsSerializer: (params) => qs.stringify(params),
  });
}
