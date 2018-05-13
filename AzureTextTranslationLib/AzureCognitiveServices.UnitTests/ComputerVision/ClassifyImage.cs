using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AzureCS.API.Lib.ComputerVision;
using Microsoft.TeamFoundation.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AzureCognitiveServices.UnitTests.ComputerVision
{
    [TestClass]
    public class ClassifyImage
    {
        private const string BasePath = @"D:\Martin\Bilder\mb\";
        private string imagePath;
        private List<int> _randomNumbersList;
        private List<string> _randomImageFiles;
        private readonly AnalyzeImage _analyzeImage;
        private static TestContext _context;


        public ClassifyImage()
        {
            _analyzeImage = new AnalyzeImage();
            _randomNumbersList = new List<int>();
        }


        [TestInitialize]
        public void Init()
        {
            // Get random image
            var directoryInfo = new DirectoryInfo(BasePath);

            var fileInfos = directoryInfo.GetFiles("*.jpg", SearchOption.AllDirectories);
            GenerateRandomNumbers();
            _randomImageFiles = new List<string>();


            foreach (int r in _randomNumbersList)
            {
                int pos = Math.Abs(r) % fileInfos.Length;
                _randomImageFiles.Add(fileInfos[pos].FullName);
            }

        }



        public void GenerateRandomNumbers()
        {
            var rndGenerator = RandomNumberGenerator.Create();


            // Create 100 random 32bit integers
            byte[] buffer = new byte[3200];
            rndGenerator.GetBytes(buffer);

            int offset = 0;
            for (int i = 0; i < 5; i++)
            {
                int next = BitConverter.ToInt32(buffer, offset);
                _randomNumbersList.Add(next);
                offset += 32;
            }

        }

        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            _context = context;
        }




        [TestMethod]
        public void VerifyTestdata()
        {
            Assert.IsNotNull(_randomImageFiles, "ImageFilename list can not be null");
            Assert.IsTrue(_randomImageFiles.Count == 100);
            Assert.IsTrue(_randomImageFiles.All(File.Exists), "Found non existing file");

        }





        //Lets begin
        [TestMethod]
        public void GetAnalasysDataFromImages()
        {
            var jSonObjects = new ConcurrentQueue<object>();
            var jSonTextObjects = new ConcurrentQueue<string>();
            List<Task<string>> taskList = new List<Task<string>>();

            var loopResult = Parallel.For(0, _randomImageFiles.Count, async (i, state) => 
            {
                if (state.ShouldExitCurrentIteration)
                {
                    if (state.LowestBreakIteration < i)
                        return;
                }
                
                string path = _randomImageFiles[i];


                Task.WaitAll(taskList.ToArray());
                string result = _analyzeImage.MakeAnalysisRequest(path).Result;
                
          

                
                

                ProcessJsData(jSonTextObjects, jSonObjects, result);
            });

            while (!loopResult.IsCompleted)
            {
                Task.Delay(50).RunSynchronously();
            }

            _context.WriteLine("Completed");
            var sb=  new StringBuilder();
            while (!jSonTextObjects.IsEmpty)
            {
                if (jSonTextObjects.TryDequeue(out string jsonStr))
                {
                    sb.Append(jsonStr);
                    _context.WriteLine(jsonStr);
                }
            }

            var fs=  File.OpenWrite(@"c:\temp\testresults.txt");
            var wrriter = new StreamWriter(fs);
            
            wrriter.Write(sb.ToString());
            wrriter.Flush();
            fs.Flush();
            fs.Close();

            fs = File.OpenWrite(@"c:\temp\testresultObjects.txt");
            var jsonTextWriter = new JsonTextWriter(new StreamWriter(fs));
            jsonTextWriter.WriteStartObject();

            while (!jSonObjects.IsEmpty)
            {
                if (jSonObjects.TryDequeue(out object obj))
                {
                    _context.WriteLine("JsonObj", obj);
                    jsonTextWriter.WriteRaw(JsonConvert.SerializeObject(obj));

                }
            }
            jsonTextWriter.WriteEndObject();
            jsonTextWriter.Flush();
            fs.Flush();
            fs.Close();


            //_analyzeImage.MakeAnalysisRequest()
        }

        // collect return data
        private void ProcessJsData(ConcurrentQueue<string> textQueue, ConcurrentQueue<object> jsonObjects, string json)
        {
            textQueue.Enqueue(json);
            try
            {
                jsonObjects.Enqueue(JsonConvert.DeserializeObject(json));
            }
            catch (Exception ex)
            {
                _context.WriteLine("Exception was thrown [0]", ex.Message);
            }

        }
    }
}
