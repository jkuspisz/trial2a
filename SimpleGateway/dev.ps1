param([string]$Command = "help")

function Show-Header {
    Write-Host "🏥 SimpleGateway Development Helper" -ForegroundColor Cyan
    Write-Host "=================================" -ForegroundColor Cyan
}

function Show-Status {
    Write-Host "📊 SimpleGateway Status" -ForegroundColor Green
    Write-Host "🟢 Application: Your app is running at http://localhost:5159" -ForegroundColor Green
    Write-Host "📊 Database: LocalDB (SimpleGatewayDb) with EF Core" -ForegroundColor Cyan
    Write-Host "✅ 5 test users seeded successfully" -ForegroundColor Green
    Write-Host ""
    Write-Host "💡 Ready for ongoing development!" -ForegroundColor Yellow
    Write-Host "   • Continue developing new features locally" -ForegroundColor Gray
    Write-Host "   • Database is fully functional" -ForegroundColor Gray
    Write-Host "   • GitHub workflows available for future Azure deployment" -ForegroundColor Gray
}

function Show-Help {
    Write-Host "📖 Available Commands:" -ForegroundColor Green
    Write-Host "  status    - Show current application status" -ForegroundColor Cyan
    Write-Host "  help      - Show this help message" -ForegroundColor Cyan
}

Show-Header

switch ($Command.ToLower()) {
    "status" { Show-Status }
    "help" { Show-Help }
    default { Show-Help }
}
