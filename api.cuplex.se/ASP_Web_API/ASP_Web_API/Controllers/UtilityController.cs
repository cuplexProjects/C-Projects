using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ASP_Web_API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UtilityController : ApiController
    {
        /// <summary>
        /// Convert Base64 string to UTF-8
        /// </summary>
        /// <param name="b64InputText">Base64 encoded string</param>
        /// <returns>UTF-8 encoded string</returns>
        [HttpGet]
        [Route("api/utility/convertFromBase64String/{b64InputText}")]
        public string ConvertFromBase64String(string b64InputText)
        {
            try
            {
                byte[] data = Convert.FromBase64String(b64InputText);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Convert UTF-8 string to Base 64
        /// </summary>
        /// <param name="strInput">UTF-8 encoded string</param>
        /// <returns>Base64 encoded string </returns>
        [HttpGet]
        [Route("api/utility/convertToBase64String/{strInput}")]
        public string ConvertToBase64String(string strInput)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(strInput);
                return Convert.ToBase64String(data);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Html Encode content
        /// </summary>
        /// <param name="encodeData"></param>
        /// <returns>encoded string</returns>
        [HttpPost]
        [Route("api/utility/htmlEncode/")]
        public HttpResponseMessage HtmlEncodeString(HtmlEncodeData encodeData)
        {
            if (encodeData == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("No post data", Encoding.UTF8, "text/plain") };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(HttpUtility.HtmlEncode(encodeData.Content), Encoding.UTF8, "text/plain") };
        } 
        
        /// <summary>
        /// Html Decode content
        /// </summary>
        /// <param name="decodeData"></param>
        /// <returns>decoded string</returns>
        [HttpPost]
        [Route("api/utility/htmlDecode/")]
        public HttpResponseMessage HtmlDecodeString(HtmlEncodeData decodeData)
        {
            if (decodeData == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = new StringContent("No post data", Encoding.UTF8, "text/plain")};

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(HttpUtility.HtmlDecode(decodeData.Content), Encoding.UTF8, "text/plain") };
        }

        /// <summary>
        /// 
        /// </summary>
        public class HtmlEncodeData
        {
            /// <summary>
            /// String content to encode or decode
            /// </summary>
            public string Content { get; set; }
        }
    }
}
