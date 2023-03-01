/* eye */

const pwShowHide = document.querySelector('.showhide')
const pwField = document.querySelector('.show-password')

pwShowHide.addEventListener('click', () => {
  if (pwField.type === 'password') {
    pwField.type = 'text'
    pwShowHide.classList.replace('fa-eye-slash', 'fa-eye')
  } else {
    pwField.type = 'password'
    pwShowHide.classList.replace('fa-eye', 'fa-eye-slash')
  }
})

/* Login , Register */

const toggleBtns = document.querySelectorAll('.toggle-btn')
const loginForm = document.getElementById('login')
const registerForm = document.getElementById('register')

toggleBtns.forEach((toggleBtn, index) => {
  toggleBtn.addEventListener('click', () => {
    reset()
    toggleBtn.classList.add('active')
    if (index == 1) {
      registerForm.style.display = 'block'
      loginForm.style.display = 'none'
    } else {
      loginForm.style.display = 'block'
      registerForm.style.display = 'none'
    }
  })
})

function reset() {
  toggleBtns.forEach((toggleBtn) => {
    toggleBtn.classList.remove('active')
  })
}
