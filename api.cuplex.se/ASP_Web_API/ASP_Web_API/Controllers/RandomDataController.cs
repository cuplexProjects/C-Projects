using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web.Http;
using ASP_Web_API.Source;

namespace ASP_Web_API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RandomDataController : ApiController
    {
        private const int MAX_BYTE_DATASENT = 10485760;
        private const int MAX_STRING_DATALENGTH = 100000;
        
        /// <summary>
        /// Using Secure Random to Generate Hexadecimal data
        /// </summary>
        /// <param name="length">Number of characters to generate</param>
        /// <returns>Random uppercase Hex string</returns>
        [HttpGet]
        [Route("api/randomdata/generateHexRandom/{length}")]
        public string GetHexRandom(int length)
        {
            RandomGenerator rndGenerator = new RandomGenerator();
            if (length > MAX_STRING_DATALENGTH)
                length = MAX_STRING_DATALENGTH;

            if (length <= 0)
                return null;

            return rndGenerator.GenerateRandomHexString(length);
        }

        /// <summary>
        /// Using Secure Random to Generate a Alphanumeric sequence
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/randomdata/generateAlphanumericRandom/{length}")]
        public string GetAlphanmumericRandom(int length)
        {
            RandomGenerator rndGenerator = new RandomGenerator();
            if (length > MAX_STRING_DATALENGTH)
                length = MAX_STRING_DATALENGTH;

            if (length <= 0)
                return null;

            return rndGenerator.GetAlphanmumericRandom(length);
        }

        /// <summary>
        /// Using Secure Random to Generate a numeric sequence
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/randomdata/generateNumericRandom/{length}")]
        public string GetNumericData(int length)
        {
            RandomGenerator rndGenerator = new RandomGenerator();
            if (length > MAX_STRING_DATALENGTH)
                length = MAX_STRING_DATALENGTH;

            if (length <= 0)
                return null;

            return rndGenerator.GenerateRandomNumericString(length);
        }

        /// <summary>
        /// Using Secure Random to Generate a base64 encoded string
        /// </summary>
        /// <param name="length">Number of characters to generate</param>
        /// <returns>Random uppercase Base64  string</returns>
        [HttpGet]
        [Route("api/randomdata/generateBase64Random/{length}")]
        public string GetBase64(int length)
        {
            RandomGenerator rndGenerator = new RandomGenerator();
            if (length > MAX_STRING_DATALENGTH)
                length = MAX_STRING_DATALENGTH;

            if (length <= 0)
                return null;

            return rndGenerator.GenerateRandomBase64String(length);
        }

        /// <summary>
        /// Using Secure Random to Generate a  upper and lowercase alfanumerical string including &quot;!@#&amp;$%=?+-&lt;&gt;_-[]{}^*/\|~.,;:&quot;
        /// </summary>
        /// <param name="length">Number of characters to generate</param>
        /// <returns>Random upper and lowercase alfanumerical string including special chars</returns>
        [HttpGet]
        [Route("api/randomdata/generateSpecialCharRandom/{length}")]
        public string GetBaseSpecielChar(int length)
        {
            RandomGenerator rndGenerator = new RandomGenerator();
            if (length > MAX_STRING_DATALENGTH)
                length = MAX_STRING_DATALENGTH;

            if (length <= 0)
                return null;

            return rndGenerator.GenerateRandomSpecialCharString(length);
        }

        /// <summary>
        /// Using Secure Random to Generate a alfanumerical password including these special characters "!+-@#?$"
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/randomdata/generateRandomPassword/{length}")]
        public string GetRandomPassword(int length)
        {
            RandomGenerator rndGenerator = new RandomGenerator();
            if (length > MAX_STRING_DATALENGTH)
                length = MAX_STRING_DATALENGTH;

            if (length <= 0)
                return null;

            return rndGenerator.GenerateRandomPasswordString(length);
        }

        /// <summary>
        /// Returns a random binary file input length specified in kb
        /// </summary>
        /// <param name="length">Length is in kb. Minimum 1 kb Maximum 10485760 kb = 10 mb </param>
        /// <returns>randomdata_YYYY_MM_DD.bin</returns>
        [HttpGet]
        [Route("api/randomdata/getRandomBinaryFile/{length}")]
        public HttpResponseMessage GetRandomBinaryFile(int length)
        {
            length = Math.Max(1, length);
            length = length*1024;
            RandomGenerator rndGenerator = new RandomGenerator();
            if (length > MAX_BYTE_DATASENT)
                length = MAX_BYTE_DATASENT;
            

            byte[] rndData = rndGenerator.GenerateRandomByteArray(length);

            var resp = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(rndData)};
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            resp.Content.Headers.ContentLength = length;
            resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(DispositionTypeNames.Attachment)
            {
                CreationDate = DateTime.Now,
                FileName = "randomdata_" + DateTime.Now.ToString("yyyy_MM_dd") + ".bin",
                Size = length
            };

            return resp;
        }
    }
}