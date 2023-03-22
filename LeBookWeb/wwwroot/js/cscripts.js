const searchOptions = document.querySelector('.search-options')
const dropDown = document.querySelector('.dropdown')
const overLay = document.querySelector('.overlay')

searchOptions.addEventListener('mouseover', () => {
    dropDown.classList.add('active')
    if (overLay != null) overLay.classList.add('show')
})

searchOptions.addEventListener('mouseleave', () => {
  dropDown.classList.remove('active')
    if (overLay != null) overLay.classList.remove('show')
})

const tabs = document.querySelectorAll('.right')
const tabBtns = document.querySelectorAll('.tab-btn')

const tab_Nav = function (tabBtnClick) {
  tabBtns.forEach((tabBtn) => {
    tabBtn.classList.remove('active')
  })

  tabs.forEach((tab) => {
    tab.classList.remove('active')
  })

  tabBtns[tabBtnClick].classList.add('active')
  tabs[tabBtnClick].classList.add('active')
}

tabBtns.forEach((tabBtn, i) => {
  tabBtn.addEventListener('mouseover', () => {
    tab_Nav(i)
  })
})

const plusBtn = document.querySelectorAll('.plus')
const minusBtn = document.querySelectorAll('.minus')
const number = document.querySelector('.number')

// INCREMENT
plusBtn.forEach((plus) => {
    plus.addEventListener('click', (event) => {
        var buttonClicked = event.target

        var inputPlus = buttonClicked.parentElement.children[1]

        var inputValue = inputPlus.value

        var newValue = parseInt(inputValue) + 1

        inputPlus.value = newValue
    })
})

// DECREMENT
minusBtn.forEach((minus) => {
    minus.addEventListener('click', (event) => {
        var buttonClicked = event.target

        var inputMinus = buttonClicked.parentElement.children[1]

        var inputValue = inputMinus.value

        var newValue = parseInt(inputValue) - 1

        if (newValue >= 0) {
            inputMinus.value = newValue
        } else {
            inputMinus.value = 0
            alert('Sản phẩm không được nhỏ hơn 0')
        }
    })
})


const linkElements = document.querySelectorAll('.link')

linkElements.forEach((dropdown) => {
    const caret = dropdown.querySelector('.caret')

    dropdown.addEventListener('click', () => {
        dropdown.classList.toggle('active')
        caret.classList.toggle('caret-rotate')
    })
})