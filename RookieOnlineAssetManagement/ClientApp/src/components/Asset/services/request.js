import qs from "qs";

import RequestService from "../../../services/requests";
import EndPoints from "../../../Constants/endpoints";

export function getAssetsRequest(query) {
  return RequestService.axios.get(EndPoints.ListAsset, {
    params: query,
    paramsSerializer: (params) => qs.stringify(params),
  });
}

export function getHistoryRequest(id, query) {
  return RequestService.axios.get(EndPoints.ListHistory(id), {
    params: query,
    paramsSerializer: (params) => qs.stringify(params),
  });
}

export function createAssetRequest(data) {
  return RequestService.axios.post(EndPoints.CreateAsset, data);
}

export function updateAssetRequest(assetId, data) {
  return RequestService.axios.put(EndPoints.UpdateAsset(assetId), data);
}

export function DeleteAssetRequest(assetId) {
  return RequestService.axios.delete(EndPoints.DeleteAsset(assetId));
}

export function CheckAssetHistory(assetId) {
  return RequestService.axios.get(EndPoints.CheckAssetHistory(assetId));
}
