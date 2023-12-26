"use strict";
// SELECTING ELEMENTS
const createBtn = document.querySelector(".create-btn");
const txtProduct = document.querySelector(".txtProduct");

// ADDING EVENT LISTENERS
createBtn.addEventListener("click", () => {
  collectData();
});

//  FUNCTIONS
const postJson = function (url, errMsg = "Something went wrong", data) {
  console.log("Hello");
  return fetch(url, {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-type": "application/json",
    },
  })
    .then((response) => {
      if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
      return response.json();
    })
    .catch((err) => console.log(err));
};

const collectData = function () {
  if (String(txtProduct.value).trim().length === 0) return;
  const data = {
    productName: String(txtProduct.value).trim(),
  };
  postJson(`${origin}/api/Product`, "Cannot Add Product Name", data);
};

const origin = window.location.origin;
