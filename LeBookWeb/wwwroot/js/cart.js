/* Amount */

const allItem = document.querySelectorAll('.item-product-cart')
const numberItem = document.querySelector('.number-items-checkbox')
numberItem.innerText = `${allItem.length}`

/* Checkbox */

const selectBtn = document.querySelector('.checkbox-all-product input')

const checkboxes = document.querySelectorAll('.checkbox-add-cart')

selectBtn.addEventListener('change', () => {
  if (selectBtn.checked == true) {
    checkboxes.forEach((checkbox) => {
      checkbox.checked = true
      checkbox.classList.add('checked')
    })
  } else {
    checkboxes.forEach((checkbox) => {
      checkbox.checked = false
      checkbox.classList.remove('checked')
    })
  }
})

checkboxes.forEach((checkbox) => {
  checkbox.addEventListener('click', () => {
    selectBtn.checked = false
  })
})
