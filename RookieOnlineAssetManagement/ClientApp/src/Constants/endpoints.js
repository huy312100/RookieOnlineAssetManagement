const Endpoints = {
  // Call API here
  // User api
  CreateUser: "/api/users",
  ListUser: "/api/users/paging",
  userId: (id) => `/api/users/${id}`,
  CheckAssigment: (username) =>
    `/api/users/FindAllAssignmentByUsername/${username}`,
  DeleteUser: (username) => `/api/users/${username}`,

  // Asset api
  ListAsset: "/api/assets/paging",
  ListHistory: (id) => `/api/assets/${id}/history`,
  CreateAsset: "/api/assets",
  UpdateAsset: (id) => `/api/assets/${id}`,
  CheckAssetHistory: (assetId) => `/api/assets/CheckAssetHistory/${assetId}`,
  DeleteAsset: (assetId) => `/api/assets/${assetId}`,

  // Category api
  ListCategory: "/api/categories",
  CreateCategory: "/api/categories",

  //Assignment api
  ListHome: "/api/assignments/HomePaging",
  ListAssignment: "/api/assignments/paging",
  ChangeAssignmentState: (id) => `api/assignments/ChangeAssignmentState/${id}`,
  DeleteAssignment: (id) => `/api/assignments/${id}`,
  CreateAssignment: "/api/assignments",
  UpdateAssignment: (id) => `/api/assignments/${id}`,

  //Return Asset api  
  RequestReturnAsset: (id) => `/api/returnassets/CreateRequestReturnAssetUser/${id}`,
  ListReturnAsset:"/api/returnassets/paging",
  AdminReturnRequestAsset: (id) => `/api/returnassets/admin/${id}`,
  AcceptRequestReturnAsset: (id) => `/api/returnassets/AcceptRequestReturnAsset/${id}`,
  CancelRequestReturnAsset: (id) => `/api/returnassets/CancelRequestReturnAsset/${id}`,
  // Report api
  ListReport: "/api/reports/paging",
};

export default Endpoints;
