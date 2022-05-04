export default class AssetRequestModel {
  constructor(AssetName, CategoryId, Specification, InstalledDate, AssetState) {
    this.AssetName = AssetName;
    this.CategoryId = CategoryId;
    this.Specification = Specification;
    this.InstalledDate = InstalledDate;
    this.AssetState = AssetState;
  }

  GetCreateAssetFormData(
    AssetName,
    CategoryId,
    Specification,
    InstalledDate,
    AssetState
  ) {
    var formData = new FormData();
    formData.append("AssetName", AssetName);
    formData.append("CategoryId", CategoryId);
    formData.append("Specification", Specification);
    formData.append("InstalledDate", InstalledDate);
    formData.append("AssetState", AssetState);
    return formData;
  }

  GetUpdateAssetFormData(AssetName, Specification, InstalledDate, AssetState) {
    var formData = new FormData();
    formData.append("AssetName", AssetName);
    formData.append("Specification", Specification);
    formData.append("InstalledDate", InstalledDate);
    formData.append("AssetState", AssetState);
    return formData;
  }
}
