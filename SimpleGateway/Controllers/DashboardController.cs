using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;
using SimpleGateway.Data;

namespace SimpleGateway.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
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

        public IActionResult PreviousExperience(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "PreviousExperience");
        }

        public IActionResult PreviousExperienceForm(string performerUsername)
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
            ViewBag.ActiveSection = "PreviousExperience";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            return View("Performer/PreviousExperienceBlank");
        }

        // Other section methods (simplified for brevity)
        public IActionResult TestPractice(string performerUsername)
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
            ViewBag.ActiveSection = "TestPractice";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Create new empty model for test
            var model = new TestDataModel
            {
                Username = performerUsername
            };

            return View("Performer/TestPractice", model);
        }

        [HttpPost]
        public IActionResult TestPractice(TestDataModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            Console.WriteLine($"TEST PRACTICE DEBUG: POST request received for TestData, user: {currentUser}");
            Console.WriteLine($"TEST PRACTICE DEBUG: Model data - UKWorkExperience: '{model?.UKWorkExperience}', LastPatientTreatment: '{model?.LastPatientTreatment}'");
            
            if (model == null)
            {
                Console.WriteLine($"TEST PRACTICE DEBUG: Model is null");
                return RedirectToAction("TestPractice", new { performerUsername = currentUser });
            }

            // Set the username and audit fields
            model.Username = currentUser;
            model.CreatedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"TEST PRACTICE DEBUG: Adding new test data record for {model.Username}");
                    
                    // Add new record
                    _context.TestData.Add(model);
                    
                    var savedRows = _context.SaveChanges();
                    Console.WriteLine($"TEST PRACTICE DEBUG: SaveChanges() affected {savedRows} rows for {model.Username}");
                    
                    if (savedRows > 0)
                    {
                        Console.WriteLine($"TEST PRACTICE DEBUG: Save successful! Record ID: {model.Id}");
                        TempData["SuccessMessage"] = $"Test practice data saved successfully to PostgreSQL! Record ID: {model.Id}";
                        
                        // Check total TestData count
                        var totalTestData = _context.TestData.Count();
                        Console.WriteLine($"TEST PRACTICE DEBUG: Total TestData records in database: {totalTestData}");
                        TempData["TotalRecords"] = totalTestData;
                        
                        return RedirectToAction("TestPractice", new { performerUsername = model.Username });
                    }
                    else
                    {
                        Console.WriteLine($"TEST PRACTICE DEBUG: Save failed - no rows affected");
                        TempData["ErrorMessage"] = "Save failed - no changes were made to the database.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TEST PRACTICE DEBUG: Exception during save: {ex.Message}");
                    Console.WriteLine($"TEST PRACTICE DEBUG: Stack trace: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"TEST PRACTICE DEBUG: Inner exception: {ex.InnerException.Message}");
                    }
                    TempData["ErrorMessage"] = $"Error saving test practice data: {ex.Message}";
                }
            }
            else
            {
                Console.WriteLine($"TEST PRACTICE DEBUG: Model validation failed, returning to form");
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
            ViewBag.ActiveSection = "TestPractice";

            var performer = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            return View("Performer/TestPractice", model);
        }

        public IActionResult StructuredConversation(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "StructuredConversation");
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
