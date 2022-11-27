const apiUrl = "http://localhost:44345"

function fnOnClickSystemOptions(evt, tabName) {
  var i, tabcontent, tablinks;
  
  tabcontent = document.getElementsByClassName("tabcontent");
  tablinks = document.getElementsByClassName("tablinks");
  
  for (i = 0; i < tabcontent.length; i++) {
    tabcontent[i].style.display = "none";
  }

  for (i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(" active", "");
  }
  
  document.getElementById(tabName).style.display = "block";
  evt.currentTarget.className += " active";
}

function fnAppendProductsIntoTable(product) {

	// insert data into table
    var tbodyRef = document.getElementById('tblProducts').getElementsByTagName('tbody')[0];
    var newRow = tbodyRef.insertRow();

    var ProductName = newRow.insertCell(0);
    var ProductSKU = newRow.insertCell(1);
    var ProductAvaibility = newRow.insertCell(2);
    var ProductSupplier = newRow.insertCell(3);

    ProductName.innerHTML = product['ProductName'];
    ProductSKU.innerHTML = product['ProductSKU'];
    ProductAvaibility.innerHTML = product['ProductAvaibility'];
    ProductSupplier.innerHTML = product['ProductSupplier'];
}

function fnGetProducts() {
	
	// remove all products from table
    $("#tblProducts tbody").html("");
	
	// send request message to api
    $.ajax({
        url: apiUrl + "/GetProducts",
        async: true,
		headers: { 'Access-Control-Allow-Origin': apiUrl, 'Access-Control-Allow-Methods': 'GET', 'Access-Control-Allowance-Headers': 'Content-Type, X-Auth-Token, Origin, Authorization' },
    }).done(function (response) {

        if (response['Status']) {
            let dataCount = response['Data'].length;

            for (let i = 0; i < dataCount; i++) {

                let product = response['Data'][i];
                fnAppendProductsIntoTable(product);
            }
        }


    }).fail(function (error) {
        console.log('error:', error)
    });;
}

function fnCreateSupplier() {
	// get all data from fields
    let productSupplierName = document.getElementById('InputSupplierRegistration').value;
	let productSupplierEmail = document.getElementById('InputSupplierEmailRegistration').value;
	let productSupplierPhoneNo = document.getElementById('InputSupplierPhoneNoRegistration').value;
	
	// form a request message
    let data = {
        "Params": {
            "sProductSupplierName": productSupplierName,
			"sProductSupplierEmail": productSupplierEmail,
			"sProductSupplierPhoneNo": productSupplierPhoneNo,
        }

    }
	
	// send request message to api
    $.ajax({
        type: "POST",
        url: apiUrl + "/CreateProductSupplier",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data),
        async: true,
    }).done(function (response) {
        let msg = '';

        if (response['Status']) msg = 'Successful!! The Supplier Code is ' + response['Data'];

        if (!response['Status']) msg = response['Err']

        alert(msg);

		// clear all data in fields
        document.getElementById("InputSupplierRegistration").value = '';
		document.getElementById("InputSupplierEmailRegistration").value = '';
		document.getElementById("InputSupplierPhoneNoRegistration").value = '';

    }).fail(function () {
        console.log('error')
    });;
}

function fnCreateProduct() {
	// get all data from fields
    let productName = document.getElementById('InputProductName').value;
    let productSKU = document.getElementById('InputProductSKU').value;
    let productAvaibility = document.querySelector('input[name="productAvaibility"]:checked').value;
    let productSupplier = document.getElementById('InputProductSupplier').value;

	// form a request message
    let data = {
        "Params": {
            "sProductName": productName,
            "sProductSKU": productSKU,
            "sProductAvaiblity": productAvaibility,
            "sProductSupplier": productSupplier,

        }

    }
         
	// send request message to api 
    $.ajax({
        type: "POST",
        url: apiUrl + "/CreateProduct",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data),
        async: true,
    }).done(function (response) {
        let msg = '';

        if (response['Status']) msg = 'Successful';

        if (!response['Status']) msg = response['Err']

        alert(msg);
		
		// clear all data in fields
        document.getElementById("InputProductName").value = '';
        document.getElementById('InputProductSKU').value = '';
        document.getElementById('InputProductSupplier').value = '';
        document.getElementById('radioYes').checked = false;
        document.getElementById('radioNo').checked = false;
      
    }).fail(function () {
        console.log('error')
    });;
}