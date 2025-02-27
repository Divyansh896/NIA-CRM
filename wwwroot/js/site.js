// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const btnBackToTop = document.getElementById('btnBackToTop')
const footer = document.getElementById('footer-scroll')

function BackToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth' // Adds a smooth scroll effect
    })
}
footer.addEventListener('click', () => {
    BackToTop()
})
btnBackToTop.addEventListener('click', () => {
    BackToTop()
})





