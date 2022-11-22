﻿using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace PictureService;

[ApiController]
[Route("pic")]
public class PictureController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SavePic(string name, [FromForm] IFormFile file)
    {
        if (file.Length > 0)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AnimalPictures", name);
        
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        
            Console.WriteLine("IN HERE");
            return Ok("File Saved");
        }
        return BadRequest("File is empty");
    }
    
    [HttpGet]
    public IActionResult GetPic(string name)
    {
        string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "AnimalPictures"), "*");

        string path = "";
        foreach (var file in files)
        {
            if (file.Contains(name))
            {
                path = file;
            }
        }
        
        if (path == "")
        {
            return NotFound();
        }
        
        if (System.IO.File.Exists(path))
        {
            var physicalFile  = PhysicalFile(path, "image/jpeg");
            
            return physicalFile;
        }
        return NotFound();
    }
}