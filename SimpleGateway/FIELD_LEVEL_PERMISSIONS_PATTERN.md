# Field-Level Permissions Pattern

## Overview

This document describes the **Field-Level Permissions Pattern** - a sophisticated access control system that allows different user roles to have granular edit permissions for specific fields within the same form/view. This pattern was successfully implemented for TestPractice2 and can be replicated for other pages requiring similar multi-role field access control.

## Business Requirements

The pattern addresses scenarios where:
- **Different user roles** need to edit **different sections** of the same form
- **Data integrity** must be maintained when multiple roles interact with the same record
- **User experience** should clearly indicate what fields each role can modify
- **Security** must be enforced both client-side and server-side

## Pattern Components

### 1. Controller Permission Setup

```csharp
// Example: TestPractice2 permissions
public IActionResult TestPractice2(string performerUsername)
{
    var currentUser = HttpContext.Session.GetString("username");
    var currentRole = HttpContext.Session.GetString("role");
    
    // Define granular permissions for different field groups
    ViewBag.CanEditMainFields = (currentRole == "performer" && currentUser == performerUsername);
    ViewBag.CanEditAdvisorComment = (currentRole == "advisor" || currentRole == "admin" || currentRole == "superuser");
    
    // Other standard ViewBag properties...
    ViewBag.CurrentUserRole = currentRole;
    ViewBag.PerformerUsername = performerUsername;
}
```

### 2. View Implementation

#### Form Fields with Conditional Readonly/Disabled Attributes

```html
<!-- Main form fields - editable by performers only -->
<input asp-for="GDCRegistrationNumber" class="form-control" readonly="@(!ViewBag.CanEditMainFields)" />

<!-- Select fields use disabled instead of readonly -->
<select asp-for="JobType" class="form-control" disabled="@(!ViewBag.CanEditMainFields)">
    <option value="">Select...</option>
    <option value="NHS">NHS</option>
</select>

<!-- Checkboxes use disabled -->
<input type="checkbox" asp-for="AdultPatients" class="form-check-input" disabled="@(!ViewBag.CanEditMainFields)" />

<!-- Textareas -->
<textarea asp-for="WorkExperience" class="form-control" readonly="@(!ViewBag.CanEditMainFields)"></textarea>
```

#### Special Permission Fields

```html
<!-- AdvisorComment section - editable by advisors only -->
@if (ViewBag.CurrentUserRole == "advisor" || ViewBag.CurrentUserRole == "admin")
{
    <div class="mb-3">
        <textarea asp-for="AdvisorComment" class="form-control" rows="4" 
                  placeholder="Enter comments for the performer..."></textarea>
    </div>
}
else
{
    <div class="alert alert-info">
        @if (!string.IsNullOrEmpty(Model.AdvisorComment))
        {
            @Html.Raw(Model.AdvisorComment.Replace("\n", "<br/>"))
        }
        else
        {
            <em>No advisor comments yet.</em>
        }
    </div>
}
```

#### Conditional Submit Button

```html
<div class="mt-4">
    @if (ViewBag.CanEditMainFields || ViewBag.CanEditAdvisorComment)
    {
        <button type="submit" class="btn btn-primary btn-lg">Save Assessment</button>
    }
    else
    {
        <div class="alert alert-info">
            <i class="bi bi-info-circle"></i> This assessment is view-only. Only authorized users can make changes.
        </div>
    }
</div>
```

### 3. POST Method Field Preservation Logic

```csharp
[HttpPost]
public IActionResult TestPractice2(TestDataModel2 model)
{
    var currentUser = HttpContext.Session.GetString("username");
    var currentRole = HttpContext.Session.GetString("role");
    
    // Define permissions
    var isPerformerEditingOwn = (currentRole == "performer" && model.Username == currentUser);
    var canEditAdvisorComment = (currentRole == "advisor" || currentRole == "admin" || currentRole == "superuser");
    
    // Permission validation
    if (!isPerformerEditingOwn && !canEditAdvisorComment)
    {
        return RedirectToAction("Index");
    }
    
    // Field-level data preservation
    var existingRecords = _context.TestData2.Where(t => t.Username == model.Username).ToList();
    
    if (existingRecords.Any())
    {
        var existingRecord = existingRecords.First();
        
        if (canEditAdvisorComment && !isPerformerEditingOwn)
        {
            // Advisors: Preserve all fields EXCEPT AdvisorComment
            var newAdvisorComment = model.AdvisorComment;
            
            var properties = typeof(TestDataModel2).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.Name != "AdvisorComment" && prop.Name != "Id" && prop.CanWrite)
                {
                    var existingValue = prop.GetValue(existingRecord);
                    prop.SetValue(model, existingValue);
                }
            }
            
            model.AdvisorComment = newAdvisorComment;
        }
        else if (isPerformerEditingOwn)
        {
            // Performers: Preserve AdvisorComment, edit everything else
            model.AdvisorComment = existingRecord.AdvisorComment;
        }
    }
    
    // Continue with standard database operations...
}
```

## Implementation Checklist

