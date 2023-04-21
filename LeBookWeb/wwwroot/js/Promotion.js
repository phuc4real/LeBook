//API
const apiUrl = "/Dashboard/Book/BookList";
const apiAdd = "/Dashboard/Promotion/AddtoPromotion"
const apiGet = "/Dashboard/Promotion/PromotionJson"
const apiRemove = "/Dashboard/Promotion/RemovePromotion"

// Element
const btn = document.querySelector('#addBook');
const bookInput = document.querySelector('#book');
const tbody = document.querySelector('#tablebody');

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

async function getBook(value, bool) {
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
    if (bookInput.value) {
        getBook(bookInput.value, 'id').then(function (result) {
            AddtoPromotion(result)
            bookInput.value = '';
        });
    }
});

function AddtoPromotion(bookid) {
    $.ajax({
        type: 'POST', url: apiAdd, data: { id: bookid},
        success: function (data) {
            getPromotionItem()
        }
    });
}

function getPromotionItem() {
    $.ajax({
        type: 'GET', url: apiGet,
        success: function (data) {
            tbody.innerHTML = ''
            data.forEach(item => {
                const tr = document.createElement('tr');

                const tdbook = document.createElement('td');
                getBook(item.value, 'title').then(function (result) {
                    tdbook.innerHTML = result;
                });
                tr.appendChild(tdbook);

                const tdaction = document.createElement('td');
                tdaction.innerHTML = '<button type="button" class="btn btn-sm" onclick="remove(' + item.value + ')" id="remove' + item.value + '">X</button>';
                tr.appendChild(tdaction);

                tbody.appendChild(tr);
            })
        }
    });
}

getPromotionItem()


function remove(id) {
    $.ajax({
        type: 'POST', url: apiRemove, data: { id: id },
        success: function (data) {
            getPromotionItem()
        }
    });
}