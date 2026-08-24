using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PurviEnterprises.Models;
using PurviEnterprises.Services;

namespace PurviEnterprises.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly EnquiryEmailService _emailService;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, EnquiryEmailService emailService)
    {
        _logger = logger;
        _environment = environment;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About() => View();

    public IActionResult Services() => View();

    public IActionResult Projects() => View();

    [HttpGet]
    public IActionResult Contact(string? service)
    {
        return View(new EnquiryViewModel { Service = service ?? string.Empty });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(EnquiryViewModel enquiry)
    {
        if (!ModelState.IsValid)
        {
            return View(enquiry);
        }

        var inboxDirectory = Path.Combine(_environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(inboxDirectory);
        var inboxPath = Path.Combine(inboxDirectory, "enquiries.json");
        var enquiries = new List<EnquiryViewModel>();

        if (System.IO.File.Exists(inboxPath))
        {
            await using var readStream = System.IO.File.OpenRead(inboxPath);
            enquiries = await JsonSerializer.DeserializeAsync<List<EnquiryViewModel>>(readStream) ?? enquiries;
        }

        enquiry.SubmittedAt = DateTime.UtcNow;
        enquiries.Add(enquiry);
        await System.IO.File.WriteAllTextAsync(inboxPath, JsonSerializer.Serialize(enquiries, new JsonSerializerOptions { WriteIndented = true }));
        await _emailService.NotifyOwnerAsync(enquiry);

        TempData["EnquirySubmitted"] = "Thank you. We will contact you shortly.";
        var whatsappMessage = $"New enquiry from {enquiry.Name}\nPhone: {enquiry.Phone}\nEmail: {enquiry.Email}\nService: {enquiry.Service}\nDetails: {enquiry.Message}";
        TempData["WhatsAppLink"] = $"https://wa.me/919653270296?text={Uri.EscapeDataString(whatsappMessage)}";
        return RedirectToAction(nameof(Contact));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
