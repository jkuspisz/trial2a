# Healthcare Platform Azure Deployment Script
# Run this script to deploy your dental healthcare platform to Azure

Write-Host "🏥 SimpleGateway Healthcare Platform - Azure Deployment" -ForegroundColor Green
Write-Host "================================================================" -ForegroundColor Cyan

# Check if Azure CLI is installed
try {
    $azVersion = az --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Azure CLI is installed" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ Azure CLI not found. Please install it first:" -ForegroundColor Red
    Write-Host "   Download: https://aka.ms/installazurecliwindows" -ForegroundColor Yellow
    Write-Host "   Then run this script again." -ForegroundColor Yellow
    exit 1
}

# Variables
$resourceGroup = "SimpleGateway-RG"
$location = "East US"
$sqlServer = "simplegateway-sql-server"
$database = "SimpleGatewayDb"
$appName = "simplegateway-healthcare"
$appPlan = "SimpleGateway-Plan"
$sqlAdmin = "sqladmin"
$sqlPassword = "YourSecurePassword123!"

Write-Host "🔑 Logging into Azure..." -ForegroundColor Yellow
az login

Write-Host "📦 Creating Resource Group..." -ForegroundColor Yellow
az group create --name $resourceGroup --location $location

Write-Host "🗄️ Creating SQL Server..." -ForegroundColor Yellow
az sql server create `
  --name $sqlServer `
  --resource-group $resourceGroup `
  --location $location `
  --admin-user $sqlAdmin `
  --admin-password $sqlPassword

Write-Host "🗃️ Creating SQL Database..." -ForegroundColor Yellow
az sql db create `
  --resource-group $resourceGroup `
  --server $sqlServer `
  --name $database `
  --service-objective Basic

Write-Host "🔥 Configuring Firewall..." -ForegroundColor Yellow
az sql server firewall-rule create `
  --resource-group $resourceGroup `
  --server $sqlServer `
  --name "AllowAzureServices" `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0

Write-Host "🌐 Creating App Service Plan..." -ForegroundColor Yellow
az appservice plan create `
  --name $appPlan `
  --resource-group $resourceGroup `
  --sku FREE `
  --is-linux false

Write-Host "🚀 Creating Web App..." -ForegroundColor Yellow
az webapp create `
  --resource-group $resourceGroup `
  --plan $appPlan `
  --name $appName `
  --runtime "DOTNETCORE|9.0"

Write-Host "� Setting Connection String..." -ForegroundColor Yellow
$connectionString = "Server=tcp:$sqlServer.database.windows.net,1433;Database=$database;User ID=$sqlAdmin;Password=$sqlPassword;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"
az webapp config connection-string set `
  --resource-group $resourceGroup `
  --name $appName `
  --settings DefaultConnection=$connectionString `
  --connection-string-type SQLAzure

Write-Host "📋 Building Application..." -ForegroundColor Yellow
dotnet clean
dotnet restore
dotnet publish -c Release -o ./publish

Write-Host "📦 Creating Deployment Package..." -ForegroundColor Yellow
if (Test-Path "deployment.zip") {
    Remove-Item "deployment.zip"
}
Compress-Archive -Path "./publish/*" -DestinationPath "deployment.zip" -Force

Write-Host "🚀 Deploying to Azure..." -ForegroundColor Yellow
az webapp deployment source config-zip `
  --resource-group $resourceGroup `
  --name $appName `
  --src "deployment.zip"

Write-Host "✅ Deployment Complete!" -ForegroundColor Green
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "🌐 Your healthcare platform is now live at:" -ForegroundColor Green
Write-Host "   https://$appName.azurewebsites.net" -ForegroundColor Cyan
Write-Host ""
Write-Host "🔑 Test Login Credentials:" -ForegroundColor Yellow
Write-Host "   Username: performer1 | Password: password123" -ForegroundColor White
Write-Host "   Username: admin | Password: admin123" -ForegroundColor White
Write-Host ""
Write-Host "📊 Next Steps:" -ForegroundColor Yellow
Write-Host "   1. Test the application at the URL above" -ForegroundColor White
Write-Host "   2. Set up Application Insights for monitoring" -ForegroundColor White
Write-Host "   3. Configure custom domain (optional)" -ForegroundColor White
Write-Host "   4. Add SSL certificate (optional)" -ForegroundColor White
Write-Host ""
Write-Host "💰 Monthly Cost Estimate:" -ForegroundColor Yellow
Write-Host "   - App Service (Free): $0/month" -ForegroundColor White
Write-Host "   - SQL Database (Basic): ~$5/month" -ForegroundColor White
Write-Host ""

# Open the application in browser
$appUrl = "https://$appName.azurewebsites.net"
Write-Host "🌐 Opening application in browser..." -ForegroundColor Yellow
Start-Process $appUrl

Write-Host "🎉 Your dental healthcare platform is now live on Azure!" -ForegroundColor Green
