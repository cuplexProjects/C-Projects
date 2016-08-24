using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public sealed class RandomImage
    {
        public int ImageRef { get; set; }
        public string FileName { get; set; }
        public string ImageId { get; set; }
        private RandomImage() { }

        public static void UpdateDB(string[] fileNameList)
        {
            using (var db = CLinq.DataContext.Create())
            {
                var randomImageList = db.RandomImages.ToList();
                foreach (string fileName in fileNameList)
                {
                    if (!randomImageList.Any(img => img.FileName == fileName))
                    {
                        CLinq.RandomImage randomImage = new CuplexLib.Linq.RandomImage();
                        randomImage.FileName = fileName;
                        randomImage.ImageId = "";

                        db.RandomImages.InsertOnSubmit(randomImage);
                        db.SubmitChanges();

                        randomImage.ImageId = CreateShortHash(randomImage.ImageRef, fileName);
                    }
                }
                db.SubmitChanges();
            }
        }
        public static void UpdateImageId(string filename, string imageId)
        {
            using (var db = CLinq.DataContext.Create())
            {
                var imageQuery =
                from ri in db.RandomImages
                where ri.FileName.ToLower() == filename.ToLower()
                select ri;

                var randImage = imageQuery.Take(1).SingleOrDefault();

                if (randImage != null && !db.RandomImages.Any(x => x.ImageId == imageId))
                {
                    randImage.ImageId = imageId;
                    db.SubmitChanges();
                }
            }
        }
        //Create 8 char Base 62 hash (218340105584896 unique combinations)
        private static string CreateShortHash(int id, string fileName)
        {
            const string LookupTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";  //62 chars
            string strHash = "";
            byte[] buffer = BitConverter.GetBytes(int.MaxValue - id); //4 byte
            byte[] buffer2 = new byte[4];
            
            for (int i = 0; i < fileName.Length; i++)
                buffer2[i % 4] = (byte)((buffer2[i % 4] + fileName[i]) % 256);
     
            for (int i = 0; i < 4; i++)        
                strHash += LookupTable[buffer[i] % 62];
           
            for (int i = 0; i < 4; i++)           
                strHash += LookupTable[buffer2[i] % 62];          

            return strHash;
        }
        public static List<RandomImage> GetRandomImageList()
        {
            using (var db = CLinq.DataContext.Create())
            {
                var randomImageQuery =
                from ri in db.RandomImages
                select new RandomImage
                {
                    FileName = ri.FileName,
                    ImageId = ri.ImageId,
                    ImageRef = ri.ImageRef
                };

                return randomImageQuery.ToList();
            }
        }
    }    
}
