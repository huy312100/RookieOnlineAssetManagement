//React url here

export const HOME = "/";
//User pages
export const USER = "/user";
export const CREATE_USER = "/user/create";
export const EDIT_USER = "/user/edit/:id";
export const EDIT_USER_ID = (id) => `/user/edit/${id}`;

//Asset pages
export const ASSET = "/asset";
export const CREATE_ASSET = "/asset/create";
export const EDIT_ASSET = "/asset/edit/:id";
export const EDIT_ASSET_ID = (id) => `/asset/edit/${id}`;
export const ASSET_HISTORY = (id) => `/asset/history/${id}`;

//Assignment pages
export const ASSIGNMENT = "/assignment";
export const CREATE_ASSIGNMENT = "/assignment/create";
export const EDIT_ASSIGNMENT = "/assignment/edit/:id";
export const EDIT_ASSIGNMENT_ID = (id) => `/assignment/edit/${id}`;

//Category pages
export const CATEGORY = "/category";
export const CREATE_CATEGORY = "/category/create";

//Request Return Asset pages
export const RETURNASSET = "/returnasset";

// Report pages
export const REPORT = "/report";

//Common pages
export const UNAUTHORIZE = "/unauthorize";
export const NOTFOUND = "/notfound";
