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
using Newtonsoft.Json;

namespace WebApi1.Controllers
{
    public class PersonsController : ApiController
    {
        static AfisEngine Afis;
        List<User> users = new List<User>();
        User user = new User();

        /*
         * Get All persons
         * 
         */
        public IEnumerable<User> GetAllPersons()
        {
            DC dc = new DC();
            users=dc.GetPersons();
            return users;
        }

        public string CheckAgent(string username, string password)
        {
           DC dc = new DC();
           bool check = dc.CheckAgent(username,password);
           // return username + " " + password;
           if(check)
            {
                return "true";
            }
           else
            {
                return "false";
            }
        }

        /*
         * Get Person using Id number
         */
        public IHttpActionResult GetPerson(int id)
        {
            DC dc = new DC();
            User user = dc.GetPerson(id);
            //users = dc.GetPersons();
            //var user = users.FirstOrDefault((p) => Convert.ToInt32(p.id) == id);
            return Ok(user);// JsonConvert.SerializeObject(user);
            /*DC dc = new DC();
            users = dc.GetPersons();
            var user = users.FirstOrDefault((p) => Convert.ToInt32(p.id) == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
            */
        }




        /*
         * Check If person exists in Data using Finger Print
         */
        [HttpPost]
        [Route("api/check")]
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
           
            try
            {
                MyPerson person = CloudAFIS.GetTemplateObject(fingerImage);
              
                DC db = new DC();
                int result = CloudAFIS.CheckPerson(person);
                if(result>0)
                {
                    //users.Add(db.GetPerson(result));
                    
                    return this.Ok(result);
                }
                else
                {
                    return this.NotFound();
                }
            
            }
            catch(Exception ex)
            {
                return this.Ok(ex.Message);
            }
          
        }

        /*
         * Upload Profile Picture and return location
         */
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


        /*
         * Upload Finger Print Picture and return location
         */
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

        /*
         * Upload User Name, Gender, D.O.B and Profile, FingerPrint Image
         * using the location obtained from previous APIs.
         * Then, save template in database using SourceAFIS
         */
        [HttpPost]
        [Route("api/upload/uploadperson")]
        public async Task<IHttpActionResult> UploadTemplate(string name,string gender,string dob, string plocation, string flocation)
        {
           if (!Request.Content.IsMimeMultipartContent())
            {
                //return this.StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
            {
                //return this.BadRequest("Missing file");
            }

            flocation=flocation.Replace("{","");
            flocation = flocation.Replace("}", "");
            flocation = flocation.Replace(@"\", "");
            flocation = flocation.Replace('"', ' ').Trim();

            plocation = plocation.Replace("{", "");
            plocation = plocation.Replace("}", "");
            plocation = plocation.Replace(@"\", "");
            plocation = plocation.Replace('"', ' ').Trim();


            Bitmap ImageFinger = (Bitmap)Image.FromFile(HttpContext.Current.Server.MapPath("~") + "images\\finger\\"+flocation, true);
          

            Afis = new AfisEngine();
            Converters convert = new Converters();
            MyFinger fp = new MyFinger();
            fp.AsBitmap =ImageFinger;
            MyPerson person = new MyPerson();
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

        }

        
    }
}
