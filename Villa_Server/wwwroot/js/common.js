﻿window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Operation Successful", {timeOut: 10000});
    }
    if (type === "error") {
        toastr.error(message, "Operation Failed", { timeOut: 10000});
    }
}

window.ShowSwal = (type, message) => {
    if (type === "success") {
        Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: message,
            showConfirmButton: false,
            timer: 1500
        });
    }
    if (type === "error") {
        Swal.fire({
            position: 'top-end',
            icon: 'error',
            title: message,
            showConfirmButton: false,
            timer: 1500
        });
    }
}

function ShowDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('show');
}

function HideDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('hide');
}