### ✅ Controller Setup
- [ ] Define `ViewBag` properties for each field group permission
- [ ] Set permissions based on user role and relationship to data
- [ ] Ensure all necessary ViewBag properties are set for the view

### ✅ View Implementation  
- [ ] Add `readonly="@(!ViewBag.CanEditFieldGroup)"` to input/textarea fields
- [ ] Add `disabled="@(!ViewBag.CanEditFieldGroup)"` to select/checkbox fields
- [ ] Implement conditional rendering for special permission sections
- [ ] Add conditional submit button with informative messages
- [ ] Test UI behavior for all user roles

### ✅ POST Method Security
- [ ] Validate permissions at the start of POST method
- [ ] Implement field-level data preservation using reflection
- [ ] Handle different permission combinations (advisor-only, performer-only, etc.)
- [ ] Ensure unauthorized users cannot submit forms
- [ ] Test server-side enforcement for all scenarios

### ✅ Testing & Validation
- [ ] Test each user role's access to appropriate fields
- [ ] Verify readonly/disabled fields cannot be manipulated client-side
- [ ] Confirm data preservation works correctly for all permission combinations
- [ ] Validate that unauthorized submissions are blocked
- [ ] Test edge cases (missing data, new records, etc.)

## Permission Patterns

### Pattern 1: Owner-Edit + Supervisor-Comment
**Use Case**: Forms where data owners fill out main content, supervisors add oversight comments

**Roles**:
- **Data Owner** (e.g., performer): Edits main fields, cannot edit supervisor comments
- **Supervisor** (e.g., advisor): Cannot edit main fields, can edit comment fields
- **Others**: View-only access

**Implementation**: TestPractice2 example above

### Pattern 2: Hierarchical Edit Access
**Use Case**: Forms where higher roles can edit everything lower roles can edit, plus additional fields

**Roles**:
- **Base User**: Can edit basic fields only
- **Supervisor**: Can edit basic fields + supervisor-specific fields  
- **Admin**: Can edit all fields

```csharp
ViewBag.CanEditBasicFields = (currentRole == "user" || currentRole == "supervisor" || currentRole == "admin");
ViewBag.CanEditSupervisorFields = (currentRole == "supervisor" || currentRole == "admin");
ViewBag.CanEditAdminFields = (currentRole == "admin");
```

### Pattern 3: Department-Based Field Access
**Use Case**: Forms where different departments edit different sections

**Roles**:
- **HR Department**: Can edit personal information fields
- **Finance Department**: Can edit salary/budget fields
- **Manager**: Can edit performance review fields

```csharp
ViewBag.CanEditPersonalInfo = (currentDepartment == "HR" || currentRole == "admin");
ViewBag.CanEditFinancialInfo = (currentDepartment == "Finance" || currentRole == "admin");
ViewBag.CanEditPerformanceInfo = (currentRole == "manager" || currentRole == "admin");
```

## Security Considerations

### Client-Side Protection
- Use `readonly` and `disabled` attributes to prevent casual editing
- Provide clear visual indicators of field permissions
- Show informative messages about access restrictions

### Server-Side Enforcement
- **Always validate permissions** in POST methods regardless of client-side restrictions
- Use **reflection-based field preservation** to surgically protect unauthorized fields
- **Log permission violations** for security monitoring
- **Return appropriate error responses** for unauthorized access attempts

### Data Integrity
- **Preserve existing field values** when users don't have permission to edit them
- **Use atomic transactions** when updating records with field-level permissions
- **Implement audit trails** for tracking who changed which fields
- **Validate business rules** after applying field-level changes

## Benefits

### 🎯 **Precise Access Control**
- Granular permissions at the field level
- Role-based access without data duplication
- Flexible permission combinations

### 🛡️ **Enhanced Security**
- Server-side enforcement prevents bypassing client restrictions
- Field-level data preservation prevents unauthorized overwrites
- Clear separation of concerns between user roles

### 👥 **Improved User Experience**
- Clear visual indicators of editable vs readonly fields
- Contextual messages explaining access restrictions
- Intuitive workflow based on user role

### 🔧 **Maintainable Architecture**
- Reusable pattern across multiple forms
- Centralized permission logic in controllers
- Consistent implementation approach

## Future Enhancements

### Advanced Features
- **Time-based permissions**: Fields editable only during certain periods
- **Conditional permissions**: Field access based on form state or other field values
- **Approval workflows**: Multi-stage editing with different permissions per stage
- **Field-level audit trails**: Track which user changed which specific fields

### Integration Opportunities
- **Role management system**: Dynamic permission assignment
- **Workflow engine**: Automated permission changes based on process state
- **API permissions**: Extend pattern to REST API endpoints
- **Reporting**: Permission-aware data export and reporting

## Conclusion

The Field-Level Permissions Pattern provides a robust, scalable solution for implementing granular access control within forms. It balances security, usability, and maintainability while providing the flexibility needed for complex multi-role scenarios.

This pattern can be adapted and extended for various use cases, making it a valuable tool for building sophisticated access control systems in web applications.

---

**Last Updated**: July 28, 2025  
**Pattern Status**: Production-Ready ✅  
**Reference Implementation**: TestPractice2 feature
