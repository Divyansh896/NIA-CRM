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

    let pressTimer;

// Detect if the device is a mobile device
function isMobileDevice() {
    return /Mobi|Android|iPhone/i.test(navigator.userAgent);
}

document.getElementById("phoneNumber").addEventListener("mousedown", function (event) {
    if (!isMobileDevice()) {
        // On PC, copy directly on click
        event.preventDefault();
        copyPhoneNumber(this);
    } else {
        // On mobile, start long press detection
        pressTimer = setTimeout(() => {
            event.preventDefault(); // Prevents calling on long press
            copyPhoneNumber(this);
        }, 500); // 500ms for long press
    }
});

document.getElementById("phoneNumber").addEventListener("mouseup", function () {
    clearTimeout(pressTimer); // Cancel copy if released early
});

document.getElementById("phoneNumber").addEventListener("mouseleave", function () {
    clearTimeout(pressTimer); // Cancel copy if the mouse leaves
});

function copyPhoneNumber(element) {
    const phoneNumber = element.getAttribute("href").replace("tel:", "");
    navigator.clipboard.writeText(phoneNumber)
        .then(() => alert("Phone number copied!"))
        .catch(err => console.error("Failed to copy: ", err));
}


