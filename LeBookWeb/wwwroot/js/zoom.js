const allHoverImages = document.querySelectorAll('.hover-image div img')
const imgContainer = document.querySelector('.img-container')

window.addEventListener('DOMContentLoaded', () => {
    allHoverImages[0].parentElement.classList.add('active')
})

allHoverImages.forEach((image) => {
  image.addEventListener('click', () => {
    imgContainer.querySelector('img').src = image.src
    zoom.style.backgroundImage = 'url(' + image.src + ')'
    resetActiveImg()
    image.parentElement.classList.add('active')
  })
})

function resetActiveImg() {
  allHoverImages.forEach((img) => {
    img.parentElement.classList.remove('active')
  })
}

const zoom = document.querySelector('.zoom')

zoom.addEventListener('mousemove', (e) => {
    zoom.style.backgroundImage = 'url(' + zoom.querySelector('img').src + ')'
  var zoomer = e.currentTarget
  e.offsetX ? (offsetX = e.offsetX) : (offsetX = e.touches[0].pageX)
  e.offsetY ? (offsetY = e.offsetY) : (offsetX = e.touches[0].pageX)
  x = (offsetX / zoomer.offsetWidth) * 100
  y = (offsetY / zoomer.offsetHeight) * 100
  zoomer.style.backgroundPosition = x + '% ' + y + '%'
})
