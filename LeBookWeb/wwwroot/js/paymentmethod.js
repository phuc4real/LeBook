const Methods = document.querySelectorAll('.method');

Methods.forEach((method) => {
    const radio = method.querySelector('input')

    method.addEventListener('click', () => {
        radio.checked = true
    })
})


const apiUrl = "/Dashboard/Promotion/GetPromoCode";

async function addAvailablePromo() {
    const response = await fetch(apiUrl);
    const data = await response.json();
    const dataList = document.querySelector('#code');
    dataList.innerHTML = '';
    data.forEach(book => {
        const option = document.createElement('option');
        option.value = book.title;
        dataList.appendChild(option);
    });
}

addAvailablePromo()

const btn = document.querySelector('#applycode');
const codeInput = document.getElementById('PromotionCode');
const cartTotal = document.getElementById('carttotal');
const orderTotal = document.getElementById('ordertotal');


btn.addEventListener('click', () => {
    codeInput.readOnly = true;

})