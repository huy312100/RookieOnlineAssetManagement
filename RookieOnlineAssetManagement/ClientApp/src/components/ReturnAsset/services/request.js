import RequestService from "../../../services/requests";
import EndPoints from "../../../Constants/endpoints";
import qs from "qs";


export function getReturnAssetsRequest(query) {
  return RequestService.axios.get(EndPoints.ListReturnAsset, {
    params: query,
    paramsSerializer: (params) => qs.stringify(params),
  });
}
export function AcceptRequestReturnAssetRequest(returnAssetId) {
  return RequestService.axios.put(EndPoints.AcceptRequestReturnAsset(returnAssetId));
}
export function CancelRequestReturnAssetRequest(returnAssetId) {
  return RequestService.axios.put(EndPoints.CancelRequestReturnAsset(returnAssetId));
}
