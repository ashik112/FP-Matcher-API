using Newtonsoft.Json;
using SourceAFIS.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WebApi1.Models;

namespace WebApi1.Utilities
{
    public class Converters
    {
        // Byte to ImageSource Converter
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;
            BitmapImage imgSrc2 = biImg as BitmapImage;

            return imgSrc;
        }
        public static BitmapImage ByteToBitmapImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            // ImageSource imgSrc = biImg as ImageSource;
            BitmapImage imgSrc = biImg as BitmapImage;

            return imgSrc;
        }


        //Serialize Object using JsonConvert//
        public static string ObjectSerializer(Person person1)
        {
            MyPerson person = (MyPerson)person1;
            string s = JsonConvert.SerializeObject(person);
            Trace.WriteLine("Serialize Person: " + s);
            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllText(@"WriteText.txt", JsonConvert.SerializeObject(person));
            return s;
        }

        //Deserialize Object using JsonConvert//
        public static MyPerson ObjectDeserializer(string s)
        {
            try
            {
                MyPerson person = JsonConvert.DeserializeObject<MyPerson>(s);
                return person;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                MyPerson person = null;
                return person;
            }


        }

        // 1. Convert Object to byte //
        public static byte[] ObjectToByteArray(Person obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // 2. Convert a byte array to an Object //
        public static Person ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Person obj = (Person)binForm.Deserialize(memStream);
            return obj;
        }



        /*    private static BitmapImage LoadImage(byte[] imageData)
      {
          if (imageData == null || imageData.Length == 0) return null;
          var image = new BitmapImage();
          using (var mem = new MemoryStream(imageData))
          {
              mem.Position = 0;
              image.BeginInit();
              image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
              image.CacheOption = BitmapCacheOption.OnLoad;
              image.UriSource = null;
              image.StreamSource = mem;
              image.EndInit();
          }
          image.Freeze();
          return image;
      }*/
    }

}