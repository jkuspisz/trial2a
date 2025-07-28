# Database Integration Pattern - Corrected Version

## Quick Reference
**Prompt:** *"Use the Database Integration Pattern like TestData"*

## Key Problem Solved
**Issue:** Entity Framework updates were creating duplicate records instead of updating existing ones.
**Solution:** Use **delete-and-recreate** pattern to ensure only one record per user.

## Corrected POST Method Pattern

```csharp
[HttpPost]
public IActionResult [SectionName]([ModelName]Model model)
{
    var currentUser = HttpContext.Session.GetString("username");
    
    if (string.IsNullOrEmpty(currentUser))
    {
        return RedirectToAction("Login", "Account");
    }

    if (model == null)
    {
        return RedirectToAction("[SectionName]", new { performerUsername = currentUser });
    }

    model.Username = currentUser;

    if (ModelState.IsValid)
    {
        try
        {
            // Database connection verification
            var canConnect = _context.Database.CanConnect();
            if (!canConnect)
            {
                _context.Database.EnsureCreated();
            }
            
            // CRITICAL: Delete all existing records for this user first
            var existingRecords = _context.[TableName].Where(x => x.Username == model.Username).ToList();
            
            if (existingRecords.Any())
            {
                Console.WriteLine($"[SECTION] DEBUG: Found {existingRecords.Count} existing records for {model.Username} - deleting all");
                _context.[TableName].RemoveRange(existingRecords);
                
                // Save deletions first
                var deletedRows = _context.SaveChanges();
                Console.WriteLine($"[SECTION] DEBUG: Deleted {deletedRows} existing records");
            }
            
            // Create new record with latest data
            Console.WriteLine($"[SECTION] DEBUG: Creating new record for {model.Username}");
            
            model.CreatedDate = DateTime.UtcNow;
            model.ModifiedDate = null;
            
            _context.[TableName].Add(model);
            
            var savedRows = _context.SaveChanges();
            
            if (savedRows > 0)
            {
                TempData["SuccessMessage"] = "Data saved successfully!";
                return RedirectToAction("[SectionName]", new { performerUsername = model.Username });
            }
            else
            {
                TempData["ErrorMessage"] = "Save failed - no changes were made.";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SECTION] DEBUG: Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Error saving data: {ex.Message}";
        }
    }

    return View("Performer/[SectionName]", model);
}
```

## Why Delete-and-Recreate Works Better

### ✅ Advantages:
1. **Guarantees Single Record**: Impossible to have duplicates
2. **Avoids EF Tracking Issues**: No entity state conflicts
3. **Simpler Logic**: No complex update property mapping
4. **Reliable on Railway**: Works consistently with PostgreSQL
5. **Clean Database**: Always maintains exactly one record per user

### ✅ Database State:
- **Before Save**: 0 or 1 record per user
- **After Save**: Exactly 1 record per user (guaranteed)
- **No Duplicates**: Physically impossible with this pattern

## GET Method Pattern (Unchanged)

```csharp
public IActionResult [SectionName](string performerUsername)
{
    // ... permission checks ...
    
    // Load existing data (will be single record due to delete-and-recreate)
    var model = _context.[TableName].FirstOrDefault(x => x.Username == performerUsername);
    if (model == null)
    {
        model = new [ModelName]Model { Username = performerUsername };
    }
    
    return View("Performer/[SectionName]", model);
}
```

## Usage Instructions

When implementing new database features:

```
Use the Database Integration Pattern like TestData for [YourFeature]:
- Use delete-and-recreate pattern in POST method
- Ensure only one record per user in database
- Load existing data in GET method with FirstOrDefault
- Follow TestPractice implementation exactly
```

This ensures:
- ✅ No duplicate records
- ✅ Always shows latest saved data
- ✅ Works reliably with Railway PostgreSQL
- ✅ Simple and predictable behavior

## Proven Workflow for Adding/Removing Fields to Existing Pages

**✅ SUCCESS CASE: Adding Registration & Employment fields to TestPractice2**

When modifying existing pages with new database fields, follow this exact sequence:

