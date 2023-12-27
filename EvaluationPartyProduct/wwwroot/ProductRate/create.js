"use strict";
// SELECTING ELEMENTS
const createBtn = document.querySelector(".create-btn");
const optProduct = document.getElementById("optProduct");
const txtRate = document.getElementById("txtRate");

var editID;
// ADDING EVENT LISTENERS
createBtn.addEventListener("click", () => {
  const data = collectData();
  console.log(data);
  if (Number.isInteger(editID) && !(editID === null))
    updateJson(`${origin}/api/ProductRate/${editID}`, "Cannot Edit ", data);
  else postJson(`${origin}/api/ProductRate`, "Cannot Add ", data);
  window.location.href = "/ProductRate/Index.html";
});

// optParty.addEventListener("change");

//  FUNCTIONS
const postJson = function (url, errMsg = "Something went wrong", data) {
  console.log("Hello");
  console.log(JSON.stringify(data));
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

const getJson = function (url, errMsg = "Something went wrong") {
  return fetch(url).then((response) => {
    if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
    return response.json();
  });
};

const getPartyByIdJson = function (url, errMsg = "Something went wrong") {
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
  const data = {
    productId: parseInt(optProduct.value),
    rate: parseFloat(txtRate.value),
  };
  return data;
};

const populate = function () {
  //   let partyHtml = "";
  //   let productHtml = "";
  getJson(`${origin}/api/Product`).then((data) => {
    console.log(data);
    data.map((ele) => {
      optProduct.insertAdjacentHTML(
        "beforeend",
        `
        <option value="${ele.id}">${ele.productName}</option>
    `
      );
    });
  });
  //   optParty.insertAdjacentHTML("beforeend", partyHtml);
  //   optProduct.insertAdjacentHTML("beforeend", productHtml);
};

const decide = function () {
  const queryString = window.location.search;
  const urlParams = new URLSearchParams(queryString);
  //populate data first
  populate();
  //now keep editing elements selected
  if (urlParams.has("id")) {
    editID = parseInt(urlParams.get("id"));
    createBtn.innerHTML = "Edit";
    getPartyByIdJson(`${origin}/api/ProductRate/${editID}`, "Cannot Find Rate ")
      .then((data) => {
        if (data.length === 0) {
          modal.style.display = "none";
          throw new Error("No Rate Found");
        }
        optProduct.value = data.productId;
        txtRate.value = data.rate;
      })
      .catch((err) => console.log(err));
  }
  console.log(editID);
};
