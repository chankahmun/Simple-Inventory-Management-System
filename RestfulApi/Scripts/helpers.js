function fnAppendProductsIntoTable(product) {


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

   
    $.ajax({
        url: "/GetProducts",
        async: true,
    }).done(function (response) {

        if (response['Status']) {
            let dataCount = response['Data'].length;

            for (let i = 0; i < dataCount; i++) {

                let product = response['Data'][i];
                fnAppendProductsIntoTable(product);
            }
        }


    }).fail(function () {
        console.log('error')
    });;
}

function fnCreateSupplier() {
    let productSupplierName = document.getElementById('InputSupplierRegistration').value;

    let data = {
        "Params": {
            "sProductSupplierName": productSupplierName,
        }

    }

    $.ajax({
        type: "POST",
        url: "/CreateProductSupplier",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data),
        async: true,
    }).done(function (response) {
        let msg = '';

        if (response['Status']) msg = 'Successful!! The Supplier Code is ' + response['Data'];

        if (!response['Status']) msg = response['Err']

        alert(msg);

        document.getElementById("InputSupplierRegistration").value = '';

    }).fail(function () {
        console.log('error')
    });;
}

function fnCreateProduct() {

    let productName = document.getElementById('InputProductName').value;
    let productSKU = document.getElementById('InputProductSKU').value;
    let productAvaibility = document.querySelector('input[name="productAvaibility"]:checked').value;
    let productSupplier = document.getElementById('InputProductSupplier').value;

    let data = {
        "Params": {
            "sProductName": productName,
            "sProductSKU": productSKU,
            "sProductAvaiblity": productAvaibility,
            "sProductSupplier": productSupplier,

        }

    }
         

    $.ajax({
        type: "POST",
        url: "/CreateProduct",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data),
        async: true,
    }).done(function (response) {
        let msg = '';

        if (response['Status']) msg = 'Successful';

        if (!response['Status']) msg = response['Err']

        alert(msg);

        document.getElementById("InputProductName").value = '';
        document.getElementById('InputProductSKU').value = '';
        document.getElementById('InputProductSupplier').value = '';
        document.getElementById('radioYes').checked = false;
        document.getElementById('radioNo').checked = false;
      
    }).fail(function () {
        console.log('error')
    });;
}