using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Chnage.Repository;
using Chnage.Controllers;

namespace Chnage.Controllers
{
    public class FilesController : Controller
    {
        private readonly MyECODBContext _context;

        public FilesController(MyECODBContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["UpdateWebAppLogs"] = _context.UpdateWebAppLogs
                .OrderByDescending(o => o.dtUploadDate)
                .ToList();
            base.OnActionExecuting(context);
        }
        // GET: UpdateWebAppArea/Files
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadResult(List<IFormFile> files)
        {
            return View(files);
        }

        // GET: UpdateWebAppArea/Files/Create
        public IActionResult Create(string userEmail)
        {
            using (AdminsController ac = new AdminsController(_context))
            {
                ViewData["AdminEmails"] = ac.GetAdminEmailsList();
            }

            if ((ViewData["AdminEmails"] as List<string>).Contains((userEmail)))
            {
                ViewData["email"] = userEmail;
                LogDetails();
                return View();
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: UpdateWebAppArea/Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(209715200)] //limit of the file size is 200 MB
        public ActionResult Create(IFormFile upload, string version, string reason, string email)
        {
            try
            {
                ViewData["email"] = email;

                if (!Regex.IsMatch(version, @"^[\d]+.[\d]+.[\d]+.[\d]+$"))
                {
                    ViewData["errorMsg"] += @"Version must match the following pattern:^[\d]+.[\d]+.[\d]+.[\d]+$.";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(reason))
                {
                    ViewData["errorMsg"] += "Must supply a reason.";
                    return View();
                }

                if (_context.UpdateWebAppLogs.Select(u => u.sWebAppversion == version).FirstOrDefault())
                {
                    ViewData["errorMsg"] = "Version already exists. Enter a different version number.";
                    return View();
                }

                if (ModelState.IsValid)
                {
                    if (upload != null && upload.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = @"D:\AutoUpdateATWA\RunMyECO.zip";
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            upload.CopyTo(stream);
                        }

                        if (System.IO.File.Exists(filePath))
                        {
                            Models.UpdateWebAppLogs logEntry = new Models.UpdateWebAppLogs();

                            logEntry.dtUploadDate = DateTime.UtcNow;
                            logEntry.sUserEmail = email;
                            logEntry.sWebAppversion = version;
                            logEntry.sReason = reason;

                            _context.UpdateWebAppLogs.Add(logEntry);
                            _context.SaveChanges();
                        }
                        return RedirectToAction("LogDetails");
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(upload);
        }

        public IActionResult FolderContent(string folderPath)
        {
            DirectoryInfo d = new DirectoryInfo(folderPath);
            //DirectoryInfo d = new DirectoryInfo(@"C:\Temp");
            FileInfo[] files = d.GetFiles("*", SearchOption.TopDirectoryOnly);
            return View(files);
        }

        public ActionResult LogDetails()
        {
            var filePath = @"D:\AutoUpdateATWA\runOutput.txt"; //server side
            //var filePath = @"C:\Temp\runOutput.txt";
            if (System.IO.File.Exists(filePath))
            {
                var fileLines = System.IO.File.ReadLines(filePath);
                List<string> fileContent = new List<string>();
                foreach (var line in fileLines)
                {
                    fileContent.Add(line);
                }
                ViewData["Log"] = fileContent;
                return View();
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<List<Models.UpdateWebAppLogs>> GetUpdateLogs()
        {
            return await _context.UpdateWebAppLogs.ToListAsync();
        }
    }
}