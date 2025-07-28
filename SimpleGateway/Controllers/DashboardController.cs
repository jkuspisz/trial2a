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

            return RedirectToAction("Uploads", new { performerUsername });
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

            return RedirectToAction("Uploads", new { performerUsername });
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
                // Check if we can query the table with the current model
                try
                {
                    var testQuery = _context.TestData.FirstOrDefault(x => x.Username == performerUsername);
                    var model = testQuery ?? new TestDataModel { Username = performerUsername };
                    return View("Performer/TestPractice", model);
                }
                catch (Exception schemaEx)
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: Schema mismatch detected: {schemaEx.Message}");
                    
                    // Schema mismatch - need to recreate table with correct structure
                    if (schemaEx.Message.Contains("column") && schemaEx.Message.Contains("does not exist"))
                    {
                        Console.WriteLine($"TESTPRACTICE DEBUG: Dropping and recreating TestData table due to missing columns");
                        
                        // Drop the existing table and recreate it with correct schema
                        _context.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS \"TestData\"");
                        
                        // Recreate with correct schema
                        _context.Database.EnsureCreated();
                        
                        Console.WriteLine($"TESTPRACTICE DEBUG: TestData table recreated with supervisor fields");
                        TempData["SuccessMessage"] = "Database table recreated with updated fields. Ready to use!";
                    }
                    
                    var model = new TestDataModel { Username = performerUsername };
                    return View("Performer/TestPractice", model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TESTPRACTICE DEBUG: Database error in GET: {ex.Message}");
                TempData["ErrorMessage"] = $"Database error: {ex.Message}";
                
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
                    // Check if table schema matches the model
                    var canQuery = _context.TestData.Any(); // This will fail if schema is wrong
                    
                    // CRITICAL: Delete all existing records for this user first
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
                        TempData["SuccessMessage"] = "Data saved successfully!";
                        return RedirectToAction("TestPractice", new { performerUsername = model.Username });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Save failed - no changes were made.";
                    }
                }
                catch (Exception schemaEx) when (schemaEx.Message.Contains("column") && schemaEx.Message.Contains("does not exist"))
                {
                    Console.WriteLine($"TESTPRACTICE DEBUG: Schema mismatch in POST: {schemaEx.Message}");
                    
                    // Drop and recreate table with correct schema
                    _context.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS \"TestData\"");
                    _context.Database.EnsureCreated();
                    
                    Console.WriteLine($"TESTPRACTICE DEBUG: TestData table recreated in POST method");
                    
                    // Now try to save again with fresh table
                    model.CreatedDate = DateTime.UtcNow;
                    model.ModifiedDate = null;
                    
                    _context.TestData.Add(model);
                    var savedRows = _context.SaveChanges();
                    
                    if (savedRows > 0)
                    {
                        TempData["SuccessMessage"] = "Data saved successfully! (Table recreated with updated schema)";
                        return RedirectToAction("TestPractice", new { performerUsername = model.Username });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Save failed after table recreation.";
                    }
                }
                catch (Exception ex)
                {
                    // ✅ SAFE ERROR HANDLING - Log errors without destroying data
                    Console.WriteLine($"TESTPRACTICE DEBUG: Exception: {ex.Message}");
                    Console.WriteLine($"TESTPRACTICE DEBUG: Inner Exception: {ex.InnerException?.Message}");
                    TempData["ErrorMessage"] = $"Error saving data: {ex.Message}";
                    
                    // 🚨 NEVER DO THIS IN CATCH BLOCKS:
                    // _context.Database.EnsureDeleted(); // ❌ DESTROYS ALL DATA
                    // _context.Database.EnsureCreated();  // ❌ AFTER DELETION
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
            return HandlePerformerSection(performerUsername, "AgreementTerms");
        }

        public IActionResult WorkBasedAssessments(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "WorkBasedAssessments");
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
            return HandlePerformerSection(performerUsername, "CPD");
        }

        public IActionResult MSF(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "MSF");
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
    }
}
