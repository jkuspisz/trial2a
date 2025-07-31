# PSQ (Patient Satisfaction Questionnaire) System Implementation Guide

## Overview

The PSQ (Patient Satisfaction Questionnaire) system is a comprehensive patient feedback collection platform designed for dental professionals. It allows practitioners to create shareable questionnaires that patients can anonymously complete to provide structured feedback about their dental care experience.

## System Architecture

### Core Components

1. **PSQ Questionnaires** - Main assessment instances created by practitioners
2. **PSQ Responses** - Anonymous patient feedback submissions
3. **Anonymous Feedback URLs** - Shareable links with unique codes for patients
4. **Results Dashboard** - Aggregated patient satisfaction analysis and reporting

### Database Schema

```sql
-- PSQ Questionnaires Table
CREATE TABLE "PSQQuestionnaires" (
    "Id" SERIAL PRIMARY KEY,
    "PerformerId" INTEGER NOT NULL,
    "Title" TEXT NOT NULL,
    "UniqueCode" TEXT NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("PerformerId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- PSQ Responses Table  
CREATE TABLE "PSQResponses" (
    "Id" SERIAL PRIMARY KEY,
    "PSQQuestionnaireId" INTEGER NOT NULL,
    "SubmittedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    
    -- 12 Patient Satisfaction Questions (PSQ.txt scoring: -1, -2, 3, 4, or 999 for "Not observed")
    "PutMeAtEaseScore" INTEGER,                    -- The Dentist put me at ease
    "TreatedWithDignityScore" INTEGER,            -- Treated me with dignity and respect
    "ListenedToConcernsScore" INTEGER,            -- Listened and responded to my concerns
    "ExplainedTreatmentOptionsScore" INTEGER,     -- Clearly explained treatment options including costs
    "InvolvedInDecisionsScore" INTEGER,           -- Involved me in decisions about my care
    "InvolvedFamilyScore" INTEGER,                -- Involved family/carers appropriately
    "TailoredApproachScore" INTEGER,              -- Tailored approach to meet my needs
    "ExplainedNextStepsScore" INTEGER,            -- Explained what will happen next with treatment
    "ProvidedGuidanceScore" INTEGER,              -- Provided guidance on dental care
    "AllocatedTimeScore" INTEGER,                 -- Allocated right amount of time for treatment
    "WorkedWithTeamScore" INTEGER,                -- Worked well with other team members
    "CanTrustDentistScore" INTEGER,               -- Can trust this dentist with dental care
    
    -- 2 Open-ended text feedback questions
    "DoesWellComment" TEXT,                       -- What dentist does particularly well
    "CouldImproveComment" TEXT,                   -- What dentist could improve upon
    
    FOREIGN KEY ("PSQQuestionnaireId") REFERENCES "PSQQuestionnaires"("Id") ON DELETE CASCADE
);
```

## Patient Satisfaction Framework & Scoring System

The PSQ system evaluates 12 core patient experience areas using the **exact scoring system from PSQ.txt**:

### Scoring System (From PSQ.txt)

**Patient Rating Scale:**
- **-1 - Strongly Disagree**: Very negative patient experience
- **-2 - Disagree**: Negative patient experience  
- **3 - Agree**: Positive patient experience
- **4 - Strongly Agree**: Very positive patient experience

**Observation Status:**
- **"Not observed"** - Separate option when aspect wasn't applicable (stored as 999)

### 12 Patient Experience Questions

1. **The Dentist put me at ease** - Comfort and anxiety management
2. **The Dentist treated me with dignity and respect** - Professional manner
3. **The Dentist listened and responded to my concerns** - Communication responsiveness
4. **The Dentist clearly explained available treatment options including costs** - Information provision
5. **The Dentist involved me as much as I wanted to be in decision about my care** - Shared decision making
6. **The Dentist involved my family/carers appropriately** - Family involvement
7. **The Dentist tailored their approach to meet my needs** - Personalized care
8. **The Dentist explained what will happen next with my treatment** - Treatment planning
9. **The Dentist provided guidance on how to take care of my teeth and gums** - Patient education
10. **The Dentist allocated the right amount of time for my treatment** - Time management
11. **The Dentist worked well with other team members** - Team collaboration
12. **I feel I can trust this dentist with my dental care** - Trust and confidence

### 2 Open-Ended Questions

13. **"Anything you feel this dentist does particularly well?"** - Positive feedback
14. **"Anything you feel this dentist could improve upon?"** - Constructive feedback

## Implementation Components

### Models Created

1. **PSQQuestionnaire.cs** - Main questionnaire entity
2. **PSQResponse.cs** - Patient response entity with 12 scores + 2 text fields
3. **PSQResultsDto.cs** - Results aggregation model
4. **SubmitPSQResponseDto.cs** - Form submission model

