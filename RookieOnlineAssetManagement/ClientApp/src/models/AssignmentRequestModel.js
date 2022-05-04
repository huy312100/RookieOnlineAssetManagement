export default class AssignmentRequestModel {
    constructor(UserId, AssetId, AssignedDate, Note) {
        this.UserId = UserId;
        this.AssetId = AssetId;
        this.AssignedDate = AssignedDate;
        this.Note = Note;
    }

    GetCreateAssignmentFormData(UserId, AssetId, AssignedDate, Note) {
        var formData = new FormData();
        formData.append("UserId", UserId);
        formData.append("AssetId", AssetId);
        formData.append("AssignedDate", AssignedDate);
        if (Note) formData.append("Note", Note);

        return formData;
    }

    GetUpdateAssignmentFormData(UserId, NewAssetId,OldAssetId, AssignedDate, Note) {
        var formData = new FormData();
        formData.append("UserId", UserId);
        formData.append("NewAssetId", NewAssetId);
        formData.append("OldAssetId", OldAssetId);
        formData.append("AssignedDate", AssignedDate);
        if (Note) formData.append("Note", Note);

        return formData;
    }
}