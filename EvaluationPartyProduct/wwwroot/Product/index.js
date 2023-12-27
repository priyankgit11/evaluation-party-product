"use strict";
// SELECTING ELEMENTS
const createBtn = document.querySelector(".create-btn");
const table = document.querySelector(".product-tbl");
// const modal = document.querySelector(".modal");
// const closeBtn = document.querySelector(".close");
const txtProduct = document.querySelector(".modtxtProduct");
// const updateBtn = document.querySelector(".update-btn");

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
//   console.log(String(txtProduct.value).trim());
//   const data = {
//     partyName: String(txtProduct.value).trim(),
//   };
//   updateProductById(
//     `${origin}/api/Product/$${updateid}`,
//     "Cannot Update Product",
//     data
//   );
//   window.location.pathname = "/Product/index.html";
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
// const getProductByJson = function (url, errMsg = "Something went wrong") {
//   return fetch(url).then((response) => {
//     if (!response.ok) throw new Error(`${errMsg} (${response.status})`);
//     return response.json();
//   });
// };
// const updateProductById = function (
//   url,
//   errMsg = "Something went wrong",
//   data
// ) {
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

const showProduct = function () {
  table.innerHTML = "";
  const origin = window.location.origin;
  getJson(`${origin}/api/Product`)
    .then((data) => {
      if (data.length == 0) table.style.display = "none";
      data.forEach((ele) => {
        table.insertAdjacentHTML(
          "beforeend",
          `
          <tr>
            <td>${ele.id}</td>
            <td>${ele.productName}</td>
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
  // getProductByJson(`${origin}/api/Product/${id}`, "Cannot Edit Product")
  //   .then((data) => {
  //     if (data.length === 0) {
  //       modal.style.display = "none";
  //       throw new Error("No Product Found");
  //     }
  //     txtProduct.value = `${data.productName}`;
  //     console.log(data.id);
  //     updateid = data.id;
  //   })
  //   .catch((err) => console.log(err));
}
function passToDelete(id) {
  if (confirm(`Are you sure you want to delete this Product?`)) {
    deleteJson(`${origin}/api/Product/${id}`, "Cannot Delete Product").catch(
      (err) => console.log(err)
    );
    window.location.reload();
  }
}
//INIT FUNCTION
const init = function () {
  createBtn.addEventListener("click", () => {
    window.location.href = "index.html";
  });
  showProduct();
};

//CALLING FUNCTIONS
init();
