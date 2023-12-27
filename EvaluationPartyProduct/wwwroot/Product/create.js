"use strict";
// SELECTING ELEMENTS
const createBtn = document.querySelector(".create-btn");
const txtProduct = document.querySelector(".txtProduct");

var editID;
// ADDING EVENT LISTENERS
createBtn.addEventListener("click", () => {
  const data = collectData();
  console.log(Number.isInteger(editID) && !(editID === null));
  if (Number.isInteger(editID) && !(editID === null))
    updateJson(
      `${origin}/api/Product/${editID}`,
      "Cannot Edit Product Name",
      data
    );
  else postJson(`${origin}/api/Product`, "Cannot Add Product Name", data);
  window.location.href = "../Product/Index.html";
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

const getProductByIdJson = function (url, errMsg = "Something went wrong") {
  return fetch(url).then((response) => {
    if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
    return response.json();
  });
};

const updateJson = function (url, errMsg = "Something went wrong", data) {
  return fetch(url, {
    method: "PUT",
    body: JSON.stringify(data),
    headers: {
      "Content-type": "application/json-patch+json",
    },
  }).then((response) => {
    if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
    return response.json();
  });
};

const collectData = function () {
  if (String(txtProduct.value).trim().length === 0) return;
  const data = {
    productName: String(txtProduct.value).trim(),
  };
  return data;
};

const decide = function () {
  const queryString = window.location.search;
  const urlParams = new URLSearchParams(queryString);
  if (urlParams.has("id")) {
    editID = parseInt(urlParams.get("id"));
    createBtn.innerHTML = "Edit Product";
    getProductByIdJson(`${origin}/api/Product/${editID}`, "Cannot Edit Product")
      .then((data) => {
        if (data.length === 0) {
          modal.style.display = "none";
          throw new Error("No Product Found");
        }
        txtProduct.value = `${data.productName}`;
      })
      .catch((err) => console.log(err));
  }
  console.log(editID);
};
