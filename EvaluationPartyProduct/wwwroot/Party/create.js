"use strict";
// SELECTING ELEMENTS
const createBtn = document.querySelector(".create-btn");
const txtParty = document.querySelector(".txtParty");

var editID;
// ADDING EVENT LISTENERS
createBtn.addEventListener("click", () => {
  const data = collectData();
  if (Number.isInteger(editID) && !(editID === null))
    updateJson(`${origin}/api/Party/${editID}`, "Cannot Edit Party Name", data);
  else postJson(`${origin}/api/Party`, "Cannot Add Party Name", data);
  // window.location.href = "../Party/Index.html";
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
  if (String(txtParty.value).trim().length === 0) return;
  const data = {
    partyName: String(txtParty.value).trim(),
  };
  return data;
};

const decide = function () {
  const queryString = window.location.search;
  const urlParams = new URLSearchParams(queryString);
  if (urlParams.has("id")) {
    editID = parseInt(urlParams.get("id"));
    createBtn.innerHTML = "Edit Party";
    getPartyByIdJson(`${origin}/api/Party/${editID}`, "Cannot Edit Party")
      .then((data) => {
        if (data.length === 0) {
          modal.style.display = "none";
          throw new Error("No Party Found");
        }
        txtParty.value = `${data.partyName}`;
      })
      .catch((err) => console.log(err));
  }
  console.log(editID);
  console.log(3);
};
