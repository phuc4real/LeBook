window.addEventListener('DOMContentLoaded', event => {
    const datatablesSimple = document.getElementById('datatablesSimple');
    if (datatablesSimple) {
        new simpleDatatables.DataTable(datatablesSimple, {
            labels: {
                placeholder: "Tìm kiếm...",
                searchTitle: "Tìm trong bảng",
                perPage: "mẫu tin trên trang",
                noRows: "Không có dữ liệu",
                info: "Hiển thị mẫu tin {start} tới {end} trong {rows} mẫu tin",
                noResults: "Không tìm thấy dữ liệu!",
            }
        });
    }
});

function submitDelete(e, id, text) {
    e.preventDefault();
    Swal.fire({
        title: 'Xác nhận',
        text: "Xóa " + text + " này?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xác nhận!',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Đã xóa!',
                text + ' đã được xoá.',
                'success'
            ).then((result) => document.getElementById("Delete+" + id).submit())
        }
    })
}

function submitRestore(e, id, text) {
    e.preventDefault();
    Swal.fire({
        title: 'Xác nhận',
        text: "Phục hồi " + text + " này?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xác nhận!',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Đã phục hồi!',
                text + ' đã được phục hồi.',
                'success'
            ).then((result) => document.getElementById("Restore+" + id).submit())
        }
    })
}