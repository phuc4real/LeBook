const Methods = document.querySelectorAll('.method');

Methods.forEach((method) => {
    const radio = method.querySelector('input')

    method.addEventListener('click', () => {
        radio.checked = true
    })
})