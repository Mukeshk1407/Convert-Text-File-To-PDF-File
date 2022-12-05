using Convert_Text_File_To_PDF_File.Models;
using Aspose.Pdf;
using Aspose.Pdf.Annotations;
using Aspose.Pdf.Operators;
using Aspose.Pdf.Text;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;




namespace Convert_Text_File_To_PDF_File.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }



        public IActionResult Index()
        {
            return View();
        }

        //Post - Create
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPOST()
        {
            //Project File path
            string webRootPath = _webHostEnvironment.WebRootPath;

            //Uploaded FileName
            var files = HttpContext.Request.Form.Files;

            if(files.Count == 0)
            {
                return RedirectToAction("Error");
            }

            //Uploaded FileName Extension
            var extension = Path.GetExtension(files[0].FileName);

            if(extension != ".txt")
            {
                return RedirectToAction("Privacy");
            }
            //Copy the File Source to Current Project
            using (var filesstream = new FileStream(Path.Combine(webRootPath, files[0].FileName), FileMode.Create))
            {
                files[0].CopyTo(filesstream);
            }


            string FilePath = Path.Combine(webRootPath, files[0].FileName);

            var fileName = new FileInfo(FilePath);

            string[] Lines = System.IO.File.ReadAllLines(FilePath);

            Document document = new Document();

            // Add page
            Page page = document.Pages.Add();

            // Add text to Line by Line
            foreach (var item in Lines)
            {
                TextFragment textFragment = new TextFragment(item);
                textFragment.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Justify;
                page.Paragraphs.Add(textFragment);
            }
            // Save PDF 
            document.Save(@"D:\Document.pdf");

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }


    }
}