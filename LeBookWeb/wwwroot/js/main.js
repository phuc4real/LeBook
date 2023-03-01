const prevIcon = '<i class="fa-solid fa-chevron-left"></i>'
const nextIcon = '<i class="fa-solid fa-chevron-right"></i>'

$('.owl-product').owlCarousel({
  loop: true,
  dots: false,
  margin: 10,
  nav: true,
  navText: [prevIcon, nextIcon],
  responsive: {
    0: {
      items: 1,
    },
    600: {
      items: 3,
    },
    900: {
      items: 6,
    },
  },
})

const linkElements = document.querySelectorAll('.link')

linkElements.forEach((dropdown) => {
  const caret = dropdown.querySelector('.caret')

  dropdown.addEventListener('click', () => {
    dropdown.classList.toggle('active')
    caret.classList.toggle('caret-rotate')
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