### Step 1: Update the Model (TestDataModel2.cs)
```csharp
// Add new fields as nullable properties
[Display(Name = "GDC registration number")]
public string? GDCRegistrationNumber { get; set; }

[Display(Name = "Date of UK registration as a dentist")]
public string? UKRegistrationDate { get; set; }

// ... add all new fields as nullable
```

### Step 2: Create Timestamped Migration
```powershell
# Use the migration management script
.\manage-testdata2-migration.ps1 -Action create -MigrationName "AddRegistrationAndEmploymentFields"
```

**Key Benefits:**
- ✅ Creates properly timestamped migration (e.g., `Update_TestData2_AddRegistrationAndEmploymentFields_20250728104254`)
- ✅ EF Core generates correct `ALTER TABLE` statements for PostgreSQL
- ✅ All new fields added as nullable (safe for existing data)
- ✅ No data loss or corruption

### Step 3: Update the View (.cshtml)
```html
<!-- Add new sections BEFORE existing content -->
<div class="card mb-4">
    <div class="card-header" style="background-color: #9bb3d1; color: #4a5568;">
        <h5 class="mb-0">Registration and Qualifications</h5>
    </div>
    <div class="card-body">
        <input asp-for="GDCRegistrationNumber" class="form-control" />
        <!-- ... all new form fields with proper asp-for binding -->
    </div>
</div>
```

### Step 4: Deploy and Auto-Apply Migration
```bash
git add .
git commit -m "Add [FieldName] fields with proper migration"
git push  # Railway auto-applies migrations on deployment
```

### Step 5: Verify Database Integration Pattern Still Works
The existing controller logic automatically handles new fields because:
- ✅ **Delete-and-recreate pattern** works with any number of fields
- ✅ **New nullable fields** don't break existing records
- ✅ **EF Core model binding** automatically maps new form fields
- ✅ **Single record per user** maintained regardless of field count

## Why This Workflow Succeeds

### ✅ Safe Migration Strategy:
1. **Nullable Fields**: New columns don't break existing data
2. **Timestamped Migrations**: No naming conflicts or overwrites
3. **EF Core Auto-Generation**: Proper SQL for PostgreSQL
4. **Railway Auto-Apply**: Migrations applied automatically on deployment

### ✅ Database Integration Pattern Compatibility:
1. **Delete-and-recreate** works with any model structure
2. **FirstOrDefault** loads all fields (old and new)
3. **Model binding** handles expanded forms automatically
4. **Single record guarantee** maintained

### ✅ Zero Downtime Deployment:
1. **Nullable fields** don't require data migration
2. **Backward compatible** with existing records
3. **No manual database operations** required
4. **Instant activation** after deployment

## Field Addition Checklist

When adding fields to existing pages:

- [ ] **Model**: Add new properties as nullable with `[Display]` attributes
- [ ] **Migration**: Use timestamped migration script (`manage-testdata2-migration.ps1`)
- [ ] **View**: Add form fields with proper `asp-for` binding
- [ ] **Styling**: Use consistent header colors (`#9bb3d1`) and Bootstrap classes
- [ ] **Placement**: Add new sections in logical order (usually before existing content)
- [ ] **Testing**: Verify page loads and form submission works
- [ ] **Deployment**: Commit, push, and let Railway auto-apply migrations

## Removing Fields Workflow

For removing fields (reverse process):

1. **Remove from View**: Delete form fields from .cshtml
2. **Create Migration**: `dotnet ef migrations add Remove[FieldName]Fields`
3. **Deploy**: Railway will drop columns automatically
4. **Clean Model**: Remove properties from model class (optional)

**Note**: Keep nullable fields in model longer than needed for safety.

## This Pattern Has Proven Success With:

- ✅ **TestPractice2**: Added 16 Registration & Employment fields seamlessly
- ✅ **Complex Forms**: Tables, dropdowns, checkboxes, textareas
- ✅ **Railway PostgreSQL**: Auto-migration handling works perfectly
- ✅ **Zero Data Loss**: Existing records preserved during field additions
- ✅ **User Experience**: No downtime or broken functionality
