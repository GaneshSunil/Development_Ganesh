using GenomeNext.Data.EntityModel;
using GenomeNext.Cloud.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GenomeNext.Portal.Controllers
{
    //[AuthorizeRedirect]
    public class S3FileUploadController : BaseController
    {
        public GNCloudStorageService CloudStorageService { get; set; }

        public S3FileUploadController ()
	    {
            //instantiate services
            CloudStorageService = new GNCloudStorageService();
        }

        private void InitCloudServices()
        {
            if (CloudStorageService.AWSConfigId == Guid.Empty)
            {
                CloudStorageService.AWSConfigId = UserContact.GNOrganization.AWSConfigId;
                CloudStorageService.ConnectToCloudStorage();
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignRequest()
        {
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string reqJson = new StreamReader(req).ReadToEnd();

            IDictionary<object, object> reqObj = null;
            try
            {
                reqObj = JsonConvert.DeserializeObject<IDictionary<object, object>>(reqJson);

                if (reqObj.ContainsKey("headers")) 
                {
                    return SignRESTRequest(reqObj);
                }
                else 
                {
                    return SignPolicy(reqObj);
                }
            }
            catch (Exception ex)
            {
                // Try and handle malformed POST body
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,ex.Message);
            }
        }

        public ActionResult UploadSuccess()
        {
            try
            {
                Dictionary<string, object> result = null;

                string bucket = Request["bucket"];
                string key = Request["key"];
                string name = Request["name"];
                string uuid = Request["uuid"];

                if (!string.IsNullOrEmpty(bucket) && !string.IsNullOrEmpty(key))
                {
                    InitCloudServices();

                    var fileMetadata = CloudStorageService.GetS3FileMetadata(bucket,key);

                    result = new Dictionary<string, object>{
                        {"complete", new GNCloudFile(){
                                Id = Guid.Parse(uuid),
                                Description = name,
                                Volume = bucket,
                                FileName = key,
                                FileSize = long.Parse(fileMetadata["ContentLength"].ToString())
                            }
                        }
                    };
                }
                else
                {
                    result = new Dictionary<string, object>{
                        {"error","An error occurred during upload of the file!!"}
                    };
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                // Try and handle malformed POST body
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        private JsonResult SignRESTRequest(IDictionary<object, object> reqObj)
        {
            JsonResult jsonResult = null;
            string headers = (string)reqObj["headers"];

            InitCloudServices();

            if (isValidRestRequest(headers))//, CloudStorageService.AWSS3Bucket))
            {
                byte[] signedHeadersBytes = null;
                string signedHeaders = null;

                using (var hashAlgorithm = new HMACSHA1(
                    Encoding.UTF8.GetBytes(CloudStorageService.AWSConfigEntity.AWSSecretAccessKey)))
                {
                    signedHeadersBytes =
                            hashAlgorithm.ComputeHash(
                                Encoding.UTF8.GetBytes(headers));
                }

                signedHeaders = Convert.ToBase64String(signedHeadersBytes);

                var result = new Dictionary<string, string>{
                    {"signature",signedHeaders}
                };

                jsonResult = Json(result);
            }
            //IF false, return 400 invalid policy result
            else
            {
                this.Response.StatusCode = 400;
                var result = new Dictionary<string, object>{
                    {"invalid",true}
                };

                jsonResult = Json(result);
            }

            return jsonResult;
        }

        private JsonResult SignPolicy(IDictionary<object,object> policyObj)
        {
            JsonResult jsonResult = null;

            InitCloudServices();

            //check policy is valid
            //IF True, sign policy and return JSON result
            if (isPolicyValid(policyObj))//, CloudStorageService.AWSS3Bucket)) 
            {
                var s = JsonSerializer.CreateDefault();
                var w = new StringWriter();
                s.Serialize(w, policyObj);
                w.Flush();

                string policy = Convert.ToBase64String(Encoding.UTF8.GetBytes(w.ToString()));
                string signedPolicy = CloudStorageService.GetSignature(policy);

                var result = new Dictionary<string,string>{
                    {"policy",policy},
                    {"signature",signedPolicy}
                };

                jsonResult = Json(result);
            }
            //IF false, return 400 invalid policy result
            else 
            {
                this.Response.StatusCode = 400;
                var result = new Dictionary<string,object>{
                    {"invalid",true}
                };

                jsonResult = Json(result);
            }

            return jsonResult;
        }

        // Ensures the REST request is targeting the correct bucket.
        private bool isValidRestRequest(string headerStr, string expectedBucket = null) 
        {
            bool isValid = true;//false;    
            
            if (expectedBucket != null)
            {
                isValid = Regex.Matches(headerStr, "\\/" + expectedBucket + "\\/.+$").Count != 0;
            }

            return isValid;
        }

        // Ensures the policy document associated with a "simple" (non-chunked) request is
        // targeting the correct bucket as expected.
        private bool isPolicyValid(IDictionary<object, object> policy, string expectedBucket = null) 
        {
            bool isValid = true;// false;

            if (expectedBucket != null)
            {
                JToken bucket = null;

                foreach (JObject condition 
                    in (JArray)policy["conditions"])
                {
                    bool success = condition.TryGetValue("bucket", out bucket);
                    if (success) break;
                }

                if (bucket != null)
                {
                    isValid = bucket.ToString().Equals(expectedBucket);
                }
            }

            return isValid;
        }
    }
}