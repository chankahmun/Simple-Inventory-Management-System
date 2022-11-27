using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static WebApiDemo.Models.ApiBaseModel;
using WebApiDemo.Helpers;
using System.Data;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class ProductsController : ApiController
    {

        #region a gateway to get the products list
        [Route("GetProducts")]
        [HttpGet]
        public ResponseModel GetProducts()
        {
            ResponseModel oResp = new ResponseModel();

            try
            {
                oResp = fnGetProducts();
            }
            catch
            {
                oResp.Err = SERVER_ERR;
                oResp.Status = false;
            }

            return oResp;
        }
        #endregion

        #region a gateway to generate product
        [Route("CreateProduct")]
        [HttpPost]
        public ResponseModel CreateProduct(RequestModel obj)
        {
            ResponseModel oResp = new ResponseModel();

            string sProductName = "",
                   sProductSKU = "",
                   sProductAvaiblity = "",
                   sProductSupplier = "",
                   sErrMsg = "";

            try
            {
                if (obj?.Params == null) 
                { 
                    oResp.Err = PARAMETERS_MISSING; 
                    return oResp; 
                }

                Dictionary<string, string> Params = obj.Params;

                Params.TryGetValue("sProductName", out sProductName);
                if (Utils.IsStringEmptyOrNull(sProductName)) 
                {
                    oResp.Err = PARAMETERS_MISSING;
                    return oResp;
                }

                Params.TryGetValue("sProductSKU", out sProductSKU);
                if (Utils.IsStringEmptyOrNull(sProductSKU))
                {
                    oResp.Err = PARAMETERS_MISSING;
                    return oResp;
                }

                if (Utils.CInt(sProductSKU) <= 0)
                {
                    oResp.Err = INVALID_SKU;
                    return oResp;
                }

                Params.TryGetValue("sProductAvaiblity", out sProductAvaiblity);
                if (Utils.IsStringEmptyOrNull(sProductAvaiblity))
                {
                    oResp.Err = PARAMETERS_MISSING;
                    return oResp;
                }

                Params.TryGetValue("sProductSupplier", out sProductSupplier);
                if (Utils.IsStringEmptyOrNull(sProductSupplier))
                {
                    oResp.Err = PARAMETERS_MISSING;
                    return oResp;
                }

                if (!fnIsValidSupplier(sProductSupplier, out sErrMsg))
                {
                    oResp.Err = INVALID_SUPPLIER;
                    return oResp;
                }

                if (!Utils.IsStringEmptyOrNull(sErrMsg))
                {
                    oResp.Err = SERVER_ERR;
                    return oResp;
                }

                ProductsModel product = new ProductsModel();
                product.ProductName = sProductName;
                product.ProductSKU = sProductSKU;
                product.ProductAvaibility = sProductAvaiblity;
                product.ProductSupplier = sProductSupplier;

                oResp = fnCreateProduct(product);

            }
            catch
            {
                oResp.Err = SERVER_ERR;
                oResp.Status = false;
            }

            return oResp;


        }
        #endregion

        #region a gateway to create product supplier

        [Route("CreateProductSupplier")]
        [HttpPost]
        public ResponseModel CreateProductSupplier(RequestModel obj)
        {
            ResponseModel oResp = new ResponseModel();

            string sProductSupplierName, sProductSupplierEmail, sProductSupplierPhoneNo;

            try
            {
                if (obj?.Params == null)
                {
                    oResp.Err = PARAMETERS_MISSING;
                    return oResp;
                }

                Dictionary<string, string> Params = obj.Params;

                Params.TryGetValue("sProductSupplierName", out sProductSupplierName);
                if (Utils.IsStringEmptyOrNull(sProductSupplierName))
                {
                    oResp.Err = PARAMETERS_MISSING;
                    return oResp;
                }

                Params.TryGetValue("sProductSupplierEmail", out sProductSupplierEmail);
                if (!Utils.IsEmail(sProductSupplierEmail))
                {
                    oResp.Err = INVALID_EMAIL;
                    return oResp;
                }


                Params.TryGetValue("sProductSupplierPhoneNo", out sProductSupplierPhoneNo);
                if (!Utils.IsPhoneNo(sProductSupplierPhoneNo))
                {
                    oResp.Err = INVALID_PHONE_NO;
                    return oResp;
                }
                oResp = fnCreateProductSupplier(sProductSupplierName, sProductSupplierEmail, sProductSupplierPhoneNo);
            }
            catch
            {
                oResp.Err = SERVER_ERR;
                oResp.Status = false;
            }

            return oResp;

   
        }
        #endregion

        #region a function which create supplier name in ProductSupplier table
        private ResponseModel fnCreateProductSupplier(string pSupplier, string pSupplierEmail, string pSupplierPhoneNo)
        {
            ResponseModel oResp = new ResponseModel();
            DataSet ds = null;
            string sError, sSqlScript;

            try
            {

                sSqlScript = "Insert ProductSupplier (SupplierName, Email, PhoneNumber) values ('{0}', '{1}', '{2}') ; SELECT * FROM ProductSupplier WHERE SupplierId = SCOPE_IDENTITY();";

                sSqlScript = String.Format(sSqlScript, pSupplier, pSupplierEmail, pSupplierPhoneNo);
                DbHelper db = new DbHelper();
                bool bRet = db.Select(Helpers.ExecuteType.Text, sSqlScript, null, out ds, out sError);

                if (bRet && Utils.isDataSetAvailable(ds))
                {
                    oResp.Data = Utils.CStr(ds.Tables[0].Rows[0]["SupplierId"]);

                    oResp.Status = true;

                }

                if (!Utils.IsStringEmptyOrNull(sError))
                {
                    oResp.Err = SERVER_ERR;
                    oResp.Status = false;
                }

            }
            catch
            {
                oResp.Err = SERVER_ERR;
                oResp.Status = false;
            }

            return oResp;
        }
        #endregion

        #region a function which get products from ProductsDetail table
        private ResponseModel fnGetProducts()
        {
            ResponseModel oResp = new ResponseModel();

            DataSet ds = null;
            string sError, sSqlScript;

          
            try
            {
                sSqlScript = "Select PD.ProductId, PD.ProductName, PD.ProductSKU, PD.ProductAvaibility , PS.SupplierName from ProductsDetail PD WITH (NOLOCK), ProductSupplier PS WHERE PD.ProductSupplierId = PS.SupplierId";

                DbHelper db = new DbHelper();
                
                bool bRet = db.Select(Helpers.ExecuteType.Text, sSqlScript, null, out ds, out sError);

                if (bRet && Utils.isDataSetAvailable(ds))
                {
                    List<ProductsModel> lstProducts = new List<ProductsModel>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstProducts.Add(new ProductsModel()
                        {
                            
                            ProductName = Utils.CStr(dr["ProductName"]),
                            ProductSKU = Utils.CStr(dr["ProductSKU"]),
                            ProductAvaibility = Utils.CProductAvibility(dr["ProductAvaibility"]),
                            ProductSupplier = Utils.CStr(dr["SupplierName"])
                        });
                    }

                    oResp.Data = lstProducts;
                    oResp.Status = true;

                }
            }
            catch
            {
                oResp.Err = SERVER_ERR;
                oResp.Status = false;
            }

            return oResp;
        }
        #endregion

        #region a function which create product in ProductsDetail table
        private ResponseModel fnCreateProduct(ProductsModel sProduct)
        {

            ResponseModel oResp = new ResponseModel();

            DataSet ds = null;
            string sError , sSqlScript;

            try
            {

                sSqlScript = "Insert ProductsDetail (ProductName, ProductSKU, ProductAvaibility, ProductSupplierId) values ('{0}',{1},{2},{3}) ";

                sSqlScript = String.Format(sSqlScript, sProduct.ProductName, sProduct.ProductSKU, sProduct.ProductAvaibility, sProduct.ProductSupplier);
                DbHelper db = new DbHelper();
                bool bRet = db.Select(Helpers.ExecuteType.Text, sSqlScript, null, out ds, out sError);

                if (bRet)
                {
                    oResp.Status = true;
                }
                
            }
            catch
            {
                oResp.Err = SERVER_ERR;
                oResp.Status = false;
            }

            return oResp;
        }
        #endregion

        #region a function which validate the validity of product supplier

        private bool fnIsValidSupplier(string pSupplier, out string pErr)
        {
            bool isValid = false;
            string sError = "", sSqlScript = "";
            
            DataSet ds = null;

            pErr = "";

            try
            {
                sSqlScript = "Select SupplierName from ProductSupplier WITH (NOLOCK) where SupplierId = {0}";

                sSqlScript = String.Format(sSqlScript, pSupplier);

                DbHelper db = new DbHelper();
                bool bRet = db.Select(Helpers.ExecuteType.Text, sSqlScript, null, out ds, out sError);

                if (bRet && Utils.isDataSetAvailable(ds))
                {
                    
                    string supplierName = Utils.CStr(ds.Tables[0].Rows[0]["SupplierName"]);

                    if (!Utils.IsStringEmptyOrNull(supplierName)) { isValid = true; }
                }

            }
            catch
            {
                pErr = SERVER_ERR;
            }

            return isValid;
        }
        #endregion

    }
}
