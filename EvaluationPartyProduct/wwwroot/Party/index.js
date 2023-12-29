console.log("hello");

//////////////FUNCTIONS

function showToastMessage(message) {
  const toastMessage = document.getElementById("toastMessage");
  toastMessage.textContent = message;
  toastMessage.className = "show";

  // Hide toast message after 3 seconds
  setTimeout(() => {
    toastMessage.className = toastMessage.className.replace("show", "");
  }, 3000);
}
