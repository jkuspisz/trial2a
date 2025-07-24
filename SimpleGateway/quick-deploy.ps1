# Quick Deploy Script for Ongoing Development
# Use this when you've made changes and want to update Azure quickly

param(
    [string]$Message = "Updated healthcare platform features"
)

Write-Host "🏥 Quick Deploy - Healthcare Platform Update" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "📝 Update: $Message" -ForegroundColor Yellow
Write-Host ""

# Variables
$resourceGroup = "SimpleGateway-RG"
$appName = "simplegateway-healthcare"

# Step 1: Quick build check
Write-Host "🔧 Building application..." -ForegroundColor Yellow
$buildResult = dotnet build --configuration Release --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed! Fix errors before deploying." -ForegroundColor Red
    exit 1
}
Write-Host "✅ Build successful" -ForegroundColor Green

# Step 2: Run any pending migrations (if you've added database changes)
Write-Host "🗄️ Checking for database updates..." -ForegroundColor Yellow
# Note: Migrations will run automatically on Azure, but you can test locally first
# Uncomment next line if you want to update local database first:
# dotnet ef database update

# Step 3: Publish
Write-Host "📦 Publishing application..." -ForegroundColor Yellow
dotnet publish -c Release -o ./publish --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Publish failed!" -ForegroundColor Red
    exit 1
}

# Step 4: Create deployment package
Write-Host "📋 Creating deployment package..." -ForegroundColor Yellow
if (Test-Path "deployment.zip") {
    Remove-Item "deployment.zip" -Force
}
Compress-Archive -Path "./publish/*" -DestinationPath "deployment.zip" -Force

# Step 5: Deploy to Azure
Write-Host "🚀 Deploying to Azure..." -ForegroundColor Yellow
Write-Host "   This will take about 1-2 minutes..." -ForegroundColor Gray

$deployResult = az webapp deployment source config-zip --resource-group $resourceGroup --name $appName --src "deployment.zip" --output none 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Deployment failed! Check Azure CLI connection." -ForegroundColor Red
    Write-Host "   Try running: az login" -ForegroundColor Yellow
    exit 1
}

# Step 6: Verify deployment
Write-Host "✅ Deployment completed!" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 Your updated platform is live at:" -ForegroundColor Cyan
Write-Host "   https://$appName.azurewebsites.net" -ForegroundColor White
Write-Host ""
Write-Host "🔍 Testing deployment..." -ForegroundColor Yellow

# Quick health check
$healthCheck = Invoke-WebRequest -Uri "https://$appName.azurewebsites.net" -Method Head -TimeoutSec 30 -ErrorAction SilentlyContinue
if ($healthCheck.StatusCode -eq 200) {
    Write-Host "✅ Application is responding correctly" -ForegroundColor Green
} else {
    Write-Host "⚠️  Application may still be starting up..." -ForegroundColor Yellow
    Write-Host "   Check the URL in a few minutes" -ForegroundColor Gray
}

# Cleanup
Remove-Item "deployment.zip" -Force -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "./publish" -ErrorAction SilentlyContinue

Write-Host ""
Write-Host "🎉 Quick deploy complete!" -ForegroundColor Green
Write-Host "💡 Tip: Use 'dotnet run' to continue local development" -ForegroundColor Blue

# Optional: Open browser
$openBrowser = Read-Host "Open updated site in browser? (y/n)"
if ($openBrowser -eq "y" -or $openBrowser -eq "Y") {
    Start-Process "https://$appName.azurewebsites.net"
}
