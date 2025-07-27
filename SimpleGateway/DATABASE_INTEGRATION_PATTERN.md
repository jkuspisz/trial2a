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
