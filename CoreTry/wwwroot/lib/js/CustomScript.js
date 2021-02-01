function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confimDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confimDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confimDeleteSpan).hide();
    }
}