### Controller: PSQController.cs

**Key Methods:**
- `Index()` - Main PSQ dashboard with questionnaire creation and results overview
- `CreateQuestionnaire()` - Generate unique PSQ code and shareable URL
- `Feedback(string code)` - Anonymous patient feedback form (GET)
- `Feedback(SubmitPSQResponseDto model)` - Process patient submissions (POST)
- `Results()` - Detailed results analysis dashboard
- `GenerateQRCode()` - QR code generation for patient access

### Views Created

1. **PSQ/Index.cshtml** - Main dashboard with questionnaire creation and overview
2. **PSQ/Feedback.cshtml** - Anonymous patient feedback form with all 12 questions + text fields
3. **PSQ/Results.cshtml** - Comprehensive results dashboard with scores and comments
4. **PSQ/FeedbackSubmitted.cshtml** - Thank you page after patient submission

### Database Integration

- **Migration**: `AddPSQSystem` migration created
- **ApplicationDbContext**: Updated with PSQQuestionnaire and PSQResponse DbSets
- **Entity Configuration**: Proper foreign keys and constraints configured

## Key Features & Workflow

### 1. PSQ Creation Workflow
```csharp
[HttpPost]
public async Task<IActionResult> CreateQuestionnaire(string performerUsername)
{
    // Generate unique 8-character code
    var uniqueCode = GenerateUniqueCode();
    
    var questionnaire = new PSQQuestionnaire {
        PerformerId = user.Id,
        Title = $"PSQ Assessment for {user.Username}",
        UniqueCode = uniqueCode,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };
    
    // Generate shareable URL: /PSQ/Feedback?code=XXXXXXXX
    var feedbackUrl = Url.Action("Feedback", "PSQ", 
        new { code = questionnaire.UniqueCode }, Request.Scheme);
}
```

### 2. Anonymous Patient Feedback Collection
```csharp
[HttpGet]
public async Task<IActionResult> Feedback(string code)
{
    var questionnaire = await _context.PSQQuestionnaires
        .FirstOrDefaultAsync(q => q.UniqueCode == code && q.IsActive);
    
    // Return anonymous form with 12 rating questions + 2 text fields
    return View(new SubmitPSQResponseDto { QuestionnaireCode = code });
}
```

### 3. Patient Response Processing
```csharp
[HttpPost]
public async Task<IActionResult> Feedback(SubmitPSQResponseDto model)
{
    var response = new PSQResponse {
        PSQQuestionnaireId = questionnaire.Id,
        SubmittedAt = DateTime.UtcNow,
        
        // All 12 patient satisfaction scores
        PutMeAtEaseScore = model.PutMeAtEaseScore,
        TreatedWithDignityScore = model.TreatedWithDignityScore,
        // ... all 12 questions ...
        CanTrustDentistScore = model.CanTrustDentistScore,
        
        // Open-ended feedback
        DoesWellComment = model.DoesWellComment?.Trim(),
        CouldImproveComment = model.CouldImproveComment?.Trim()
    };
    
    _context.PSQResponses.Add(response);
    await _context.SaveChangesAsync();
}
```

### 4. Results Analysis & Reporting
```csharp
private async Task<PSQResultsDto> GetPSQResults(int questionnaireId)
{
    var responses = await _context.PSQResponses
        .Where(r => r.PSQQuestionnaireId == questionnaireId)
        .ToListAsync();
    
    // Calculate averages excluding "Not observed" (999) values
    var questionAverages = CalculateQuestionAverages(responses);
    
    // Collect text comments
    var positiveComments = responses
        .Where(r => !string.IsNullOrEmpty(r.DoesWellComment))
        .Select(r => r.DoesWellComment!).ToList();
        
    var improvementComments = responses
        .Where(r => !string.IsNullOrEmpty(r.CouldImproveComment))
        .Select(r => r.CouldImproveComment!).ToList();
}
```

## UI/UX Design Features

### Patient-Friendly Feedback Form
- **Clear Scoring**: -1 (Strongly Disagree) to 4 (Strongly Agree) with color coding
- **Separate "Not Observed"**: Prevents scoring confusion like MSF system had
- **Anonymous Access**: No login required for patients
- **Mobile Responsive**: Works on phones/tablets for easy patient access
- **Progress Indication**: Clear question numbering (1-12)

### Practitioner Dashboard
- **QR Code Generation**: Easy patient access via QR scan
- **Real-time Statistics**: Live response counts and averages
- **Detailed Analytics**: Question-by-question breakdown
- **Comment Categorization**: Positive feedback vs improvement suggestions
- **Shareable URLs**: Easy distribution to patients

