using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi1.Models;
using SourceAFIS.Simple;
using WebApi1.Utilities;
using System.Windows.Media.Imaging;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace WebApi1.Controllers
{
    public class PersonsController : ApiController
    {
        static AfisEngine Afis;
        List<User> users = new List<User>();

        public IEnumerable<User> GetAllProducts()
        {
            DC dc = new DC();
            users=dc.GetPersons();
            return users;
        }

        public IHttpActionResult GetPerson(int id)
        {
            DC dc = new DC();
            users = dc.GetPersons();
            var user = users.FirstOrDefault((p) => Convert.ToInt32(p.id) == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("api/upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
            {
                return this.BadRequest("Missing file");
            }

            byte[] payload = await fileContents.ReadAsByteArrayAsync();

            BitmapImage fingerImage = Converters.ByteToBitmapImage(payload);
            /*BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(fingerImage));

            using (var fileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~") + "images\\temp\\", System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }*/
            //DC dc = new DC();
            try
            {
                MyPerson person = CloudAFIS.GetTemplateObject(fingerImage);
                //int result = CloudAFIS.CheckPerson(person);
                DC db = new DC();
                int result = CloudAFIS.CheckPerson(person);
                if(result>0)
                {
                    users.Add(db.GetPerson(result));
                    return this.Ok(users);
                }
                else
                {
                    return this.NotFound();
                }
               
                /*if (result >= 0)
                {
                    return this.Ok("Person Found");
                }
                else
                {
                    return this.Ok("Person not found!!!!!");
                }*/
            }
            catch(Exception ex)
            {
                return this.Ok(ex.Message);
            }
           // return this.Ok(string.Join<User>(",", users.ToArray()));


               /* return this.Ok(new

            /*if (result >= 0)
            {
                DC db = new DC();
                //User user = db.GetPerson(result);
                users.Add(db.GetPerson(result));
                return this.Ok(new
                {
                    Result = "file uploaded successfully",
                });
            }
            else
            {
                return users;
            }*/
               /*  MessageBox.Show("Person Found!" + "\n" + "Name: " + u.name
                     + "\n" + "Gender: " + u.gender + "\n");*/
               //  }

            // TODO: do something with the payload.
            // note that this method is reading the uploaded file in memory
            // which might not be optimal for large files. If you just want to
            // save the file to disk or stream it to another system over HTTP
            // you should work directly with the fileContents.ReadAsStreamAsync() stream

            /* return this.Ok(new
         {
             Result = "file uploaded successfully",
         });*/
        }


        [HttpPost]
        [Route("api/upload/uploadprofile")]
        public async Task<IHttpActionResult> UploadProfile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
            {
                return this.BadRequest("Missing file");
            }

            byte[] payload = await fileContents.ReadAsByteArrayAsync();

            BitmapImage fingerImage = Converters.ByteToBitmapImage(payload);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(fingerImage));
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            using (var fileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~") + @"images\profile\profile" + @timestamp + @".jpg", System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
            return this.Ok( "profile" + @timestamp + @".jpg");
           /* return this.Ok(new
            {
                Result = "C:\\Users\\Mark\\Documents\\Visual Studio 2017\\Projects\\WebApi1\\WebApi1\\images\\profile\\profile" + timestamp + ".jpg",
            });*/

        }
        [HttpPost]
        [Route("api/upload/uploadfinger")]
        public async Task<IHttpActionResult> UploadFinger()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
            {
                return this.BadRequest("Missing file");
            }

            byte[] payload = await fileContents.ReadAsByteArrayAsync();

            BitmapImage fingerImage = Converters.ByteToBitmapImage(payload);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(fingerImage));
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            using (var fileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~")+@"images\finger\finger" + @timestamp + @".jpg", System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
            return this.Ok( "finger" + @timestamp + @".jpg");
            //return this.Ok(@"C:\Users\Mark\Documents\Visual Studio 2017\Projects\WebApi1\WebApi1\images\finger\finger" + timestamp + ".jpg");

        }

        [HttpPost]
        [Route("api/upload/uploadtemplate")]
        public async Task<IHttpActionResult> UploadTemplate(string name,string gender,string dob, string plocation, string flocation)
        {



            if (!Request.Content.IsMimeMultipartContent())
            {
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
            {
                return this.BadRequest("Missing file");
            }

            byte[] payload = await fileContents.ReadAsByteArrayAsync();

            /*BitmapImage fingerImage = Converters.ByteToBitmapImage(payload);
             BitmapEncoder encoder = new PngBitmapEncoder();
             encoder.Frames.Add(BitmapFrame.Create(fingerImage));

             using (var fileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~") + @"\images\finger\finger" + @timestamp + @".jpg", System.IO.FileMode.Create))
             {
                 encoder.Save(fileStream);
             }*/

            flocation=flocation.Replace("{","");
            flocation = flocation.Replace("}", "");
            flocation = flocation.Replace(@"\", "");
            flocation = flocation.Replace('"', ' ').Trim();

            plocation = plocation.Replace("{", "");
            plocation = plocation.Replace("}", "");
            plocation = plocation.Replace(@"\", "");
            plocation = plocation.Replace('"', ' ').Trim();
            Bitmap ImageFinger = (Bitmap)Image.FromFile(HttpContext.Current.Server.MapPath("~") + "images\\finger\\"+flocation, true);
            // MyPerson person = CloudAFIS.GetTemplateObject(Converters.ByteToBitmapImage(ImageFinger));
            // MyPerson person = CloudAFIS.GetTemplateObject(Converters.Bitmap2BitmapImage(ImageFinger));
            //  MyPerson person = CloudAFIS.GetTemplateObject(FingerImage);
            // string person = cloud.GetTemplate(Converters.ByteToBitmapImage(ImageFinger));

            Afis = new AfisEngine();
            Converters convert = new Converters();
              MyFinger fp = new MyFinger();
              fp.AsBitmap =ImageFinger;
               MyPerson person = new MyPerson();
               //person.Name = textName.Text;
             // Add fingerprint to the person
              person.Fingerprints.Add(fp);
               Afis.Extract(person);

             DC db = new DC();
             bool status=db.CreateUser(plocation,name,gender,dob, flocation, person);
               if(status)
               {
                   return this.Ok("Data Insert Successful!");
                  // MessageBox.Show("Data Insert Successful!");
               }
               else
               {
                   return this.Ok(" Data Insert Failed!");
               }



            //return this.Ok(HttpContext.Current.Server.MapPath("~") + "images\\finger\\" + flocation);
            /* return this.Ok(new
             {
                 Result = "File Uploaded Successfully:"+name+" "+gender+" "+dob+" "+payload.Length+"\n\n "+plocation+ "\n\n "+flocation,
             });*/

        }

        //[HttpPost]
        public IHttpActionResult PostPersonByFinger(string myarray)
        {
            //DC dc = new DC();
            // users = dc.GetPersons();
           // List<string> s = new List<string>();
            //s.Add(myarray);
            try
            {
                return Ok("DGDGDGDG");
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
           
            /*byte[] decBytes4 = HttpServerUtility.UrlTokenDecode(finger);
            BitmapImage fingerImage = Converters.ByteToBitmapImage(decBytes4);
            DC dc = new DC();
            MyPerson person = CloudAFIS.GetTemplateObject(fingerImage);
            int result = CloudAFIS.CheckPerson(person);
            if (result >= 0)
            {
                DC db = new DC();
                //User user = db.GetPerson(result);
                users.Add(db.GetPerson(result));
                return (users);
                /*  MessageBox.Show("Person Found!" + "\n" + "Name: " + u.name
                      + "\n" + "Gender: " + u.gender + "\n");*/
            //  }

            //return users;*/

            // var user = users.FirstOrDefault((p) => Convert.ToInt32(p.id) == id);


        }
    }
}
