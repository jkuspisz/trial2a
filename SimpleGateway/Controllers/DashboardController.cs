using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;
using SimpleGateway.Data;
using System.Reflection;

namespace SimpleGateway.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TestData2Context _testData2Context;

        public DashboardController(ApplicationDbContext context, TestData2Context testData2Context)
        {
            _context = context;
            _testData2Context = testData2Context;
        }

        public IActionResult Index()
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentRole = currentRole;

            if (currentRole == "performer")
            {
                // Show performer dashboard with proper ViewBag setup
                ViewBag.PerformerUsername = currentUser;
                ViewBag.CurrentUserRole = currentRole;
                ViewBag.IsOwnDashboard = true;
                ViewBag.CanEdit = true;
                ViewBag.CanComment = false;
                ViewBag.CanApprove = false;
                ViewBag.IsReadOnly = false;
                ViewBag.ActiveSection = "Dashboard";

                // Get performer's name for display
                var performer = _context.Users.FirstOrDefault(u => u.Username == currentUser);
                if (performer != null)
                {
                    ViewBag.DisplayName = $"{performer.FirstName} {performer.LastName}";
                    ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
                }
                else
                {
                    ViewBag.DisplayName = currentUser;
                    ViewBag.PerformerName = currentUser;
                }

                return View("PerformerDashboard");
            }
            else if (currentRole == "admin" || currentRole == "superuser")
            {
                // Redirect admin/superuser to dedicated admin dashboard
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                // Show list of performers for advisors and supervisors
                List<UserModel> assignedPerformers = new List<UserModel>();
                
                if (currentRole == "supervisor")
                {
                    // Supervisors can only see performers assigned to them
                    var currentUserId = _context.Users.FirstOrDefault(u => u.Username == currentUser)?.Id;
                    Console.WriteLine($"DEBUG: Supervisor {currentUser} has ID: {currentUserId}");
                    
                    if (currentUserId.HasValue)
                    {
                        assignedPerformers = _context.Assignments
                            .Include(a => a.Performer)
                            .Where(a => a.SupervisorId == currentUserId.Value && a.IsActive)
                            .Select(a => a.Performer)
                            .Where(p => p != null)
                            .ToList()!;
                        
                        Console.WriteLine($"DEBUG: Found {assignedPerformers.Count} assigned performers for supervisor {currentUser}");
                    }
                }
                else if (currentRole == "advisor")
                {
                    // Advisors can only see performers assigned to them
                    var currentUserId = _context.Users.FirstOrDefault(u => u.Username == currentUser)?.Id;
                    if (currentUserId.HasValue)
                    {
                        assignedPerformers = _context.Assignments
                            .Include(a => a.Performer)
                            .Where(a => a.AdvisorId == currentUserId.Value && a.IsActive)
                            .Select(a => a.Performer)
                            .Where(p => p != null)
                            .ToList()!;
                    }
                }
                
                ViewBag.Users = assignedPerformers;
                return View();
            }
        }

        // Method for supervisors/advisors to view specific performer dashboards
        public IActionResult ViewPerformerDashboard(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // If no performer specified, redirect to main index
            if (string.IsNullOrEmpty(performerUsername))
            {
                return Index();
            }

            // Check permissions - ensure user has access to this performer
            if (currentRole == "performer")
            {
                // Performers should use the regular Index method for their own dashboard
                return RedirectToAction("Index");
            }

            // For non-performers, check if they have access to this performer
            if (currentRole == "supervisor" || currentRole == "advisor")
            {
                var currentUserId = _context.Users.FirstOrDefault(u => u.Username == currentUser)?.Id;
                if (currentUserId.HasValue)
                {
                    bool hasAccess = false;
                    var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
                    
                    if (performer != null)
                    {
                        if (currentRole == "supervisor")
                        {
                            hasAccess = _context.Assignments
                                .Any(a => a.SupervisorId == currentUserId.Value && 
                                         a.PerformerId == performer.Id && 
                                         a.IsActive);
                        }
                        else if (currentRole == "advisor")
                        {
                            hasAccess = _context.Assignments
                                .Any(a => a.AdvisorId == currentUserId.Value && 
                                         a.PerformerId == performer.Id && 
                                         a.IsActive);
                        }
                    }
                    
                    if (!hasAccess)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else if (currentRole == "admin" || currentRole == "superuser")
            {
                // Admin/superuser can view any performer dashboard
            }
            else
            {
                return RedirectToAction("Index");
            }

            // Set up ViewBag for performer dashboard view
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentRole = currentRole;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "advisor" || currentRole == "supervisor");
            ViewBag.CanApprove = (currentRole == "supervisor");
            ViewBag.IsReadOnly = !(currentRole == "performer" && currentUser == performerUsername);
            ViewBag.ActiveSection = "Dashboard";

            // Get performer's name for display
            var performerUser = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performerUser != null)
            {
                ViewBag.DisplayName = $"{performerUser.FirstName} {performerUser.LastName}";
                ViewBag.PerformerName = $"{performerUser.FirstName} {performerUser.LastName}";
            }
            else
            {
                ViewBag.DisplayName = performerUsername;
                ViewBag.PerformerName = performerUsername;
            }

            return View("PerformerDashboard");
        }

        public IActionResult PerformerDetails(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            
            // PERFORMER DETAILS SPECIFIC PERMISSIONS
            // Only performers (editing their own) and admin/superuser can edit fields
            ViewBag.CanEditPerformerDetails = ((currentRole == "performer" && currentUser == performerUsername) || 
                                             currentRole == "admin" || currentRole == "superuser");
            
            ViewBag.ActiveSection = "PerformerDetails";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Get or create performer details from database
            Console.WriteLine($"DASHBOARD DEBUG: Attempting to retrieve performer details for {performerUsername}");
            var model = _context.PerformerDetails.FirstOrDefault(p => p.Username == performerUsername);
            if (model == null)
            {
                Console.WriteLine($"DASHBOARD DEBUG: No existing details found for {performerUsername}, creating new");
                // Create new performer details if not exists
                model = new PerformerDetailsModel
                {
                    Username = performerUsername
                };
            }
            else
            {
                Console.WriteLine($"DASHBOARD DEBUG: Found existing details for {performerUsername} - FirstName: {model.FirstName}, LastName: {model.LastName}, GDC: {model.GDCNumber}");
            }

            // Check total PerformerDetails count
            var totalPerformerDetails = _context.PerformerDetails.Count();
            Console.WriteLine($"DASHBOARD DEBUG: Total PerformerDetails records in database: {totalPerformerDetails}");

            return View("Performer/PerformerDetails", model);
        }

        [HttpPost]
        public IActionResult PerformerDetails(PerformerDetailsModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            Console.WriteLine($"DASHBOARD DEBUG: POST request received for PerformerDetails, user: {currentUser}, model.Username: {model?.Username}");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine($"DASHBOARD DEBUG: No current user in session, redirecting to login");
                return RedirectToAction("Login", "Account");
            }

            if (model == null)
            {
                Console.WriteLine($"DASHBOARD DEBUG: Model is null");
                return RedirectToAction("PerformerDetails", new { performerUsername = currentUser });
            }

            // PERFORMER DETAILS PERMISSION CHECK
            var currentRole = HttpContext.Session.GetString("role");
            var canEditPerformerDetails = ((currentRole == "performer" && model.Username == currentUser) || 
                                         currentRole == "admin" || currentRole == "superuser");
            
            if (!canEditPerformerDetails)
            {
                Console.WriteLine($"DASHBOARD DEBUG: Permission denied - {currentUser} ({currentRole}) cannot edit {model.Username}'s performer details");
                return RedirectToAction("Index");
            }
            
            Console.WriteLine($"DASHBOARD DEBUG: Permission granted - {currentUser} ({currentRole}) can edit performer details for {model.Username}");

            Console.WriteLine($"DASHBOARD DEBUG: ModelState.IsValid = {ModelState.IsValid}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"DASHBOARD DEBUG: ModelState errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"DASHBOARD DEBUG: - {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if performer details already exists
                    var existingDetails = _context.PerformerDetails.FirstOrDefault(p => p.Username == model.Username);
                    
                    if (existingDetails != null)
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Updating existing performer details for {model.Username}");
                        Console.WriteLine($"DASHBOARD DEBUG: Before update - FirstName: '{existingDetails.FirstName}' -> '{model.FirstName}'");
                        // Update existing record
                        existingDetails.FirstName = model.FirstName;
                        existingDetails.LastName = model.LastName;
                        existingDetails.GDCNumber = model.GDCNumber;
                        existingDetails.Email = model.Email;
                        existingDetails.ContactNumber = model.ContactNumber;
                        existingDetails.SupportingDentist = model.SupportingDentist;
                        existingDetails.SupportingDentistContactNumber = model.SupportingDentistContactNumber;
                        existingDetails.PracticeAddress = model.PracticeAddress;
                        existingDetails.PracticePostCode = model.PracticePostCode;
                        existingDetails.UniversityCountryOfQualification = model.UniversityCountryOfQualification;
                        existingDetails.DateOfDentalQualification = model.DateOfDentalQualification;
                        existingDetails.DateOfUKRegistration = model.DateOfUKRegistration;
                        existingDetails.IsCompleted = model.IsCompleted;
                        
                        _context.PerformerDetails.Update(existingDetails);
                        Console.WriteLine($"DASHBOARD DEBUG: After update - FirstName: '{existingDetails.FirstName}'");
                    }
                    else
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Creating new performer details for {model.Username}");
                        Console.WriteLine($"DASHBOARD DEBUG: New record - FirstName: '{model.FirstName}', LastName: '{model.LastName}'");
                        // Add new record
                        _context.PerformerDetails.Add(model);
                    }
                    
                    var savedRows = _context.SaveChanges();
                    Console.WriteLine($"DASHBOARD DEBUG: SaveChanges() affected {savedRows} rows for {model.Username}");
                    
                    if (savedRows > 0)
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Save successful! Redirecting with success message");
                        TempData["SuccessMessage"] = "Details saved successfully!";
                        return RedirectToAction("PerformerDetails", new { performerUsername = model.Username });
                    }
                    else
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Save failed - no rows affected");
                        TempData["ErrorMessage"] = "Save failed - no changes were made to the database.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Exception during save: {ex.Message}");
                    Console.WriteLine($"DASHBOARD DEBUG: Stack trace: {ex.StackTrace}");
                    TempData["ErrorMessage"] = $"Error saving details: {ex.Message}";
                }
            }
            else
            {
                Console.WriteLine($"DASHBOARD DEBUG: ModelState invalid, redisplaying form");
                TempData["ErrorMessage"] = "Please correct the validation errors and try again.";
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PerformerUsername = model.Username;
            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentUserRole = HttpContext.Session.GetString("role");
            ViewBag.IsOwnDashboard = (currentUser == model.Username);
            ViewBag.ActiveSection = "PerformerDetails";
            
            var performer2 = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (performer2 != null)
            {
                ViewBag.PerformerName = $"{performer2.FirstName} {performer2.LastName}";
            }
            
            return View("Performer/PerformerDetails", model);
        }

        public IActionResult Uploads(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            ViewBag.ActiveSection = "Uploads";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Get uploaded files for this performer
            var fileUpload = _context.FileUploads
                .Where(f => f.Username == performerUsername)
                .FirstOrDefault();

            if (fileUpload != null)
            {
                // Get file entries for this FileUpload
                var allFiles = _context.FileUploadEntries
                    .Where(f => f.FileUploadModelId == fileUpload.Id)
                    .OrderByDescending(f => f.UploadedAt)
                    .ToList();

                // Separate files by category
                ViewBag.IndemnityFiles = allFiles
                    .Where(f => f.Category == "IndemnityEvidence")
                    .ToList();
                
                ViewBag.AdditionalFiles = allFiles
                    .Where(f => f.Category == "Additional")
                    .ToList();
            }
            else
            {
                ViewBag.IndemnityFiles = new List<FileUploadEntry>();
                ViewBag.AdditionalFiles = new List<FileUploadEntry>();
            }

            return View($"Performer/Uploads");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(string performerUsername, string category, string description, bool isRequired, IFormFile uploadFile)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions - only performers can upload to their own files
            if (currentRole != "performer" || currentUser != performerUsername)
            {
                TempData["ErrorMessage"] = "You don't have permission to upload files for this performer.";
                return RedirectToAction("Uploads", new { performerUsername });
            }

            // Validate file
            if (uploadFile == null || uploadFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a file to upload.";
                return RedirectToAction("Uploads", new { performerUsername });
            }

            // Check file size (10MB limit)
            if (uploadFile.Length > 10 * 1024 * 1024)
            {
                TempData["ErrorMessage"] = "File size must be less than 10MB.";
                return RedirectToAction("Uploads", new { performerUsername });
            }

            // Validate file types
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".txt" };
            var fileExtension = Path.GetExtension(uploadFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "Only PDF, Word documents, images, and text files are allowed.";
                return RedirectToAction("Uploads", new { performerUsername });
            }

            try
            {
                // Create uploads directory if it doesn't exist
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", performerUsername);
                Directory.CreateDirectory(uploadsPath);

                // Generate unique filename
                var fileName = uploadFile.FileName;
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }

                // Get or create FileUploadModel for this performer
                var fileUploadModel = _context.FileUploads
                    .Include(f => f.UploadedFiles)
                    .FirstOrDefault(f => f.Username == performerUsername);

                if (fileUploadModel == null)
                {
                    fileUploadModel = new FileUploadModel
                    {
                        Username = performerUsername,
                        UploadedFiles = new List<FileUploadEntry>()
                    };
                    _context.FileUploads.Add(fileUploadModel);
                }

                // Create file entry
                var fileEntry = new FileUploadEntry
                {
                    Description = description,
                    FileName = fileName,
                    FileSize = uploadFile.Length,
                    ContentType = uploadFile.ContentType,
                    FilePath = Path.Combine("uploads", performerUsername, uniqueFileName),
                    IsRequired = isRequired,
                    Category = category,
                    UploadedAt = DateTime.UtcNow
                };

                fileUploadModel.UploadedFiles.Add(fileEntry);
                fileUploadModel.LastUpdated = DateTime.UtcNow;

                // Update indemnity flag if this is indemnity evidence
                if (category == "IndemnityEvidence")
                {
                    fileUploadModel.HasIndemnityEvidence = true;
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"File '{fileName}' uploaded successfully!";
                Console.WriteLine($"FILE UPLOAD SUCCESS: {fileName} uploaded for {performerUsername}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FILE UPLOAD ERROR: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while uploading the file. Please try again.";
            }

            // Redirect to appropriate page based on category
            if (category.StartsWith("CPD_"))
            {
                return RedirectToAction("CPD", new { performerUsername });
            }
            else
            {
                return RedirectToAction("Uploads", new { performerUsername });
            }
        }

        public IActionResult DownloadFile(int id)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            var fileEntry = _context.FileUploadEntries
                .FirstOrDefault(f => f.Id == id);

            if (fileEntry == null)
            {
                return NotFound("File not found.");
            }

            // Get the associated FileUpload to check permissions
            var fileUpload = _context.FileUploads
                .FirstOrDefault(f => f.Id == fileEntry.FileUploadModelId);

            // Check permissions
            var performerUsername = fileUpload?.Username;
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return Forbid("You don't have permission to download this file.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileEntry.FilePath);
            
            if (!System.IO.File.Exists(filePath))
            {
                TempData["ErrorMessage"] = "File not found on server.";
                return RedirectToAction("Uploads", new { performerUsername });
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, fileEntry.ContentType, fileEntry.FileName);
        }

        public async Task<IActionResult> DeleteFile(int id)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            var fileEntry = _context.FileUploadEntries
                .FirstOrDefault(f => f.Id == id);

            if (fileEntry == null)
            {
                return NotFound("File not found.");
            }

            // Get the associated FileUpload to check permissions
            var fileUpload = _context.FileUploads
                .FirstOrDefault(f => f.Id == fileEntry.FileUploadModelId);

            var performerUsername = fileUpload?.Username;

            // Check permissions - only performers can delete their own files
            if (currentRole != "performer" || currentUser != performerUsername)
            {
                TempData["ErrorMessage"] = "You don't have permission to delete this file.";
                return RedirectToAction("Uploads", new { performerUsername });
            }

            try
            {
                // Delete physical file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileEntry.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Remove from database
                _context.FileUploadEntries.Remove(fileEntry);

                // Update indemnity flag if this was indemnity evidence
                if (fileEntry.Category == "IndemnityEvidence")
                {
                    var remainingIndemnityFiles = _context.FileUploadEntries
                        .Where(f => f.FileUploadModelId == fileEntry.FileUploadModelId && f.Category == "IndemnityEvidence" && f.Id != id)
                        .Any();
                    
                    if (!remainingIndemnityFiles && fileUpload != null)
                    {
                        fileUpload.HasIndemnityEvidence = false;
                    }
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"File '{fileEntry.FileName}' deleted successfully.";
                Console.WriteLine($"FILE DELETE SUCCESS: {fileEntry.FileName} deleted for {performerUsername}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FILE DELETE ERROR: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the file.";
            }

            // Redirect to appropriate page based on category
            if (fileEntry.Category.StartsWith("CPD_"))
            {
                return RedirectToAction("CPD", new { performerUsername });
            }
            else
            {
                return RedirectToAction("Uploads", new { performerUsername });
            }
        }

        // TestPractice - Supervisor/Supporting Dentist Information
        public IActionResult TestPractice(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // REMOVED USER RESTRICTIONS - All users can access TestPractice now
            Console.WriteLine($"TESTPRACTICE DEBUG: {currentRole} user {currentUser} accessing TestPractice for {performerUsername}");

            // FIELD-LEVEL PERMISSIONS PATTERN: TestPractice supervisor information
            // ALL users can view, only supervisors/admin can edit
            ViewBag.CanEditSupervisorFields = (currentRole == "supervisor" || currentRole == "admin" || currentRole == "superuser");
            ViewBag.CanViewSupervisorFields = true; // All users can view

            // Set common ViewBag properties  
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (performerUsername == currentUser);
            ViewBag.CanComment = false;
            ViewBag.CanApprove = false;
            ViewBag.IsReadOnly = !ViewBag.CanEditSupervisorFields; // Readonly for non-supervisors
            ViewBag.ActiveSection = "TestPractice";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            try
            {
                // Database connection verification - SAFE METHOD (following Database Integration Pattern)
                var canConnect = _context.Database.CanConnect();
                if (!canConnect)
                {
                    Console.WriteLine("TESTPRACTICE DEBUG: Database connection issue in GET");
                    TempData["ErrorMessage"] = "Database connection issue. Please try again later.";
                    var emptyModel = new TestDataModel { Username = performerUsername };
                    return View("Performer/TestPractice", emptyModel);
                }

                Console.WriteLine($"TESTPRACTICE DEBUG: Database connection verified for GET");
                
                // Emergency schema verification - Check if ALL required fields exist (Database Integration Pattern)
                try
                {
                    Console.WriteLine("TESTPRACTICE DEBUG: Verifying ALL required fields schema exists");
                    
                    // Test if ALL required fields exist by attempting a comprehensive query
                    var testQuery = _context.TestData.Where(x => 
                        x.UKWorkExperience != null && 
                        x.LastPatientTreatment != null && 
                        x.GDCNumber != null).Count();
                    Console.WriteLine($"TESTPRACTICE DEBUG: Schema verification passed - ALL required fields exist");
                }
                catch (Exception schemaEx)
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: Schema verification failed - {schemaEx.Message}");
                    if (schemaEx.Message.Contains("relation") && schemaEx.Message.Contains("does not exist"))
                    {
                        Console.WriteLine("TESTPRACTICE DEBUG: TestData table missing - applying emergency table creation");
                        
                        // Emergency table creation for Railway PostgreSQL (Database Integration Pattern)
                        try
                        {
                            _context.Database.ExecuteSqlRaw(@"
                                CREATE TABLE IF NOT EXISTS ""TestData"" (
                                    ""Id"" integer GENERATED BY DEFAULT AS IDENTITY,
                                    ""Username"" text NOT NULL,
                                    ""CreatedDate"" timestamp with time zone NOT NULL DEFAULT NOW(),
                                    ""ModifiedDate"" timestamp with time zone,
                                    ""GDCNumber"" text,
                                    ""YearsOnPerformersList"" text,
                                    ""TrainingCoursesAttended"" text,
                                    ""CurrentSupervisionExperience"" text,
                                    ""CurrentConditionsRestrictions"" text,
                                    ""CPDCompliance"" text,
                                    ""DeclarationSigned"" boolean,
                                    ""DeclarationSignedDate"" timestamp with time zone,
                                    ""DeclarationSignedBy"" text,
                                    CONSTRAINT ""PK_TestData"" PRIMARY KEY (""Id"")
                                );
                            ");
                            Console.WriteLine("TESTPRACTICE DEBUG: Emergency TestData table creation completed WITH all required fields");
                        }
                        catch (Exception fixEx)
                        {
                            Console.WriteLine($"TESTPRACTICE DEBUG: Emergency table creation failed - {fixEx.Message}");
                        }
                    }
                    else if (schemaEx.Message.Contains("column") || schemaEx.Message.Contains("does not exist"))
                    {
                        Console.WriteLine("TESTPRACTICE DEBUG: Missing supervisor columns detected - applying emergency schema fix");
                        
                        // Emergency schema fix for Railway PostgreSQL (Database Integration Pattern)
                        try
                        {
                            _context.Database.ExecuteSqlRaw(@"
                                ALTER TABLE ""TestData"" 
                                ADD COLUMN IF NOT EXISTS ""GDCNumber"" text,
                                ADD COLUMN IF NOT EXISTS ""YearsOnPerformersList"" text,
                                ADD COLUMN IF NOT EXISTS ""TrainingCoursesAttended"" text,
                                ADD COLUMN IF NOT EXISTS ""CurrentSupervisionExperience"" text,
                                ADD COLUMN IF NOT EXISTS ""CurrentConditionsRestrictions"" text,
                                ADD COLUMN IF NOT EXISTS ""CPDCompliance"" text,
                                ADD COLUMN IF NOT EXISTS ""DeclarationSigned"" boolean,
                                ADD COLUMN IF NOT EXISTS ""DeclarationSignedDate"" timestamp with time zone,
                                ADD COLUMN IF NOT EXISTS ""DeclarationSignedBy"" text;
                            ");
                            Console.WriteLine("TESTPRACTICE DEBUG: Emergency schema fix applied successfully - ALL required fields added");
                        }
                        catch (Exception fixEx)
                        {
                            Console.WriteLine($"TESTPRACTICE DEBUG: Emergency schema fix failed - {fixEx.Message}");
                        }
                    }
                }
                
                // Try to get existing data for this performer - with column error handling
                TestDataModel? existingData = null;
                try
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: Attempting to retrieve existing data for {performerUsername}");
                    existingData = _context.TestData.FirstOrDefaultAsync(x => x.Username == performerUsername).Result;
                    Console.WriteLine($"TESTPRACTICE DEBUG: Data retrieval successful for {performerUsername}");
                }
                catch (Exception dataEx)
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: Data retrieval failed: {dataEx.Message}");
                    if (dataEx.Message.Contains("column") && dataEx.Message.Contains("does not exist"))
                    {
                        Console.WriteLine("TESTPRACTICE DEBUG: Column missing during data retrieval - applying emergency column fix");
                        
                        // Apply emergency column fix immediately
                        try
                        {
                            _context.Database.ExecuteSqlRaw(@"
                                ALTER TABLE ""TestData"" 
                                ADD COLUMN IF NOT EXISTS ""UKWorkExperience"" text NOT NULL DEFAULT '',
                                ADD COLUMN IF NOT EXISTS ""LastPatientTreatment"" text NOT NULL DEFAULT '',
                                ADD COLUMN IF NOT EXISTS ""GDCNumber"" text,
                                ADD COLUMN IF NOT EXISTS ""YearsOnPerformersList"" text,
                                ADD COLUMN IF NOT EXISTS ""TrainingCoursesAttended"" text,
                                ADD COLUMN IF NOT EXISTS ""CurrentSupervisionExperience"" text,
                                ADD COLUMN IF NOT EXISTS ""CurrentConditionsRestrictions"" text,
                                ADD COLUMN IF NOT EXISTS ""CPDCompliance"" text,
                                ADD COLUMN IF NOT EXISTS ""DeclarationSigned"" boolean,
                                ADD COLUMN IF NOT EXISTS ""DeclarationSignedDate"" timestamp with time zone,
                                ADD COLUMN IF NOT EXISTS ""DeclarationSignedBy"" text;
                            ");
                            Console.WriteLine("TESTPRACTICE DEBUG: Emergency column fix applied - retrying data retrieval");
                            
                            // Retry data retrieval after column fix
                            existingData = _context.TestData.FirstOrDefaultAsync(x => x.Username == performerUsername).Result;
                            Console.WriteLine($"TESTPRACTICE DEBUG: Data retrieval retry successful for {performerUsername}");
                        }
                        catch (Exception retryEx)
                        {
                            Console.WriteLine($"TESTPRACTICE DEBUG: Emergency column fix and retry failed: {retryEx.Message}");
                            existingData = null; // Ensure we create new model
                        }
                    }
                    else
                    {
                        Console.WriteLine($"TESTPRACTICE DEBUG: Non-column data retrieval error: {dataEx.Message}");
                        existingData = null; // Ensure we create new model
                    }
                }
                
                if (existingData != null)
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: Found existing supervisor data for {performerUsername}");
                    return View("Performer/TestPractice", existingData);
                }
                else
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: No existing supervisor data found for {performerUsername}, creating new model");
                    var model = new TestDataModel { Username = performerUsername };
                    return View("Performer/TestPractice", model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TESTPRACTICE DEBUG: Database error in GET: {ex.Message}");
                Console.WriteLine($"TESTPRACTICE DEBUG: Inner Exception: {ex.InnerException?.Message}");
                
                // Provide helpful error information
                if (ex.Message.Contains("column") || ex.Message.Contains("does not exist"))
                {
                    TempData["ErrorMessage"] = "Database schema issue detected. Contact administrator to update database structure.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Database error: {ex.Message}";
                }
                
                var model = new TestDataModel { Username = performerUsername };
                return View("Performer/TestPractice", model);
            }
        }

        [HttpPost]
        public IActionResult TestPractice(TestDataModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            if (model == null)
            {
                return RedirectToAction("TestPractice", new { performerUsername = currentUser });
            }

            // CRITICAL: Username comes from form (performer being viewed), not logged-in user
            // This allows supervisors to edit performer data without creating new rows
            Console.WriteLine($"TESTPRACTICE DEBUG: Form submitted with Username: {model.Username}, CurrentUser: {currentUser}");

            // FIELD-LEVEL PERMISSIONS: Only supervisors/admin can edit supervisor information
            var currentRole = HttpContext.Session.GetString("role");
            var canEditSupervisorFields = (currentRole == "supervisor" || currentRole == "admin" || currentRole == "superuser");
            
            if (!canEditSupervisorFields)
            {
                Console.WriteLine($"TESTPRACTICE DEBUG: Access denied - role {currentRole} cannot edit supervisor information");
                TempData["ErrorMessage"] = "Only supervisors and administrators can edit supervisor information.";
                return RedirectToAction("TestPractice", new { performerUsername = model.Username });
            }

            Console.WriteLine($"TESTPRACTICE DEBUG: {currentRole} user {currentUser} authorized to edit supervisor data for {model.Username}");

            if (ModelState.IsValid)
            {
                try
                {
                    // Database connection verification - SAFE METHOD (following Database Integration Pattern)
                    var canConnect = _context.Database.CanConnect();
                    if (!canConnect)
                    {
                        Console.WriteLine("TESTPRACTICE DEBUG: Database connection issue");
                        TempData["ErrorMessage"] = "Database connection issue. Please try again.";
                        return View("Performer/TestPractice", model);
                    }

                    Console.WriteLine($"TESTPRACTICE DEBUG: Database connection verified");
                    
                    // EMERGENCY SCHEMA VERIFICATION AND FIXING (Database Integration Pattern for Railway PostgreSQL)
                    try
                    {
                        Console.WriteLine("TESTPRACTICE POST DEBUG: Verifying supervisor field schema exists");
                        
                        // Test if supervisor fields exist by attempting a basic query
                        var testQuery = _context.TestData.Where(x => x.GDCNumber != null).Take(1).ToList();
                        Console.WriteLine("TESTPRACTICE POST DEBUG: Supervisor fields schema verification successful");
                    }
                    catch (Exception schemaEx)
                    {
                        Console.WriteLine($"TESTPRACTICE POST DEBUG: Schema verification failed - {schemaEx.Message}");
                        if (schemaEx.Message.Contains("relation") && schemaEx.Message.Contains("does not exist"))
                        {
                            Console.WriteLine("TESTPRACTICE POST DEBUG: TestData table missing - applying emergency table creation");
                            
                            try
                            {
                                _context.Database.ExecuteSqlRaw(@"
                                    CREATE TABLE IF NOT EXISTS ""TestData"" (
                                        ""Id"" integer GENERATED BY DEFAULT AS IDENTITY,
                                        ""Username"" text NOT NULL,
                                        ""CreatedDate"" timestamp with time zone NOT NULL DEFAULT NOW(),
                                        ""ModifiedDate"" timestamp with time zone,
                                        ""GDCNumber"" text,
                                        ""YearsOnPerformersList"" text,
                                        ""TrainingCoursesAttended"" text,
                                        ""CurrentSupervisionExperience"" text,
                                        ""CurrentConditionsRestrictions"" text,
                                        ""CPDCompliance"" text,
                                        ""DeclarationSigned"" boolean,
                                        ""DeclarationSignedDate"" timestamp with time zone,
                                        ""DeclarationSignedBy"" text,
                                        CONSTRAINT ""PK_TestData"" PRIMARY KEY (""Id"")
                                    );
                                ");
                                Console.WriteLine("TESTPRACTICE POST DEBUG: Emergency TestData table creation completed WITH all required fields");
                            }
                            catch (Exception fixEx)
                            {
                                Console.WriteLine($"TESTPRACTICE POST DEBUG: Emergency table creation failed - {fixEx.Message}");
                                TempData["ErrorMessage"] = "Database schema issue detected. Please try again or contact administrator.";
                                return View("Performer/TestPractice", model);
                            }
                        }
                        else if (schemaEx.Message.Contains("column") && (schemaEx.Message.Contains("does not exist") || schemaEx.Message.Contains("GDCNumber")))
                        {
                            Console.WriteLine("TESTPRACTICE POST DEBUG: Supervisor fields missing - applying emergency schema fix");
                            
                            try
                            {
                                // PostgreSQL syntax for Railway environment - ALL required fields
                                var sqlCommands = new[]
                                {
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"GDCNumber\" text",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"YearsOnPerformersList\" text",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"TrainingCoursesAttended\" text",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"CurrentSupervisionExperience\" text",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"CurrentConditionsRestrictions\" text",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"CPDCompliance\" text",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"DeclarationSigned\" boolean",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"DeclarationSignedDate\" timestamp with time zone",
                                    "ALTER TABLE \"TestData\" ADD COLUMN IF NOT EXISTS \"DeclarationSignedBy\" text"
                                };
                                
                                foreach (var sql in sqlCommands)
                                {
                                    _context.Database.ExecuteSqlRaw(sql);
                                    Console.WriteLine($"TESTPRACTICE POST DEBUG: Executed emergency SQL: {sql}");
                                }
                                
                                Console.WriteLine("TESTPRACTICE POST DEBUG: Emergency schema fix completed - ALL required fields added");
                            }
                            catch (Exception fixEx)
                            {
                                Console.WriteLine($"TESTPRACTICE POST DEBUG: Emergency schema fix failed: {fixEx.Message}");
                                TempData["ErrorMessage"] = "Database schema issue detected. Please try again or contact administrator.";
                                return View("Performer/TestPractice", model);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"TESTPRACTICE POST DEBUG: Non-schema database error: {schemaEx.Message}");
                            throw; // Re-throw if not a schema issue
                        }
                    }
                    
                    // DATABASE OPERATIONS - with additional error handling (Database Integration Pattern)
                    try
                    {
                        // CRITICAL: Delete all existing records for this user first (Database Integration Pattern)
                        var existingRecords = _context.TestData.Where(x => x.Username == model.Username).ToList();
                        
                        if (existingRecords.Any())
                        {
                            Console.WriteLine($"TESTPRACTICE DEBUG: Found {existingRecords.Count} existing records for {model.Username} - deleting all");
                            _context.TestData.RemoveRange(existingRecords);
                            
                            // Save deletions first
                            var deletedRows = _context.SaveChanges();
                            Console.WriteLine($"TESTPRACTICE DEBUG: Deleted {deletedRows} existing records");
                        }
                        
                        // Create new record with latest data
                        Console.WriteLine($"TESTPRACTICE DEBUG: Creating new record for {model.Username}");
                        
                        model.CreatedDate = DateTime.UtcNow;
                        model.ModifiedDate = null;
                        
                        _context.TestData.Add(model);
                        
                        var savedRows = _context.SaveChanges();
                        
                        if (savedRows > 0)
                        {
                            Console.WriteLine($"TESTPRACTICE DEBUG: Successfully saved {savedRows} records");
                            
                            // Get total count for verification
                            var totalRecords = _context.TestData.Count();
                            Console.WriteLine($"TESTPRACTICE DEBUG: Total TestData records in database: {totalRecords}");
                            
                            TempData["SuccessMessage"] = $"Supervisor information saved successfully! (Saved by: {currentUser})";
                            TempData["TotalRecords"] = totalRecords;
                            return RedirectToAction("TestPractice", new { performerUsername = model.Username });
                        }
                        else
                        {
                            Console.WriteLine($"TESTPRACTICE DEBUG: Save failed - no changes were made");
                            TempData["ErrorMessage"] = "Save failed - no changes were made.";
                        }
                    }
                    catch (Exception dbEx)
                    {
                        Console.WriteLine($"TESTPRACTICE POST DEBUG: Database operation failed: {dbEx.Message}");
                        Console.WriteLine($"TESTPRACTICE POST DEBUG: Database Inner Exception: {dbEx.InnerException?.Message}");
                        
                        // Check if it's still a schema issue that slipped through
                        if (dbEx.Message.Contains("column") || dbEx.Message.Contains("does not exist") || dbEx.Message.Contains("relation"))
                        {
                            Console.WriteLine("TESTPRACTICE POST DEBUG: Additional schema issue detected during database operations");
                            TempData["ErrorMessage"] = "Database schema issue detected. Contact administrator to update database structure.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = $"Database error: {dbEx.Message}";
                        }
                        
                        return View("Performer/TestPractice", model);
                    }
                }
                catch (Exception ex)
                {
                    // Enhanced error logging following Database Integration Pattern
                    Console.WriteLine($"TESTPRACTICE DEBUG: General exception: {ex.Message}");
                    Console.WriteLine($"TESTPRACTICE DEBUG: Inner Exception: {ex.InnerException?.Message}");
                    Console.WriteLine($"TESTPRACTICE DEBUG: Stack Trace: {ex.StackTrace}");
                    
                    TempData["ErrorMessage"] = $"Error saving supervisor information: {ex.Message}";
                    
                    // If it's a column/schema issue, try to help by noting it
                    if (ex.Message.Contains("column") || ex.Message.Contains("does not exist"))
                    {
                        Console.WriteLine($"TESTPRACTICE DEBUG: Schema issue detected - may need database table update");
                        TempData["ErrorMessage"] = "Database schema issue detected. Contact administrator to update database structure.";
                    }
                }
            }
            else
            {
                Console.WriteLine($"TESTPRACTICE DEBUG: ModelState is invalid");
                foreach (var modelError in ModelState)
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: ModelState Error - Key: {modelError.Key}, Errors: {string.Join(", ", modelError.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            return View("Performer/TestPractice", model);
        }

        // TestPractice2 methods - identical copy of TestPractice
        public IActionResult TestPractice2(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            
            // TESTPRACTICE2 SPECIFIC PERMISSIONS
            // Main form fields: Performers can edit their own data, supervisors/advisors/admins can view
            ViewBag.CanEditMainFields = (currentRole == "performer" && currentUser == performerUsername);
            // AdvisorComment: Only advisors and admins can edit
            ViewBag.CanEditAdvisorComment = (currentRole == "advisor" || currentRole == "admin" || currentRole == "superuser");
            
            ViewBag.ActiveSection = "TestPractice2";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Get or create test data from database with proper error handling
            Console.WriteLine($"TESTPRACTICE2 DEBUG: Attempting to retrieve test data for {performerUsername}");
            TestDataModel2? model = null;
            int totalTestData = 0;
            
            try
            {
                // Check database schema and apply migrations if needed for TestData2Context
                try
                {
                    var canConnect = _testData2Context.Database.CanConnect();
                    if (canConnect)
                    {
                        var pendingMigrations = _testData2Context.Database.GetPendingMigrations();
                        if (pendingMigrations.Any())
                        {
                            Console.WriteLine($"TESTPRACTICE2 DEBUG: Found {pendingMigrations.Count()} pending migrations in GET method: {string.Join(", ", pendingMigrations)}");
                            _testData2Context.Database.Migrate();
                            Console.WriteLine($"TESTPRACTICE2 DEBUG: Migrations applied successfully in GET method");
                        }
                    }
                }
                catch (Exception migrationEx)
                {
                    Console.WriteLine($"TESTPRACTICE2 DEBUG: Migration check/apply failed in GET method: {migrationEx.Message}");
                }
                
                model = _testData2Context.TestData2.FirstOrDefault(t => t.Username == performerUsername);
                totalTestData = _testData2Context.TestData2.Count();
                
                if (model == null)
                {
                    Console.WriteLine($"TESTPRACTICE2 DEBUG: No existing test data found for {performerUsername}, creating new");
                    // Create new test data if not exists
                    model = new TestDataModel2
                    {
                        Username = performerUsername
                    };
                }
                else
                {
                    Console.WriteLine($"TESTPRACTICE2 DEBUG: Found existing test data for {performerUsername} - ID: {model.Id}, Username: {model.Username}");
                }
                
                Console.WriteLine($"TESTPRACTICE2 DEBUG: Total TestData2 records in database: {totalTestData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TESTPRACTICE2 ERROR: Database access failed - {ex.Message}");
                Console.WriteLine($"TESTPRACTICE2 ERROR: Stack trace - {ex.StackTrace}");
                
                // If it's a column missing error, try to apply migrations
                if (ex.Message.Contains("column") && ex.Message.Contains("does not exist"))
                {
                    try
                    {
                        Console.WriteLine($"TESTPRACTICE2 DEBUG: Detected missing column error, attempting to apply TestData2Context migrations...");
                        
                        // First ensure the database can connect
                        var canConnect = _testData2Context.Database.CanConnect();
                        Console.WriteLine($"TESTPRACTICE2 DEBUG: TestData2Context can connect: {canConnect}");
                        
                        // Don't use EnsureCreated() as it conflicts with migrations
                        // Just proceed with migration if we can connect
                        
                        // Check for pending migrations specifically
                        var pendingMigrations = _testData2Context.Database.GetPendingMigrations().ToList();
                        Console.WriteLine($"TESTPRACTICE2 DEBUG: Found {pendingMigrations.Count} pending migrations: {string.Join(", ", pendingMigrations)}");
                        
                        if (pendingMigrations.Any())
                        {
                            _testData2Context.Database.Migrate(); // Use isolated context for TestData2 migrations
                            Console.WriteLine($"TESTPRACTICE2 DEBUG: TestData2Context migrations applied after column error");
                        }
                        else
                        {
                            Console.WriteLine($"TESTPRACTICE2 DEBUG: No pending migrations found, but column still missing - attempting manual column addition");
                            
                            // Manual column addition as fallback for cloud platform issues
                            try
                            {
                                var sql = "ALTER TABLE \"TestData2\" ADD COLUMN IF NOT EXISTS \"AdvisorComment\" text NULL;";
                                Console.WriteLine($"TESTPRACTICE2 DEBUG: Executing manual SQL: {sql}");
                                _testData2Context.Database.ExecuteSqlRaw(sql);
                                Console.WriteLine($"TESTPRACTICE2 DEBUG: Manual column addition completed");
                            }
                            catch (Exception sqlEx)
                            {
                                Console.WriteLine($"TESTPRACTICE2 DEBUG: Manual SQL failed: {sqlEx.Message}");
                            }
                        }
                        
                        // Retry the database query
                        model = _testData2Context.TestData2.FirstOrDefault(t => t.Username == performerUsername);
                        Console.WriteLine($"TESTPRACTICE2 DEBUG: Successfully retrieved data after TestData2Context migration");
                    }
                    catch (Exception migrationEx)
                    {
                        Console.WriteLine($"TESTPRACTICE2 DEBUG: Migration after column error failed: {migrationEx.Message}");
                        Console.WriteLine($"TESTPRACTICE2 DEBUG: Migration exception stack trace: {migrationEx.StackTrace}");
                        model = new TestDataModel2 { Username = performerUsername };
                        TempData["ErrorMessage"] = "Database schema issue detected. The form will work but may not save properly until migrations are applied.";
                    }
                }
                else
                {
                    // Create new model as fallback
                    model = new TestDataModel2
                    {
                        Username = performerUsername
                    };
                    
                    // Add error message to TempData
                    TempData["ErrorMessage"] = "Database table may not exist yet. Please ensure migrations are applied. Using blank form for now.";
                }
                Console.WriteLine("TESTPRACTICE2 DEBUG: Using fallback blank model due to database error");
            }

            return View("Performer/TestPractice2", model);
        }

        [HttpPost]
        public IActionResult TestPractice2(TestDataModel2 model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            Console.WriteLine($"TEST PRACTICE2 DEBUG: POST request received for TestData2, user: {currentUser}");
            Console.WriteLine($"TEST PRACTICE2 DEBUG: Model data - UKWorkExperience: '{model?.UKWorkExperience}', LastPatientTreatment: '{model?.LastPatientTreatment}'");
            
            if (model == null)
            {
                Console.WriteLine($"TEST PRACTICE2 DEBUG: Model is null");
                return RedirectToAction("TestPractice2", new { performerUsername = currentUser });
            }

            // Don't set the username to currentUser - it should come from the form's hidden field
            // This allows advisors to edit performer data without creating new rows
            Console.WriteLine($"TEST PRACTICE2 DEBUG: Form submitted with Username: {model.Username}, CurrentUser: {currentUser}");

            // TESTPRACTICE2 PERMISSION CHECK
            var currentRole = HttpContext.Session.GetString("role");
            
            // Performers can edit their own main fields, advisors/admins can edit AdvisorComment
            var isPerformerEditingOwn = (currentRole == "performer" && model.Username == currentUser);
            var canEditAdvisorComment = (currentRole == "advisor" || currentRole == "admin" || currentRole == "superuser");
            
            // Must be either performer editing their own data OR advisor/admin editing comments
            if (!isPerformerEditingOwn && !canEditAdvisorComment)
            {
                Console.WriteLine($"TEST PRACTICE2 DEBUG: Permission denied - {currentUser} ({currentRole}) cannot edit {model.Username}'s data");
                return RedirectToAction("Index");
            }
            
            Console.WriteLine($"TEST PRACTICE2 DEBUG: Permissions - IsPerformerEditingOwn: {isPerformerEditingOwn}, CanEditAdvisorComment: {canEditAdvisorComment}");

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"TEST PRACTICE2 DEBUG: Checking for existing test data for {model.Username}");
                    
                    // Check if TestData2 database exists and create if needed - USE ISOLATED CONTEXT
                    try
                    {
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Checking TestData2Context database connection...");
                        var canConnect = _testData2Context.Database.CanConnect();
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Can connect to TestData2 database: {canConnect}");
                        
                        // Don't use EnsureCreated() as it conflicts with existing migrations
                        // Just proceed with checking for pending migrations
                        
                        // Apply pending migrations to TestData2Context only
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Checking for TestData2 pending migrations...");
                        var pendingMigrations = _testData2Context.Database.GetPendingMigrations();
                        if (pendingMigrations.Any())
                        {
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: Found {pendingMigrations.Count()} TestData2 pending migrations: {string.Join(", ", pendingMigrations)}");
                            _testData2Context.Database.Migrate();
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: TestData2 migrations applied successfully");
                        }
                        else
                        {
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: No pending migrations found - checking if manual column addition needed");
                            
                            // Manual column addition as fallback for cloud platform issues
                            try
                            {
                                var sql = "ALTER TABLE \"TestData2\" ADD COLUMN IF NOT EXISTS \"AdvisorComment\" text NULL;";
                                Console.WriteLine($"TEST PRACTICE2 DEBUG: Executing manual SQL: {sql}");
                                _testData2Context.Database.ExecuteSqlRaw(sql);
                                Console.WriteLine($"TEST PRACTICE2 DEBUG: Manual column addition completed");
                            }
                            catch (Exception sqlEx)
                            {
                                Console.WriteLine($"TEST PRACTICE2 DEBUG: Manual SQL failed (may already exist): {sqlEx.Message}");
                            }
                        }
                    }
                    catch (Exception dbEx)
                    {
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Database connection/creation/migration error: {dbEx.Message}");
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Stack trace: {dbEx.StackTrace}");
                        
                        // SAFETY: Do NOT delete any database! Just log the error and continue
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Database migration failed, but continuing without destroying data");
                        TempData["ErrorMessage"] = "Database schema issue detected. Please contact administrator to apply migrations.";
                        return View("Performer/TestPractice2", model);
                    }
                    
                    // Ensure only one record per user - delete any existing records first (ISOLATED TO TestData2)
                    var existingRecords = _testData2Context.TestData2.Where(t => t.Username == model.Username).ToList();
                    
                    // TESTPRACTICE2 FIELD-LEVEL PERMISSIONS - CORRECTED
                    if (existingRecords.Any())
                    {
                        var existingRecord = existingRecords.First();
                        
                        if (canEditAdvisorComment && !isPerformerEditingOwn)
                        {
                            // Advisors/Admins can ONLY edit AdvisorComment - preserve all other fields from existing record
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: User {currentUser} ({currentRole}) editing AdvisorComment only for {model.Username}");
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: New AdvisorComment: '{model.AdvisorComment}'");
                            
                            // Copy all fields from existing record EXCEPT AdvisorComment
                            var newAdvisorComment = model.AdvisorComment; // Save the new comment
                            
                            // Use reflection to copy all properties except AdvisorComment
                            var properties = typeof(TestDataModel2).GetProperties();
                            foreach (var prop in properties)
                            {
                                if (prop.Name != "AdvisorComment" && prop.Name != "Id" && prop.CanWrite)
                                {
                                    var existingValue = prop.GetValue(existingRecord);
                                    prop.SetValue(model, existingValue);
                                }
                            }
                            
                            // Restore the new AdvisorComment
                            model.AdvisorComment = newAdvisorComment;
                        }
                        else if (isPerformerEditingOwn)
                        {
                            // Performers can edit main fields but NOT AdvisorComment - preserve AdvisorComment
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: Performer {currentUser} editing their own main fields - preserving AdvisorComment");
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: Existing AdvisorComment: '{existingRecord.AdvisorComment}'");
                            model.AdvisorComment = existingRecord.AdvisorComment; // Preserve existing advisor comment
                        }
                    }
                    else
                    {
                        // No existing record - only performers can create new records for themselves
                        if (!isPerformerEditingOwn)
                        {
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: Cannot create new record for {model.Username} by {currentUser} - must be performer creating their own record");
                            return RedirectToAction("Index");
                        }
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Creating new record for performer {model.Username}");
                    }
                    
                    // Delete existing records before creating new one (following delete-and-recreate pattern)
                    if (existingRecords.Any())
                    {
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Found {existingRecords.Count} existing records for {model.Username} - deleting all");
                        foreach (var record in existingRecords)
                        {
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: Deleting record ID {record.Id} for {model.Username}");
                        }
                        _testData2Context.TestData2.RemoveRange(existingRecords);
                        
                        // Save the deletions first (ISOLATED CONTEXT)
                        var deletedRows = _testData2Context.SaveChanges();
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Deleted {deletedRows} existing records");
                    }
                    else
                    {
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: No existing records found for {model.Username}");
                    }
                    
                    // Create new record with the latest data
                    Console.WriteLine($"TEST PRACTICE2 DEBUG: Creating new record for {model.Username}");
                    Console.WriteLine($"TEST PRACTICE2 DEBUG: New data - UKWorkExperience: '{model.UKWorkExperience}', LastPatientTreatment: '{model.LastPatientTreatment}'");
                    
                    // Set audit fields for new record
                    model.CreatedDate = DateTime.UtcNow;
                    model.ModifiedDate = null;
                    
                    // Add new record (ISOLATED CONTEXT)
                    _testData2Context.TestData2.Add(model);
                    
                    var savedRows = _testData2Context.SaveChanges();
                    Console.WriteLine($"TEST PRACTICE2 DEBUG: SaveChanges() affected {savedRows} rows for {model.Username}");
                    
                    if (savedRows > 0)
                    {
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Save successful! Record ID: {model.Id}");
                        TempData["SuccessMessage"] = $"Test practice 2 data saved successfully to PostgreSQL! Record ID: {model.Id}";
                        
                        // Check total TestData2 count (ISOLATED)
                        var totalTestData = _testData2Context.TestData2.Count();
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Total TestData2 records in database: {totalTestData}");
                        TempData["TotalRecords"] = totalTestData;
                        
                        return RedirectToAction("TestPractice2", new { performerUsername = model.Username });
                    }
                    else
                    {
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Save failed - no rows affected");
                        TempData["ErrorMessage"] = "Save failed - no changes were made to the database.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TEST PRACTICE2 DEBUG: Exception during save: {ex.Message}");
                    Console.WriteLine($"TEST PRACTICE2 DEBUG: Stack trace: {ex.StackTrace}");
                    
                    var innerEx = ex.InnerException;
                    var level = 1;
                    while (innerEx != null)
                    {
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Inner exception level {level}: {innerEx.Message}");
                        Console.WriteLine($"TEST PRACTICE2 DEBUG: Inner exception level {level} type: {innerEx.GetType().Name}");
                        if (innerEx.StackTrace != null)
                        {
                            Console.WriteLine($"TEST PRACTICE2 DEBUG: Inner exception level {level} stack trace: {innerEx.StackTrace}");
                        }
                        innerEx = innerEx.InnerException;
                        level++;
                    }
                    
                    // Show user-friendly message but log details
                    var errorMsg = $"Database error: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        errorMsg += $" | Inner: {ex.InnerException.Message}";
                    }
                    TempData["ErrorMessage"] = errorMsg;
                }
            }
            else
            {
                Console.WriteLine($"TEST PRACTICE2 DEBUG: Model validation failed, returning to form");
                TempData["ErrorMessage"] = "Please fill in all required fields.";
            }

            // Return with ViewBag set up
            ViewBag.PerformerUsername = model.Username;
            ViewBag.CurrentUserRole = HttpContext.Session.GetString("role");
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == model.Username);
            ViewBag.CanEdit = true;
            ViewBag.CanComment = false;
            ViewBag.CanApprove = false;
            ViewBag.IsReadOnly = false;
            ViewBag.ActiveSection = "TestPractice2";

            // Use main context for performer name lookup - this is safe as it's read-only
            var performer = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            return View("Performer/TestPractice2", model);
        }

        public IActionResult StructuredConversation(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Get performer for validation
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer == null)
            {
                return NotFound("Performer not found");
            }

            // Get existing structured conversation data
            var existingData = _context.StructuredConversations
                .FirstOrDefault(sc => sc.Username == performerUsername);

            var model = existingData ?? new StructuredConversationModel { Username = performerUsername };

            // Set ViewBag properties for permissions
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.ActiveSection = "StructuredConversation";
            ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            
            // Advisors can edit all fields, others can only view
            ViewBag.CanEditAdvisorFields = (currentRole == "advisor" || currentRole == "admin" || currentRole == "superuser");

            return View("Performer/StructuredConversation", model);
        }

        [HttpPost]
        public IActionResult StructuredConversation(StructuredConversationModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            if (model == null)
            {
                return RedirectToAction("StructuredConversation", new { performerUsername = currentUser });
            }

            // CRITICAL: Username comes from form (performer being viewed), not logged-in user
            // This allows advisors to edit performer data without creating new rows
            Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: Form submitted with Username: {model.Username}, CurrentUser: {currentUser}");

            // Permission check - only advisors/admins can edit
            var canEditAdvisorFields = (currentRole == "advisor" || currentRole == "admin" || currentRole == "superuser");
            
            if (!canEditAdvisorFields)
            {
                Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: Access denied - role {currentRole} cannot edit structured conversation");
                TempData["ErrorMessage"] = "Only advisors and administrators can edit structured conversations.";
                return RedirectToAction("StructuredConversation", new { performerUsername = model.Username });
            }

            Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: {currentRole} user {currentUser} authorized to edit structured conversation for {model.Username}");

            if (ModelState.IsValid)
            {
                try
                {
                    // Database connection verification - SAFE METHOD (following DATABASE_INTEGRATION_PATTERN.md)
                    var canConnect = _context.Database.CanConnect();
                    if (!canConnect)
                    {
                        // ⚠️ ONLY use EnsureCreated() if database doesn't exist at all
                        // NEVER use EnsureDeleted() - it destroys ALL data
                        _context.Database.EnsureCreated();
                    }
                    
                    // CRITICAL: Delete all existing records for this user first (delete-and-recreate pattern)
                    var existingRecords = _context.StructuredConversations.Where(x => x.Username == model.Username).ToList();
                    
                    if (existingRecords.Any())
                    {
                        Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: Found {existingRecords.Count} existing records for {model.Username} - deleting all");
                        _context.StructuredConversations.RemoveRange(existingRecords);
                        
                        // Save deletions first
                        var deletedRows = _context.SaveChanges();
                        Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: Deleted {deletedRows} existing records");
                    }
                    
                    // Create new record with latest data
                    Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: Creating new record for {model.Username}");
                    
                    model.CreatedDate = DateTime.UtcNow;
                    model.ModifiedDate = null;
                    
                    _context.StructuredConversations.Add(model);
                    
                    var savedRows = _context.SaveChanges();
                    
                    if (savedRows > 0)
                    {
                        TempData["SuccessMessage"] = "Structured conversation saved successfully!";
                        return RedirectToAction("StructuredConversation", new { performerUsername = model.Username });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Save failed - no changes were made.";
                    }
                }
                catch (Exception ex)
                {
                    // ✅ SAFE ERROR HANDLING - Log errors without destroying data
                    Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: Exception: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error saving structured conversation: {ex.Message}";
                    
                    // 🚨 NEVER DO THIS IN CATCH BLOCKS:
                    // _context.Database.EnsureDeleted(); // ❌ DESTROYS ALL DATA
                    // _context.Database.EnsureCreated();  // ❌ AFTER DELETION
                }
            }
            else
            {
                Console.WriteLine($"STRUCTURED CONVERSATION DEBUG: ModelState invalid, redisplaying form");
                TempData["ErrorMessage"] = "Please correct the validation errors and try again.";
            }

            return View("Performer/StructuredConversation", model);
        }

        public IActionResult AgreementTerms(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            ViewBag.ActiveSection = "AgreementTerms";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Load or create AgreementTermsModel
            var model = _context.AgreementTerms.FirstOrDefault(a => a.Username == performerUsername);
            if (model == null)
            {
                model = new AgreementTermsModel { Username = performerUsername };
            }

            return View("Performer/AgreementTerms", model);
        }

        public IActionResult WorkBasedAssessments(string performerUsername)
        {
            Console.WriteLine($"DEBUG: WorkBasedAssessments accessed for performer: {performerUsername}");
                        
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Emergency table creation and schema migration - following Database Integration Pattern
            try
            {
                var testQuery = _context.WorkBasedAssessments.Take(1).ToList();
                Console.WriteLine("WorkBasedAssessments table exists and is accessible");
            }
            catch (Exception tableEx)
            {
                Console.WriteLine($"WorkBasedAssessments table access failed: {tableEx.Message}");
                
                // Try to add missing columns first (in case table exists but with old schema)
                try
                {
                    Console.WriteLine("Attempting to add missing columns to existing table");
                    _context.Database.ExecuteSqlRaw(@"
                        ALTER TABLE ""WorkBasedAssessments"" 
                        ADD COLUMN IF NOT EXISTS ""ProcedureDescription"" TEXT,
                        ADD COLUMN IF NOT EXISTS ""LearningReflection"" TEXT,
                        ADD COLUMN IF NOT EXISTS ""LearningNeeds"" TEXT,
                        ADD COLUMN IF NOT EXISTS ""OverallAcceptable"" BOOLEAN,
                        ADD COLUMN IF NOT EXISTS ""SupervisorActionPlan"" TEXT;
                    ");
                    Console.WriteLine("Successfully added missing columns");
                }
                catch (Exception alterEx)
                {
                    Console.WriteLine($"Column addition failed: {alterEx.Message}. Creating new table.");
                    
                    // If column addition fails, create the complete table
                    try
                    {
                        _context.Database.ExecuteSqlRaw(@"
                            CREATE TABLE IF NOT EXISTS ""WorkBasedAssessments"" (
                                ""Id"" SERIAL PRIMARY KEY,
                                ""Username"" TEXT NOT NULL,
                                ""AssessmentType"" TEXT NOT NULL,
                                ""Title"" TEXT NOT NULL,
                                ""Status"" TEXT NOT NULL DEFAULT 'Draft',
                                ""AssessmentDate"" TIMESTAMP,
                                ""ProcedureDescription"" TEXT,
                                ""LearningReflection"" TEXT,
                                ""LearningNeeds"" TEXT,
                                ""IsPerformerSubmitted"" BOOLEAN NOT NULL DEFAULT FALSE,
                                ""PerformerSubmittedAt"" TIMESTAMP,
                                ""OverallAcceptable"" BOOLEAN,
                                ""SupervisorActionPlan"" TEXT,
                                ""IsSupervisorCompleted"" BOOLEAN NOT NULL DEFAULT FALSE,
                                ""CompletedBySupervisor"" TEXT,
                                ""SupervisorCompletedAt"" TIMESTAMP,
                                ""CreatedAt"" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                                ""UpdatedAt"" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
                            );
                        ");
                        Console.WriteLine("Emergency WorkBasedAssessments table creation completed");
                    }
                    catch (Exception createEx)
                    {
                        Console.WriteLine($"Emergency table creation failed: {createEx.Message}");
                        _context.Database.EnsureCreated();
                    }
                }
            }

            // Get all assessments for this performer
            var assessments = _context.WorkBasedAssessments
                .Where(a => a.Username == performerUsername)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            Console.WriteLine($"DEBUG: Found {assessments.Count} assessments for {performerUsername}");

            // Set up ViewBag for navigation and permissions
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentRole = currentRole;
            ViewBag.ActiveSection = "WorkBasedAssessments";
            ViewBag.Assessments = assessments;
            ViewBag.CanCreate = (currentRole == "performer" && performerUsername == currentUser) || currentRole == "admin";
            ViewBag.CanEditSupervisor = currentRole == "admin" || currentRole == "advisor" || currentRole == "supervisor";

            // Get performer display name
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }
            else
            {
                ViewBag.PerformerName = performerUsername;
            }

            return View("Performer/WorkBasedAssessments");
        }

        public IActionResult LearningModules(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "LearningModules");
        }

        public IActionResult LearningProgress(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "LearningProgress");
        }

        public IActionResult SupplementaryUploads(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "SupplementaryUploads");
        }

        public IActionResult ManagementPlan(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "ManagementPlan");
        }

        public IActionResult SkillsAssessment(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "SkillsAssessment");
        }

        public IActionResult CPD(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            ViewBag.ActiveSection = "CPD";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Get uploaded CPD files for this performer
            var fileUpload = _context.FileUploads
                .Where(f => f.Username == performerUsername)
                .FirstOrDefault();

            if (fileUpload != null)
            {
                // Get file entries for this FileUpload
                var allFiles = _context.FileUploadEntries
                    .Where(f => f.FileUploadModelId == fileUpload.Id)
                    .OrderByDescending(f => f.UploadedAt)
                    .ToList();

                // Separate CPD files by category
                ViewBag.MedicalEmergenciesFiles = allFiles
                    .Where(f => f.Category == "CPD_MedicalEmergencies")
                    .ToList();
                
                ViewBag.DisinfectionDecontaminationFiles = allFiles
                    .Where(f => f.Category == "CPD_DisinfectionDecontamination")
                    .ToList();
                
                ViewBag.RadiographyRadiationProtectionFiles = allFiles
                    .Where(f => f.Category == "CPD_RadiographyRadiationProtection")
                    .ToList();
                
                ViewBag.SafeguardingChildrenVulnerableAdultsFiles = allFiles
                    .Where(f => f.Category == "CPD_SafeguardingChildrenVulnerableAdults")
                    .ToList();
                
                ViewBag.ExtraCPDFiles = allFiles
                    .Where(f => f.Category == "CPD_Extra")
                    .ToList();
            }
            else
            {
                ViewBag.MedicalEmergenciesFiles = new List<FileUploadEntry>();
                ViewBag.DisinfectionDecontaminationFiles = new List<FileUploadEntry>();
                ViewBag.RadiographyRadiationProtectionFiles = new List<FileUploadEntry>();
                ViewBag.SafeguardingChildrenVulnerableAdultsFiles = new List<FileUploadEntry>();
                ViewBag.ExtraCPDFiles = new List<FileUploadEntry>();
            }

            return View("Performer/CPD");
        }

        public IActionResult MSF(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            try
            {
                // Get the performer user first
                var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
                if (performer == null)
                {
                    ViewBag.HasActiveAssessment = false;
                    ViewBag.ResponseCount = 0;
                    return HandlePerformerSection(performerUsername, "MSF");
                }

                // Check for existing active MSF questionnaire using PerformerId
                var existingQuestionnaire = _context.MSFQuestionnaires
                    .Include(q => q.Responses)
                    .FirstOrDefault(q => q.PerformerId == performer.Id && q.IsActive);

                if (existingQuestionnaire != null)
                {
                    // Set ViewBag properties for existing assessment
                    ViewBag.HasActiveAssessment = true;
                    ViewBag.AssessmentTitle = existingQuestionnaire.Title;
                    ViewBag.CreatedDate = existingQuestionnaire.CreatedAt.ToString("dd/MM/yyyy");
                    ViewBag.ResponseCount = existingQuestionnaire.Responses?.Count ?? 0;
                    
                    // Generate feedback URL using proper URL generation
                    var feedbackUrl = Url.Action("Feedback", "MSF", new { code = existingQuestionnaire.UniqueCode }, Request.Scheme);
                    ViewBag.FeedbackUrl = feedbackUrl;
                    
                    // For now, skip QR code generation to avoid complexity
                    ViewBag.QRCodeImage = "";
                }
                else
                {
                    ViewBag.HasActiveAssessment = false;
                    ViewBag.ResponseCount = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSF DEBUG: Error loading MSF data: {ex.Message}");
                ViewBag.HasActiveAssessment = false;
                ViewBag.ResponseCount = 0;
                TempData["ErrorMessage"] = "Error loading MSF data. Using blank form.";
            }

            return HandlePerformerSection(performerUsername, "MSF");
        }

        [HttpPost]
        public IActionResult MSF(string performerUsername, string action)
        {
            var currentUser = HttpContext.Session.GetString("username");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions - only the performer can start their own MSF assessment
            var currentRole = HttpContext.Session.GetString("role");
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                TempData["ErrorMessage"] = "You can only manage your own MSF assessments.";
                return RedirectToAction("MSF", new { performerUsername = currentUser });
            }

            if (action == "start")
            {
                try
                {
                    // Get the performer user first
                    var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
                    if (performer == null)
                    {
                        TempData["ErrorMessage"] = "Performer not found.";
                        return RedirectToAction("MSF", new { performerUsername });
                    }

                    // Use Database Integration Pattern: Delete and recreate
                    // First, deactivate any existing questionnaires for this performer using PerformerId
                    var existingQuestionnaires = _context.MSFQuestionnaires
                        .Where(q => q.PerformerId == performer.Id)
                        .ToList();

                    foreach (var existing in existingQuestionnaires)
                    {
                        existing.IsActive = false;
                    }

                    // Create new MSF questionnaire
                    var newQuestionnaire = new MSFQuestionnaire
                    {
                        Title = $"MSF Assessment - {DateTime.Now:dd/MM/yyyy}",
                        UniqueCode = Guid.NewGuid().ToString("N")[..8].ToUpper(), // 8-character unique code
                        PerformerId = performer.Id,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.MSFQuestionnaires.Add(newQuestionnaire);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "New MSF assessment created successfully!";
                    Console.WriteLine($"MSF DEBUG: Created new questionnaire with code {newQuestionnaire.UniqueCode} for {performerUsername}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"MSF DEBUG: Error creating new assessment: {ex.Message}");
                    TempData["ErrorMessage"] = "Error creating new MSF assessment. Please try again.";
                }
            }

            return RedirectToAction("MSF", new { performerUsername });
        }

        public IActionResult PSQ(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "PSQ");
        }

        public IActionResult ClinicalNoteAudit(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "ClinicalNoteAudit");
        }

        public IActionResult HelpGuidance(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "HelpGuidance");
        }

        public IActionResult FinalSignOff(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "FinalSignOff");
        }

        private IActionResult HandlePerformerSection(string performerUsername, string sectionName)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            ViewBag.ActiveSection = sectionName;

            // Get performer's name for display
            var performer3 = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer3 != null)
            {
                ViewBag.PerformerName = $"{performer3.FirstName} {performer3.LastName}";
            }

            return View($"Performer/{sectionName}");
        }

        // POST method for creating new assessment
        [HttpPost]
        public IActionResult CreateWorkBasedAssessment(string performerUsername, string assessmentType)
        {
            Console.WriteLine($"DEBUG: Creating new {assessmentType} assessment for {performerUsername}");
            
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Validation
            if (string.IsNullOrEmpty(assessmentType) || (assessmentType != "DOPS" && assessmentType != "CBDs"))
            {
                TempData["ErrorMessage"] = "Please select a valid assessment type (DOPS or CBDs).";
                return RedirectToAction("WorkBasedAssessments", new { performerUsername });
            }

            try
            {
                // Create new assessment
                var newAssessment = new WorkBasedAssessmentModel
                {
                    Username = performerUsername,
                    AssessmentType = assessmentType,
                    Title = $"{assessmentType} Assessment - {DateTime.Now:yyyy-MM-dd}",
                    Status = "Draft",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.WorkBasedAssessments.Add(newAssessment);
                _context.SaveChanges();

                Console.WriteLine($"DEBUG: Created new assessment with ID: {newAssessment.Id}");
                TempData["SuccessMessage"] = $"New {assessmentType} assessment created successfully.";
                
                // Redirect to edit the new assessment
                return RedirectToAction("EditWorkBasedAssessment", new { id = newAssessment.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to create assessment: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to create assessment. Please try again.";
                return RedirectToAction("WorkBasedAssessments", new { performerUsername });
            }
        }

        // GET method for editing/completing assessment
        public IActionResult EditWorkBasedAssessment(int id, string? performerUsername = null)
        {
            Console.WriteLine($"\n=== EditWorkBasedAssessment GET DEBUG START ===");
            Console.WriteLine($"DEBUG: EditWorkBasedAssessment accessed for ID: {id}, performerUsername: '{performerUsername}'");
            
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var assessment = _context.WorkBasedAssessments.FirstOrDefault(a => a.Id == id);
                if (assessment == null)
                {
                    Console.WriteLine($"ERROR: Assessment with ID {id} not found in database");
                    TempData["ErrorMessage"] = "Assessment not found.";
                    return RedirectToAction("Index");
                }

                Console.WriteLine($"DEBUG: Found assessment for editing:");
                Console.WriteLine($"DEBUG: - ID: {assessment.Id}, Username: '{assessment.Username}', Type: '{assessment.AssessmentType}'");

                // CRITICAL FIX: Ensure we use the correct performer username
                var targetPerformerUsername = !string.IsNullOrEmpty(performerUsername) ? performerUsername : assessment.Username;
                
                Console.WriteLine($"DEBUG: Target performer username: '{targetPerformerUsername}'");
                Console.WriteLine($"DEBUG: Assessment belongs to: '{assessment.Username}'");
                
                // Validate that the assessment belongs to the specified performer
                if (assessment.Username != targetPerformerUsername)
                {
                    Console.WriteLine($"ERROR: Assessment belongs to '{assessment.Username}' but trying to access for '{targetPerformerUsername}'");
                    TempData["ErrorMessage"] = "Assessment not found for this performer.";
                    return RedirectToAction("WorkBasedAssessments", new { performerUsername = targetPerformerUsername });
                }

                // Set up ViewBag for navigation and permissions
                ViewBag.PerformerUsername = targetPerformerUsername;
                ViewBag.CurrentUser = currentUser;
                ViewBag.CurrentRole = currentRole;
                ViewBag.ActiveSection = "WorkBasedAssessments";
                
                // Determine what user can edit
                ViewBag.CanEditPerformer = (currentRole == "performer" && assessment.Username == currentUser) || currentRole == "admin";
                ViewBag.CanEditSupervisor = currentRole == "admin" || currentRole == "advisor" || currentRole == "supervisor";
                ViewBag.IsReadOnly = (!ViewBag.CanEditPerformer && !ViewBag.CanEditSupervisor);

                Console.WriteLine($"DEBUG: ROLE PERMISSIONS - CurrentUser: '{currentUser}', CurrentRole: '{currentRole}'");
                Console.WriteLine($"DEBUG: ROLE PERMISSIONS - CanEditPerformer: {ViewBag.CanEditPerformer}");
                Console.WriteLine($"DEBUG: ROLE PERMISSIONS - CanEditSupervisor: {ViewBag.CanEditSupervisor}");
                Console.WriteLine($"DEBUG: ROLE PERMISSIONS - IsReadOnly: {ViewBag.IsReadOnly}");

                // Get performer display name
                var performer = _context.Users.FirstOrDefault(u => u.Username == assessment.Username);
                if (performer != null)
                {
                    ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
                }
                else
                {
                    ViewBag.PerformerName = assessment.Username;
                }

                return View("Performer/EditWorkBasedAssessment", assessment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to load assessment: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load assessment.";
                return RedirectToAction("Index");
            }
        }

        // POST method for performer to update assessment - Database Integration Pattern with Field-Level Permissions
        [HttpPost]
        public IActionResult UpdatePerformerAssessment(WorkBasedAssessmentModel model, bool isSubmission = false, bool isSupervisorCompletion = false)
        {
            Console.WriteLine($"\n=== UpdatePerformerAssessment DEBUG START ===");
            Console.WriteLine($"DEBUG: Received model - ID: {model.Id}, Username: '{model.Username}', AssessmentType: '{model.AssessmentType}'");
            Console.WriteLine($"DEBUG: isSubmission: {isSubmission}, isSupervisorCompletion: {isSupervisorCompletion}");
            
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine($"ERROR: No current user in session");
                return RedirectToAction("Login", "Account");
            }

            if (model == null)
            {
                return RedirectToAction("WorkBasedAssessments", new { performerUsername = currentUser });
            }

            // Field-Level Permissions Pattern - Validate permissions before processing
            var isPerformerEditingOwn = (currentRole == "performer" && model.Username == currentUser);
            var canEditPerformerFields = isPerformerEditingOwn || currentRole == "admin";
            var canEditSupervisorFields = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "admin");
            
            // Permission validation
            if (!canEditPerformerFields && !canEditSupervisorFields)
            {
                Console.WriteLine($"ERROR: User {currentUser} ({currentRole}) has no permission to edit assessment for {model.Username}");
                TempData["ErrorMessage"] = "You don't have permission to edit this assessment.";
                return RedirectToAction("WorkBasedAssessments", new { performerUsername = model.Username });
            }
            
            // Performers cannot submit assessments they can't edit
            if (isSubmission && !canEditPerformerFields)
            {
                Console.WriteLine($"ERROR: User {currentUser} ({currentRole}) cannot submit assessment for {model.Username}");
                TempData["ErrorMessage"] = "You don't have permission to submit this assessment.";
                return RedirectToAction("EditWorkBasedAssessment", new { id = model.Id, performerUsername = model.Username });
            }
            
            // Supervisors cannot complete assessments they can't edit
            if (isSupervisorCompletion && !canEditSupervisorFields)
            {
                Console.WriteLine($"ERROR: User {currentUser} ({currentRole}) cannot complete assessment for {model.Username}");
                TempData["ErrorMessage"] = "You don't have permission to complete this assessment.";
                return RedirectToAction("EditWorkBasedAssessment", new { id = model.Id, performerUsername = model.Username });
            }

            // Database Integration Pattern - Use delete-and-recreate for reliability
            if (ModelState.IsValid)
            {
                try
                {
                    // Database connection verification - SAFE METHOD
                    var canConnect = _context.Database.CanConnect();
                    if (!canConnect)
                    {
                        // ⚠️ ONLY use EnsureCreated() if database doesn't exist at all
                        _context.Database.EnsureCreated();
                    }
                    
                    // CRITICAL: Delete existing record for this ID first
                    var existingRecords = _context.WorkBasedAssessments.Where(x => x.Id == model.Id).ToList();
                    
                    if (existingRecords.Any())
                    {
                        var existingRecord = existingRecords.First();
                        Console.WriteLine($"DEBUG: Found existing record for ID {model.Id} - applying Field-Level Permissions");
                        
                        // Field-Level Permissions Pattern: Preserve fields based on user permissions
                        if (isSupervisorCompletion && canEditSupervisorFields && !canEditPerformerFields)
                        {
                            // Supervisors completing: Preserve all fields EXCEPT supervisor fields
                            Console.WriteLine($"Supervisor {currentUser} completing assessment - preserving performer fields");
                            
                            var newOverallAcceptable = model.OverallAcceptable;
                            var newSupervisorActionPlan = model.SupervisorActionPlan;
                            
                            // Use reflection to copy all properties except supervisor fields
                            var properties = typeof(WorkBasedAssessmentModel).GetProperties();
                            foreach (var prop in properties)
                            {
                                if (prop.Name != "OverallAcceptable" && 
                                    prop.Name != "SupervisorActionPlan" && 
                                    prop.Name != "IsSupervisorCompleted" &&
                                    prop.Name != "CompletedBySupervisor" &&
                                    prop.Name != "SupervisorCompletedAt" &&
                                    prop.Name != "Status" &&
                                    prop.Name != "UpdatedAt" &&
                                    prop.Name != "Id" && 
                                    prop.CanWrite)
                                {
                                    var existingValue = prop.GetValue(existingRecord);
                                    prop.SetValue(model, existingValue);
                                }
                            }
                            
                            // Restore supervisor fields with new values
                            model.OverallAcceptable = newOverallAcceptable;
                            model.SupervisorActionPlan = newSupervisorActionPlan;
                            
                            // Set completion metadata
                            model.IsSupervisorCompleted = true;
                            model.CompletedBySupervisor = currentUser;
                            model.SupervisorCompletedAt = DateTime.UtcNow;
                            model.Status = "Complete";
                        }
                        else if (canEditPerformerFields && !isSupervisorCompletion)
                        {
                            // Performers editing: Preserve supervisor fields, edit everything else
                            Console.WriteLine($"Performer {currentUser} editing - preserving supervisor fields");
                            
                            model.OverallAcceptable = existingRecord.OverallAcceptable;
                            model.SupervisorActionPlan = existingRecord.SupervisorActionPlan;
                            model.IsSupervisorCompleted = existingRecord.IsSupervisorCompleted;
                            model.CompletedBySupervisor = existingRecord.CompletedBySupervisor;
                            model.SupervisorCompletedAt = existingRecord.SupervisorCompletedAt;
                            
                            // Preserve creation timestamp
                            model.CreatedAt = existingRecord.CreatedAt;
                        }
                        else
                        {
                            // Default: preserve metadata
                            model.CreatedAt = existingRecord.CreatedAt;
                            model.IsPerformerSubmitted = existingRecord.IsPerformerSubmitted;
                            model.PerformerSubmittedAt = existingRecord.PerformerSubmittedAt;
                            model.OverallAcceptable = existingRecord.OverallAcceptable;
                            model.SupervisorActionPlan = existingRecord.SupervisorActionPlan;
                            model.IsSupervisorCompleted = existingRecord.IsSupervisorCompleted;
                            model.CompletedBySupervisor = existingRecord.CompletedBySupervisor;
                            model.SupervisorCompletedAt = existingRecord.SupervisorCompletedAt;
                        }
                        
                        // Handle performer submission
                        if (isSubmission && canEditPerformerFields)
                        {
                            model.IsPerformerSubmitted = true;
                            model.PerformerSubmittedAt = DateTime.UtcNow;
                            Console.WriteLine($"DEBUG: Marking assessment ID {model.Id} as submitted by performer");
                        }
                        
                        _context.WorkBasedAssessments.RemoveRange(existingRecords);
                        var deletedRows = _context.SaveChanges();
                        Console.WriteLine($"DEBUG: Deleted {deletedRows} existing records");
                    }
                    
                    model.UpdatedAt = DateTime.UtcNow;
                    
                    _context.WorkBasedAssessments.Add(model);
                    
                    var savedRows = _context.SaveChanges();
                    
                    if (savedRows > 0)
                    {
                        Console.WriteLine($"DEBUG: Successfully saved assessment using Database Integration Pattern");
                        if (isSupervisorCompletion)
                        {
                            TempData["SuccessMessage"] = "Assessment completed successfully! The performer has been notified.";
                        }
                        else if (isSubmission)
                        {
                            TempData["SuccessMessage"] = "Assessment submitted successfully! It's now available for supervisor review.";
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "Assessment updated successfully!";
                        }
                        Console.WriteLine($"=== UpdatePerformerAssessment DEBUG END ===\n");
                        return RedirectToAction("EditWorkBasedAssessment", new { id = model.Id, performerUsername = model.Username });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Save failed - no changes were made.";
                    }
                }
                catch (Exception ex)
                {
                    // ✅ SAFE ERROR HANDLING - Log errors without destroying data
                    Console.WriteLine($"DEBUG: Exception: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error saving data: {ex.Message}";
                    
                    // 🚨 NEVER DO THIS IN CATCH BLOCKS:
                    // _context.Database.EnsureDeleted(); // ❌ DESTROYS ALL DATA
                    // _context.Database.EnsureCreated();  // ❌ AFTER DELETION
                }
            }
            else
            {
                Console.WriteLine($"DEBUG: ModelState validation failed");
                foreach (var modelState in ModelState)
                {
                    var key = modelState.Key;
                    var state = modelState.Value;
                    if (state.Errors.Count > 0)
                    {
                        Console.WriteLine($"DEBUG: VALIDATION ERROR - Key: '{key}'");
                        foreach (var error in state.Errors)
                        {
                            Console.WriteLine($"DEBUG: VALIDATION ERROR - Message: '{error.ErrorMessage}'");
                        }
                    }
                }
            }

            return RedirectToAction("EditWorkBasedAssessment", new { id = model.Id, performerUsername = model.Username });
        }
    }
}
