//API
const apiUrl = "/Dashboard/Book/BookList";
const apiAdd = "/Dashboard/PurchaseOrder/AddToPurchase"
const apiGet = "/Dashboard/PurchaseOrder/PurchaseJson"
const apiRemove = "/Dashboard/PurchaseOrder/RemovePurchase"

// Element
const btn = document.querySelector('#addBook');
const bookInput = document.querySelector('#book');
const quantityInput = document.querySelector('#quantity');
const priceInput = document.querySelector('#price');
const tbody = document.querySelector('#tablebody');
const total = document.querySelector('#PurchaseOrder_PurchaseTotal');

async function addBookOptions() {
    const response = await fetch(apiUrl);
    const data = await response.json();
    const dataList = document.querySelector('#bookdata');
    dataList.innerHTML = '';
    data.forEach(book => {
        const option = document.createElement('option');
        option.value = book.title;
        dataList.appendChild(option);
    });
}

addBookOptions()

async function getBook(value,bool) {
    const response = await fetch(apiUrl);
    const data = await response.json();
    if (bool === 'id') {
        const book = data.find(item => item.title === value);
        return book.id;
    }
    else {
        const book = data.find(item => item.id === value);
        return book.title;
    }
}

btn.addEventListener('click', function () {
    getBook(bookInput.value,'id').then(function (result) {
        AddToPurchase(result, quantityInput.value, priceInput.value)
        bookInput.value = '';
        quantityInput.value = '';
        priceInput.value = '';
    });
});

function AddToPurchase(bookid,quantity,price) {
    $.ajax({
        type: 'POST', url: apiAdd, data: { id: bookid, quantity: quantity, price: price },
        success: function (data) {
            getPurchaseItem()
        }
    });
}

function getPurchaseItem() {
    $.ajax({
        type: 'GET', url: apiGet,
        success: function (data) {
            tbody.innerHTML = ''
            let res = 0;
            data.forEach(item => {
                const tr = document.createElement('tr');

                const tdbook = document.createElement('td');
                getBook(item.bookId, 'title').then(function (result) {
                    tdbook.innerHTML = result;
                });
                tr.appendChild(tdbook);

                const tdquantity = document.createElement('td');
                tdquantity.innerHTML = item.quantity;
                tr.appendChild(tdquantity);

                const tdprice = document.createElement('td');
                tdprice.innerHTML = item.price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                tr.appendChild(tdprice);

                const tdtotal = document.createElement('td');
                tdtotal.innerHTML = (item.price * item.quantity).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                tr.appendChild(tdtotal);
                res += (item.price * item.quantity);

                const tdaction = document.createElement('td');
                tdaction.innerHTML = '<button type="button" class="btn btn-sm" onclick="remove(' + item.bookId + ')" id="remove' + item.bookId + '">X</button>';
                tr.appendChild(tdaction);

                tbody.appendChild(tr);
            })
            total.value = res;
        }
    });
}

getPurchaseItem()


function remove(id) {
    $.ajax({
        type: 'POST', url: apiRemove, data: { id: id},
        success: function (data) {
            getPurchaseItem()
        }
    });
}