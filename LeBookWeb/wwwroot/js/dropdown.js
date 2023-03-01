const searchOptions = document.querySelector('.search-options')
const dropDown = document.querySelector('.dropdown')
const overLay = document.querySelector('.overlay')

searchOptions.addEventListener('mouseover', () => {
  dropDown.classList.add('active')
  overLay.classList.add('show')
})

searchOptions.addEventListener('mouseleave', () => {
  dropDown.classList.remove('active')
  overLay.classList.remove('show')
})

// window.addEventListener('scroll', () => {
//   if (window.pageYOffset > 300) {
//     dropDown.classList.remove('active')
//     overLay.classList.remove('show')
//   }
// })

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
