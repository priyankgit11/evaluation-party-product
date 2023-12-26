"use strict";
// SELECTING ELEMENTS
const createBtn = document.querySelector(".create-btn");
const txtParty = document.querySelector(".txtParty");

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
  if (String(txtParty.value).trim().length === 0) return;
  const data = {
    partyName: String(txtParty.value).trim(),
  };
  postJson(`${origin}/api/Party`, "Cannot Add Party Name", data);
};

const origin = window.location.origin;
postJson(`${origin}/api/Party`);
console.log("Helo");
