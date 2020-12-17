using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace knowledgebuilderapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        [HttpPost]
        // [Authorize]
        public async Task<IActionResult> UploadPhotos(ICollection<IFormFile> files)
        {
            if (Request.Form.Files.Count <= 0)
                return BadRequest("No Files");

            //String usrName = String.Empty;
            //try
            //{
            //    usrName = HIHAPIUtility.GetUserID(this);
            //    if (String.IsNullOrEmpty(usrName))
            //        throw new UnauthorizedAccessException();
            //}
            //catch
            //{
            //    throw new UnauthorizedAccessException();
            //}

            var jsonresults = new List<ImageFileUploadResult>();
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    var filename1 = file.FileName;
                    var idx1 = filename1.LastIndexOf('.');
                    var fileext = filename1.Substring(idx1);
                    var newfilename = Guid.NewGuid().ToString("N") + fileext;

                    using (var fileStream = new FileStream(Path.Combine(Startup.UploadFolder, newfilename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    jsonresults.Add(new ImageFileUploadResult
                    {
                        name = filename1,
                        type = fileext == ".png" ? "image/png" : "image/jpeg",
                        size = (int)file.Length,
                        progress = "1.0",
                        url = "/" + Startup.UploadFolderName + "/" + newfilename,
                        thumbnail_url = "/" + Startup.UploadFolderName + "/" + newfilename,
                        delete_url = "/api/ImageUpload/" + newfilename,
                        delete_type = "DELETE",
                    });
                }
            }
            if (Request.Form.Files.Count > 0)
            {
                foreach (var file in Request.Form.Files)
                {
                    var filename1 = file.FileName;
                    var idx1 = filename1.LastIndexOf('.');
                    var fileext = filename1.Substring(idx1);
                    var newfilename = Guid.NewGuid().ToString("N") + fileext;

                    using (var fileStream = new FileStream(Path.Combine(Startup.UploadFolder, newfilename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    jsonresults.Add(new ImageFileUploadResult
                    {
                        name = filename1,
                        type = fileext == ".png" ? "image/png" : "image/jpeg",
                        size = (int)file.Length,
                        progress = "1.0",
                        url = "/" + Startup.UploadFolderName + "/" + newfilename,
                        thumbnail_url = "/" + Startup.UploadFolderName + "/" + newfilename,
                        delete_url = "/api/ImageUpload/" + newfilename,
                        delete_type = "DELETE",
                    });
                }
            }

            if (jsonresults.Count <= 0)
            {
                return Problem();
            }
            else
            {
                return new JsonResult(jsonresults.ToArray());
            }
        }

        [HttpDelete("{strfile}")]
        // [Authorize]
        public IActionResult DeleteUploadedFile(String strfile)
        {
            var fileFullPath = Path.Combine(Startup.UploadFolder, strfile);
            var filename = Path.GetFileNameWithoutExtension(fileFullPath);
            var fileext = Path.GetExtension(fileFullPath);

            try
            {
                // File
                if (System.IO.File.Exists(fileFullPath))
                {
                    System.IO.File.Delete(fileFullPath);
                }
            }
            catch (Exception exp)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(exp.Message);
#endif

                return BadRequest(exp.Message);
            }

            return Ok();
        }
    }
}