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

namespace WebApi1.Controllers
{
    public class PersonsController : ApiController
    {
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
