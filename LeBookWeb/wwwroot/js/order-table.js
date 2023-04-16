const apiUrl = "/Dashboard/Order/OrderList";
/*const apiUrl = "https://fiduswriter.github.io/simple-datatables/demos/18-fetch-api/demo.json";*/

fetch(apiUrl).then(
    response => response.json()
).then(
    data => {
        if (!data.length) {
            return
        }
        console.log(Object.keys(data[0]))
        new simpleDatatables.DataTable(".orderTables", {
            data: {
                headings: [ "Mã hoá đơn", "Người đặt hàng", "Ngày lập", "Tổng thành tiền", "Tình trạng đơn hàng", "Phương thức thanh toán", "Tình trạng thanh toán", " " ],
                data: data.map(item => Object.values(item))
            },
            labels: {
                placeholder: "Tìm kiếm...",
                searchTitle: "Tìm trong bảng",
                perPage: "mẫu tin trên trang",
                noRows: "Không có dữ liệu",
                info: "Hiển thị mẫu tin {start} tới {end} trong {rows} mẫu tin",
                noResults: "Không tìm thấy dữ liệu!",
            }
        })
    }
)