### Results Visualization
- **Score Heatmaps**: Color-coded performance indicators
- **Progress Bars**: Visual representation of patient satisfaction levels
- **Comment Cards**: Organized display of patient feedback
- **Export Ready**: Structured data for reports

## Security & Best Practices

### Patient Privacy
- **Complete Anonymity**: No patient identification required or stored
- **Secure URLs**: Unique codes provide access control
- **HTTPS Only**: Encrypted data transmission
- **No Tracking**: No patient IP or device tracking

### Data Integrity
- **Validation**: Form validation prevents invalid submissions
- **Safe Database Operations**: EF Core protects against SQL injection
- **Error Handling**: Graceful failure recovery
- **Audit Trail**: Timestamp all submissions

### Session Management
- **Practitioner Authentication**: Secure access to results
- **Session Validation**: Robust login verification (inherited from MSF fixes)
- **Role-Based Access**: Only authorized users can view specific results

## Deployment Strategy

### EF Core Migration
```bash
# Migration created automatically
dotnet ef migrations add AddPSQSystem --context ApplicationDbContext

# Applied automatically on Railway deployment
dotnet ef database update --context ApplicationDbContext
```

### Railway PostgreSQL
- **Automatic Schema Updates**: Migrations apply on deployment
- **Connection String**: Configured for Railway PostgreSQL service
- **Environment Variables**: Secure database connection management

## Navigation Integration

### Sidebar Integration
The PSQ system integrates seamlessly with the standardized 14-item performer sidebar:

**Position 11**: PSQ Assessment (between MSF and Clinical Note Audit)
- **Active Highlighting**: ViewBag.ActiveSection = "PSQ"
- **Consistent Navigation**: Matches documented sidebar standard
- **Proper URLs**: Controller sets ViewBag.PerformerUsername correctly

## Comparison to MSF System

### Architectural Similarities (Exact Replica)
- **Database Structure**: Same pattern (Questionnaires + Responses tables)
- **Controller Methods**: Identical workflow (Index, CreateQuestionnaire, Feedback, Results)
- **URL Structure**: Same pattern (/PSQ/Feedback?code=XXXXXXXX)
- **QR Code Generation**: Same implementation
- **Session Management**: Same validation patterns
- **Error Handling**: Same robust error handling

### Content Differences (PSQ-Specific)
- **Scoring System**: -1, -2, 3, 4 (from PSQ.txt) vs 1-3 + "Not Observed" (MSF)
- **Questions**: 12 patient experience questions vs 17 competency questions
- **Respondents**: Patients vs Professional colleagues
- **Focus**: Patient satisfaction vs Professional development
- **Text Fields**: 2 open-ended questions vs MSF comment fields

## Testing Checklist

### Functionality Tests
- [ ] PSQ questionnaire creation with unique codes
- [ ] Anonymous patient access via URL and QR code
- [ ] All 12 rating questions submit correctly
- [ ] Open-ended text fields save properly
- [ ] Results dashboard shows accurate averages
- [ ] Comments display correctly categorized
- [ ] "Not observed" responses excluded from averages
- [ ] Navigation sidebar highlights correctly

### Security Tests
- [ ] Anonymous access works without login
- [ ] Invalid codes return proper error pages
- [ ] Practitioner authentication required for results
- [ ] No patient data leakage between questionnaires
- [ ] XSS protection in comment fields
- [ ] CSRF protection on all forms

## Future Enhancements

### Immediate Opportunities
1. **Email Integration** - Send PSQ links to patients via email
2. **SMS Integration** - Text PSQ links to patients
3. **Appointment Integration** - Auto-generate PSQ after appointments
4. **Bulk Analytics** - Compare scores across time periods

### Advanced Features
1. **Patient Demographics** - Optional demographic collection
2. **Multi-Language Support** - PSQ in multiple languages
3. **PDF Reports** - Professional PSQ summary reports
4. **Benchmark Comparisons** - Compare against industry standards

## Summary

The PSQ system provides a complete replica of the MSF architecture, adapted specifically for patient satisfaction feedback in dental practices. Key architectural decisions prioritize patient privacy, ease of use, and comprehensive feedback collection. The system follows the exact PSQ.txt specification with proper scoring, clear questions, and comprehensive results analysis.

**Status**: ✅ Complete implementation ready for deployment
**Database**: ✅ Migration created (AddPSQSystem)
**Views**: ✅ All 4 views created with proper sidebar navigation
**Controller**: ✅ Full PSQController with all MSF-equivalent methods
**Models**: ✅ All 4 models created matching PSQ requirements
**Integration**: ✅ ApplicationDbContext updated, sidebar navigation integrated

**Next Steps**: 
1. Deploy to Railway (migration will auto-apply)
2. Test PSQ questionnaire creation
3. Test anonymous patient feedback submission
4. Verify results dashboard functionality

**Last Updated**: July 31, 2025
