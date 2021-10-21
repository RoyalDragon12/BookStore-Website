using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;


namespace WebApplication2.ModelsAction
{
    public class ModAction
    {
        //This is a brand new ModelsAction created 2021 with intention, unlike Cart & User.

        //Because Roles don't go along with Entity Framework Code First, we'll need to manually add Roles checking condition in every single ActionResult :)
        public static bool RoleChecker(object session)
        {
            if(session != null)
            {
                if(session.ToString() == "Mod")
                {
                    return true;
                }
            }
            return false;
        }

        //Upload Image for books and publishers
        public static void UploadImage(HttpPostedFileBase photo, string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string directory = path + "/Images"; //this put the Image into the Images folder

            if (photo != null && photo.ContentLength > 0)
            {
                var extension = Path.GetExtension(photo.FileName); //Get the extension aka .jpg , .png, etc
                photo.SaveAs(Path.Combine(directory, fileName + extension)); //Rename the uploaded image to whatever you please
            }
        }

        //Turns Vietnamese into ehhhh proper English ABCD ? Is used to make a model 's Code, which is used in the URL to return the view of that model.
        public static string DeAccent(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(' ', '-');
        }

        //Paging system to display. Ex: page = 1, numOfItems = 10 -> Display first 10 items. page = 2 , numOfItems = 10 -> Display items from 11 - 20.
        public static IEnumerable<T> ListByPage<T>(string page, int numOfItems, IEnumerable<T> list)
        {
            int pageNumber = 1;
            if (page != null && int.TryParse(page, out pageNumber))
            {
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
            }
            list = list.Skip((pageNumber - 1) * numOfItems).Take(numOfItems);
            return list;
        }
    }
}