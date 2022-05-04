export default class AssetModel {
  constructor(
    id,
    assetCode,
    assetName,
    category,
    specification,
    installedDate,
    location,
    state
  ) {
    this.id = id;
    this.assetCode = assetCode;
    this.assetName = assetName;
    this.category = category;
    this.specification = specification;
    this.installedDate = installedDate;
    this.location = location;
    this.state = state;
  }
}
