# Sidebar Navigation Guide

## Overview
This document outlines the standardized sidebar navigation structure used across all performer-related pages in the SimpleGateway application. The sidebar provides consistent navigation between different sections of a performer's dashboard.

## Standard Sidebar Order

The sidebar navigation follows this exact order for all performer pages:

### 1. **Dashboard Overview** 
- **Link**: `/Dashboard/ViewPerformerDashboard?performerUsername={username}`
- **Icon**: `<i class="bi bi-speedometer2"></i>`
- **ViewBag.ActiveSection**: `"Dashboard"`
- **Purpose**: Main dashboard overview for the performer

### 2. **Performer Details**
- **Link**: `/Dashboard/PerformerDetails?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"PerformerDetails"`
- **Purpose**: Basic performer information and details

### 3. **Supporting Dentist Declaration**
- **Link**: `/Dashboard/TestPractice?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"TestPractice"`
- **Purpose**: Supervisor form for editing performer data (6 questions)

### 4. **Previous Experience**
- **Link**: `/Dashboard/TestPractice2?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"TestPractice2"`
- **Purpose**: Performer questions (includes UKWorkExperience and LastPatientTreatment)

### 5. **Uploads**
- **Link**: `/Dashboard/Uploads?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"Uploads"`
- **Purpose**: File upload and document management

### 6. **Structured Conversation**
- **Link**: `/Dashboard/StructuredConversation?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"StructuredConversation"`
- **Purpose**: Structured conversation assessments

### 7. **Agreement Terms**
- **Link**: `/Dashboard/AgreementTerms?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"AgreementTerms"`
- **Purpose**: Terms and agreements management

### 8. **Work Based Assessments**
- **Link**: `/Dashboard/WorkBasedAssessments?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"WorkBasedAssessments"`
- **Purpose**: Work-based assessment tracking

### 9. **CPD**
- **Link**: `/Dashboard/CPD?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"CPD"`
- **Purpose**: Continuing Professional Development tracking

### 10. **MSF Assessment**
- **Link**: `@Url.Action("Index", "MSF", new { performerUsername = ViewBag.PerformerUsername })`
- **Icon**: `<i class="fas fa-comments"></i>`
- **ViewBag.ActiveSection**: `"MSF"`
- **Purpose**: Multi-Source Feedback assessments

### 11. **PSQ**
- **Link**: `/Dashboard/PSQ?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"PSQ"`
- **Purpose**: Patient Satisfaction Questionnaire

### 12. **Clinical Note Audit**
- **Link**: `/Dashboard/ClinicalNoteAudit?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"ClinicalNoteAudit"`
- **Purpose**: Clinical note auditing

### 13. **Help & Guidance**
- **Link**: `/Dashboard/HelpGuidance?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"HelpGuidance"`
- **Purpose**: Help documentation and guidance

### 14. **Final Sign Off**
- **Link**: `/Dashboard/FinalSignOff?performerUsername={username}`
- **Icon**: None
- **ViewBag.ActiveSection**: `"FinalSignOff"`
- **Purpose**: Final assessment sign-off

## Implementation Standards

### HTML Structure
```html
<div class="col-3">
    <ul class="list-group">
        <li class="list-group-item @(ViewBag.ActiveSection == "SectionName" ? "active" : "")">
            <a href="/Dashboard/Action?performerUsername=@ViewBag.PerformerUsername" 
               style="@(ViewBag.ActiveSection == "SectionName" ? "color: white;" : "")">
                <!-- Optional Icon -->
                Section Name
            </a>
        </li>
        <!-- Repeat for each section -->
    </ul>
</div>
```

### Controller Requirements
Every controller action that displays a performer page must set:

```csharp
ViewBag.ActiveSection = "SectionName";  // Matches the section being displayed
ViewBag.PerformerUsername = username;   // The performer's username
ViewBag.PerformerName = displayName;    // The performer's display name (optional)
```

### CSS Classes
- **Active Item**: Gets `active` class and `color: white;` style
- **Regular Item**: No additional classes, default styling
- **Bootstrap**: Uses `list-group` and `list-group-item` classes

## Special Cases

### MSF Controller
- **MSF/Index**: Uses the standard sidebar with `ViewBag.ActiveSection = "MSF"`
- **MSF/Results**: Uses the standard sidebar with `ViewBag.ActiveSection = "MSF"`
- **MSF Links**: Use `@Url.Action("Index", "MSF", new { performerUsername = ViewBag.PerformerUsername })`

### Icons
Only these sections have icons:
- **Dashboard Overview**: `<i class="bi bi-speedometer2"></i>`
- **MSF Assessment**: `<i class="fas fa-comments"></i>`

## Files Using This Sidebar

### Implemented and Standardized:
- `Views/Dashboard/Performer/PerformerDetails.cshtml`
- `Views/Dashboard/Performer/TestPractice.cshtml`
- `Views/Dashboard/Performer/TestPractice2.cshtml`
- `Views/MSF/Index.cshtml`
- `Views/MSF/Results.cshtml`

### Need Standardization:
Any other performer-related views should follow this exact structure and order.

## Maintenance Notes

1. **Order Changes**: If the sidebar order needs to change, update ALL implementing views
2. **New Sections**: Add new sections in the appropriate position and update all views
3. **Consistency**: Always maintain the exact same order across all performer pages
4. **ViewBag**: Ensure controllers set the required ViewBag properties
5. **Testing**: Test navigation between all sections to ensure proper highlighting

## Database Tables Referenced

- **Supporting Dentist Declaration**: Uses `TestData` table (supervisor questions)
- **Previous Experience**: Uses `TestData2` table (performer questions with UKWorkExperience/LastPatientTreatment)
- **MSF**: Uses `MSFQuestionnaires` and `MSFResponses` tables

## Related Documentation

- `DATABASE_INTEGRATION_PATTERN.md` - Database handling patterns
- `MSF_IMPLEMENTATION_SUMMARY.md` - MSF-specific implementation details
- `FIELD_LEVEL_PERMISSIONS_PATTERN.md` - Permission handling for different user roles
