const DEBUG = 1;

/* Amount */

const allItem = document.querySelectorAll('.item-product-cart')
const numberItem = document.querySelector('.number-items-checkbox')
numberItem.innerText = `${allItem.length}`

/* Checkbox */

const container = document.getElementById('cart-content');
const checkboxes = container.querySelectorAll('.checkbox-add-cart');
const selectBtn = document.querySelector('.checkbox-all-product input');
const numCheckboxes = checkboxes.length;

let allChecked = false;

$("document").ready(isAllChecked());

container.addEventListener('change', (event) => {
    if (event.target.matches('.checkbox-add-cart')) {
        const id = event.target.getAttribute('data-cartid');
        cartToBuy(id);

        isAllChecked();
    }
});

function isAllChecked() {
    let numChecked = 0;
    checkboxes.forEach((checkbox) => {
        if (checkbox.checked) {
            numChecked++;
        }
    });

    if (numChecked === numCheckboxes) {
        allChecked = true;
        selectBtn.checked = true;
    } else {
        allChecked = false;
        selectBtn.checked = false;
    }
}

selectBtn.addEventListener('change', () => {
    checkboxes.forEach((checkbox) => {
        if (selectBtn.checked)
        {
            if (!checkbox.checked) checkbox.click()
        }    
        else checkbox.click()
    });
    allChecked = selectBtn.checked;
});

function cartToBuy(cartId) {
    $.ajax({
        type: 'POST',
        url: 'Cart/ToBuy',
        data: { cartId: cartId },
        success: function (data) {
            if (DEBUG) console.log("Đã thay đổi #" + cartId);
            setTimeout(updateTotal(), 1000);
        }
    });
    
}

const money = new Intl.NumberFormat('de-DE',
    { style: 'currency', currency: 'VND' });

function updateTotal() {
    $.ajax({
        url: 'Cart/TotalPrice',
        success: function (data) {
            $('#total-price').text(money.format(data));
            $('.final-price').text(money.format(data));
            if (DEBUG) console.log("Tổng tiền: " + data);
        }
    });
}

function applyCode() {
    $.ajax({
        url: 'Cart/SaleOff',
        success: function (data) {
            $('#saleoff').text(money.format(data.sale));
            $('.final-price').text(money.format(data.cartTotal));
        }
    });
}
document.addEventListener("DOMContentLoaded", applyCode);