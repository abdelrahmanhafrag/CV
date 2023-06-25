using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATSWebAppV2.Data;
using ATSWebAppV2.Models;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;
using PdfSharpCore.Drawing.Layout;


namespace ATSWebAppV2.Controllers
{
    public class CVController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CVController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CV
        public async Task<IActionResult> Index()
        {
              return _context.CV != null ? 
                          View(await _context.CV.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CV'  is null.");
        }

        // GET: CV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CV == null)
            {
                return NotFound();
            }

            var cV = await _context.CV
                .FirstOrDefaultAsync(m => m.id == id);
            if (cV == null)
            {
                return NotFound();
            }

            return View(cV);
        }

        // GET: CV/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,FullName,Email,Phone,Address,Summary,WorkExperience,Education,HobbiesAndInterests")] CV cV)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cV);
                await _context.SaveChangesAsync();

                // Generate the CV PDF and save it to the server
                GeneratePdfCV(cV);

                // Send the CV PDF to HR (e.g., as an email attachment)

                return RedirectToAction(nameof(Index));
            }

            return View(cV);
        }

        private void GeneratePdfCV(CV cv)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Create a new PDF document
                PdfDocument document = new PdfDocument();

                // Add a page to the document
                PdfPage page = document.AddPage();

                // Create a graphics object for drawing on the page
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Draw the CV content on the page
                XTextFormatter textFormatter = new XTextFormatter(gfx);

                // Define fonts for the title and other information
                XFont fontTitle = new XFont("Times New Roman", 18, XFontStyle.Italic);
                XFont fontInfo = new XFont("Arial", 10, XFontStyle.Regular);

                // Calculate the height of the title and info lines
                XSize titleSize = gfx.MeasureString($"{cv.FullName}", fontTitle);
                XSize infoSize = gfx.MeasureString($"{cv.Address}\n{cv.Phone}\n{cv.Email}", fontInfo);
                double lineHeight = fontInfo.Height * 1.2; // Adjust the line height as needed

                // Calculate the y-coordinate for the title and info lines
                double titleY = (page.Height - (titleSize.Height + infoSize.Height + lineHeight)) / 2;
                double infoY = titleY + titleSize.Height + lineHeight;

                // Draw the title (full name)
                XRect titleRect = new XRect(0, 40, page.Width, titleSize.Height);
                XSize titleTextSize = gfx.MeasureString($"{cv.FullName}", fontTitle);
                double titleX = (page.Width - titleTextSize.Width) / 2;
                titleRect.X = titleX;
                textFormatter.DrawString($"{cv.FullName}", fontTitle, XBrushes.Black, titleRect, XStringFormats.TopLeft);

                // Draw the info lines (address, phone number, email)
                XRect infoRect = new XRect(0, 80, page.Width, infoSize.Height);
                XSize infoTextSize = gfx.MeasureString($"{cv.Address}\n{cv.Phone}\n{cv.Email}", fontInfo);
                double infoX = (page.Width - infoTextSize.Width) / 2;
                infoRect.X = infoX;
                textFormatter.Alignment = XParagraphAlignment.Left; // Set alignment to Left
                textFormatter.DrawString($"{cv.Address}\n{cv.Phone} {cv.Email}", fontInfo, XBrushes.Black, infoRect, XStringFormats.TopLeft);

                // Draw a line to separate the header from the rest of the page
                double lineY = infoRect.Bottom + lineHeight / 2;
                gfx.DrawLine(XPens.Black, 40, lineY, page.Width - 40, lineY);

                // Define the font and size for the main body sections
                XFont fontBody = new XFont("Helvetica", 9);
                XFont fontBodyTitle = new XFont("Helvetica", 12);

                // Calculate the height of each main body section
                XSize summarySize = gfx.MeasureString($"{cv.Summary}", fontBody);
                XSize workExperienceSize = gfx.MeasureString($"{cv.WorkExperience}", fontBody);
                XSize educationSize = gfx.MeasureString($"{cv.Education}", fontBody);
                XSize hobbiesAndInterestsSize = gfx.MeasureString($"{cv.HobbiesAndInterests}", fontBody);

                // Calculate the y-coordinate for each main body section
                double summaryY = lineY + lineHeight;
                double workExperienceY = summaryY + summarySize.Height + lineHeight;
                double educationY = workExperienceY + workExperienceSize.Height + lineHeight;
                double hobbiesAndInterestsY = educationY + educationSize.Height + lineHeight;

                // Draw the main body sections

                XRect professionalSummaryRect = new XRect(40, summaryY, page.Width - 80, summarySize.Height);
                textFormatter.DrawString($"Professional Summary:", fontBodyTitle, XBrushes.Black, professionalSummaryRect, XStringFormats.TopLeft);

                XRect summaryRect = new XRect(80, professionalSummaryRect.Bottom + 20, page.Width - 80, summarySize.Height);
                textFormatter.DrawString($"{cv.Summary}", fontBody, XBrushes.Gray, summaryRect, XStringFormats.TopLeft);

                XRect workHistoryRect = new XRect(40, summaryRect.Bottom + 20, page.Width - 80, workExperienceSize.Height);
                textFormatter.DrawString($"Work History:", fontBodyTitle, XBrushes.Black, workHistoryRect, XStringFormats.TopLeft);

                XRect workExperienceRect = new XRect(80, workHistoryRect.Bottom + 20, page.Width - 80, workExperienceSize.Height);
                textFormatter.DrawString($"{cv.WorkExperience}", fontBody, XBrushes.Gray, workExperienceRect, XStringFormats.TopLeft);

                XRect educationHistoryRect = new XRect(40, workExperienceRect.Bottom + 20, page.Width - 80, educationSize.Height);
                textFormatter.DrawString($"Education History:", fontBodyTitle, XBrushes.Black, educationHistoryRect, XStringFormats.TopLeft);

                XRect educationRect = new XRect(80, educationHistoryRect.Bottom + 20, page.Width - 80, educationSize.Height);
                textFormatter.DrawString($"{cv.Education}", fontBody, XBrushes.Gray, educationRect, XStringFormats.TopLeft);

                XRect hobbiesAndInterestsRect = new XRect(40, educationRect.Bottom + 20, page.Width - 80, hobbiesAndInterestsSize.Height);
                textFormatter.DrawString($"Hobbies and Interests:", fontBodyTitle, XBrushes.Black, hobbiesAndInterestsRect, XStringFormats.TopLeft);

                XRect hobbiesRect = new XRect(80, hobbiesAndInterestsRect.Bottom + 20, page.Width - 80, hobbiesAndInterestsSize.Height);
                textFormatter.DrawString($"{cv.HobbiesAndInterests}", fontBody, XBrushes.Gray, hobbiesRect, XStringFormats.TopLeft);

                //rect = new XRect(40, 80, page.Width - 80, page.Height - 80);
                //textFormatter.DrawString("Summary: " + cv.Summary, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                //rect = new XRect(40, 120, page.Width - 80, page.Height - 80);
                //textFormatter.DrawString("Address: " + cv.Address, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                //rect = new XRect(40, 160, page.Width - 80, page.Height - 80);
                //textFormatter.DrawString("Email: " + cv.Email, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                //rect = new XRect(40, 200, page.Width - 80, page.Height - 80);
                //textFormatter.DrawString("Work Experience: " + cv.WorkExperience, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                //rect = new XRect(40, 240, page.Width - 80, page.Height - 80);
                //textFormatter.DrawString("Education: " + cv.Education, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                //rect = new XRect(40, 280, page.Width - 80, page.Height - 80);
                //textFormatter.DrawString("Hobbies and Interest: " + cv.HobbiesAndInterests, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                // Add more content here as needed


                // Save the PDF to the memory stream
                document.Save(stream, false);

                // Get the bytes of the PDF from the memory stream
                byte[] pdfBytes = stream.ToArray();

                // Save the PDF file to the server
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), $" {cv.FullName} | CV.pdf");
                System.IO.File.WriteAllBytes(filePath, pdfBytes);
            }
        }


        // GET: CV/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CV == null)
            {
                return NotFound();
            }

            var cV = await _context.CV.FindAsync(id);
            if (cV == null)
            {
                return NotFound();
            }
            return View(cV);
        }

        // POST: CV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,FullName,Email,Phone,Address,Summary,WorkExperience,Education,HobbiesAndInterests")] CV cV)
        {
            if (id != cV.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cV);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CVExists(cV.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cV);
        }

        // GET: CV/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CV == null)
            {
                return NotFound();
            }

            var cV = await _context.CV
                .FirstOrDefaultAsync(m => m.id == id);
            if (cV == null)
            {
                return NotFound();
            }

            return View(cV);
        }

        // POST: CV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CV == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CV'  is null.");
            }
            var cV = await _context.CV.FindAsync(id);
            if (cV != null)
            {
                _context.CV.Remove(cV);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CVExists(int id)
        {
          return (_context.CV?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
