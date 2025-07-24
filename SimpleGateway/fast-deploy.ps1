# Simple Azure Container Instance Deployment
# This is the fastest way to get your app live on Azure!

Write-Host "🏥 SimpleGateway - Fast Azure Deployment" -ForegroundColor Green
Write-Host "=======================================" -ForegroundColor Cyan

# Build the application first
Write-Host "🔨 Building application..." -ForegroundColor Yellow
dotnet publish -c Release -o ./publish

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful!" -ForegroundColor Green
} else {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

# Create a simple Dockerfile
Write-Host "🐳 Creating Docker configuration..." -ForegroundColor Yellow
@"
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "SimpleGateway.dll"]
"@ | Out-File -FilePath "Dockerfile" -Encoding ASCII

# Create Azure Container Registry
Write-Host "📦 Creating Azure Container Registry..." -ForegroundColor Yellow
$acrName = "simplegatewayacr" + (Get-Random -Maximum 9999)
az acr create --resource-group "SimpleGateway-RG" --name $acrName --sku Basic --admin-enabled true

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Container Registry created: $acrName" -ForegroundColor Green
    
    # Build and push container
    Write-Host "🏗️ Building and pushing container..." -ForegroundColor Yellow
    az acr build --registry $acrName --image "simplegateway:latest" .
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Container built and pushed!" -ForegroundColor Green
        
        # Get ACR credentials
        $acrLoginServer = az acr show --name $acrName --query loginServer --output tsv
        $acrUsername = az acr credential show --name $acrName --query username --output tsv
        $acrPassword = az acr credential show --name $acrName --query passwords[0].value --output tsv
        
        # Deploy to Container Instance
        Write-Host "🚀 Deploying to Azure Container Instance..." -ForegroundColor Yellow
        az container create `
            --resource-group "SimpleGateway-RG" `
            --name "simplegateway-app" `
            --image "$acrLoginServer/simplegateway:latest" `
            --cpu 1 --memory 1.5 `
            --registry-login-server $acrLoginServer `
            --registry-username $acrUsername `
            --registry-password $acrPassword `
            --dns-name-label "simplegateway-healthcare" `
            --ports 80
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "🎉 DEPLOYMENT SUCCESSFUL!" -ForegroundColor Green
            Write-Host "================================" -ForegroundColor Green
            Write-Host ""
            Write-Host "🌐 Your application is now live at:" -ForegroundColor Cyan
            Write-Host "   http://simplegateway-healthcare.eastus.azurecontainer.io" -ForegroundColor Yellow
            Write-Host ""
            Write-Host "📊 Deployment Details:" -ForegroundColor Cyan
            Write-Host "   • Container Registry: $acrName" -ForegroundColor Gray
            Write-Host "   • Container Instance: simplegateway-app" -ForegroundColor Gray
            Write-Host "   • Resource Group: SimpleGateway-RG" -ForegroundColor Gray
            Write-Host ""
            Write-Host "💡 Next Steps:" -ForegroundColor Yellow
            Write-Host "   1. Test your live application" -ForegroundColor Gray
            Write-Host "   2. Set up Azure SQL Database for production" -ForegroundColor Gray
            Write-Host "   3. Configure custom domain (optional)" -ForegroundColor Gray
            
            # Open in browser
            Write-Host "🌐 Opening application in browser..." -ForegroundColor Green
            Start-Process "http://simplegateway-healthcare.eastus.azurecontainer.io"
        }
    }
}

Write-Host ""
Write-Host "🏥 Healthcare Platform deployment complete!" -ForegroundColor Green
