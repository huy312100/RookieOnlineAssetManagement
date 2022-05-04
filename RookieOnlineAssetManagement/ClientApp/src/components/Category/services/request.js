import RequestService from "../../../services/requests";
import EndPoints from "../../../Constants/endpoints";

export function getCategoriesRequest() {
  return RequestService.axios.get(EndPoints.ListCategory);
}

export function createCategoryRequest(data){
  return RequestService.axios.post(EndPoints.CreateCategory, data);
}
