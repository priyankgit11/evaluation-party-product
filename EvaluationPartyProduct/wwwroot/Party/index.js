"use strict";
// SELECTING ELEMENTS
const createBtn = document.querySelector(".create-btn");
const table = document.querySelector(".party-tbl");
// const modal = document.querySelector(".modal");
// const closeBtn = document.querySelector(".close");
// const txtParty = document.querySelector(".modtxtParty");
// const updateBtn = document.querySelector(".sure-btn");

////// ADDING EVENT LISTENERS
//Modal close button
// closeBtn.addEventListener("click", () => {
//   modal.style.display = "none";
// });
//Clicks anywhere except modal then close modal
// window.addEventListener("click", (e) => {
//   if (e.target == modal) modal.style.display = "none";
// });
// updateBtn.addEventListener("click", () => {
//   console.log(String(txtParty.value).trim());
//   const data = {
//     partyName: String(txtParty.value).trim(),
//   };
//   updatePartyById(
//     `${origin}/api/Party/$${updateid}`,
//     "Cannot Update Party",
//     data
//   );

//   window.location.pathname = "/Party/index.html";
// });

/////////////// FUNCTIONS
const getJson = function (url, errMsg = "Something went wrong") {
  return fetch(url).then((response) => {
    if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
    return response.json();
  });
};
const deleteJson = function (url, errMsg = "Something went wrong") {
  return fetch(url, {
    method: "DELETE",
  }).then((response) => {
    if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
    return response.json();
  });
};
// const getPartyByIdJson = function (url, errMsg = "Something went wrong") {
//   return fetch(url).then((response) => {
//     if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
//     return response.json();
//   });
// };
// const updatePartyById = function (url, errMsg = "Something went wrong", data) {
//   return fetch(url, {
//     method: "PUT",
//     body: JSON.stringify(data),
//     headers: {
//       "Content-type": "application/json-patch+json",
//     },
//   }).then((response) => {
//     if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
//     return response.json();
//   });
// };

const showParty = function () {
  table.innerHTML = "";
  const origin = window.location.origin;
  getJson(`${origin}/api/Party`)
    .then((data) => {
      if (data.length == 0) table.style.display = "none";
      data.forEach((ele) => {
        table.insertAdjacentHTML(
          "beforeend",
          `
          <tr>
            <td>${ele.id}</td>
            <td>${ele.partyName}</td>
            <td>
              <button onclick="passToEdit(${ele.id})" class="btn edit-btn">Edit</button>
              <button onclick="passToDelete(${ele.id})" class="btn delete-btn">Delete</button>
            </td>
          </tr>
        `
        );
      });
    })
    .catch((err) => console.log(err));
};

function passToEdit(id) {
  // modal.style.display = "flex";
  window.location.href = `create.html?id=${id}`;
  // getPartyByIdJson(`${origin}/api/Party/${id}`, "Cannot Edit Party")
  //   .then((data) => {
  //     if (data.length === 0) {
  //       modal.style.display = "none";
  //       throw new Error("No Party Found");
  //     }
  //     txtParty.value = `${data.partyName}`;
  //     console.log(data.id);
  //     updateid = data.id;
  //   })
  //   .catch((err) => console.log(err));
}
function passToDelete(id) {
  if (confirm(`Are you sure you want to delete this Party?`)) {
    deleteJson(`${origin}/api/Party/${id}`, "Cannot Delete Party").catch(
      (err) => console.log(err)
    );
    window.location.href = "index.html";
  }
}
//INIT FUNCTION
const init = function () {
  createBtn.addEventListener("click", () => {
    window.location.href = "create.html";
  });
  showParty();
};

//CALLING FUNCTIONS
init();
