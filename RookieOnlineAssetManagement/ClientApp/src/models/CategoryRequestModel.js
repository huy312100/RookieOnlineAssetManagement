export default class CategoryRequestModel {
  constructor(CategoryName, Prefix) {
    this.CategoryName = CategoryName;
    this.Prefix = Prefix;
  }

  GetCreateCategoryFormData(CategoryName, Prefix) {
    var formData = new FormData();
    formData.append("CategoryName", CategoryName);
    formData.append("Prefix", Prefix);
    return formData;
  }
}
