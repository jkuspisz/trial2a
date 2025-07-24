# Azure Deployment Guide for SimpleGateway

## Prerequisites
- Azure account with active subscription
- Azure CLI installed locally
- Healthcare training platform ready for deployment

## Phase 1: Azure Resources Setup

### 1. Create Azure Resource Group
```bash
az group create --name SimpleGateway-RG --location "East US"
```

### 2. Create Azure SQL Database
```bash
# Create SQL Server
az sql server create \
  --name simplegateway-sql-server \
  --resource-group SimpleGateway-RG \
  --location "East US" \
  --admin-user sqladmin \
  --admin-password "YourSecurePassword123!"

# Create Database
az sql db create \
  --resource-group SimpleGateway-RG \
  --server simplegateway-sql-server \
  --name SimpleGatewayDb \
  --service-objective Basic
```

### 3. Create Azure App Service
```bash
# Create App Service Plan
az appservice plan create \
  --name SimpleGateway-Plan \
  --resource-group SimpleGateway-RG \
  --sku FREE \
  --is-linux false

# Create Web App
az webapp create \
  --resource-group SimpleGateway-RG \
  --plan SimpleGateway-Plan \
  --name simplegateway-app \
  --runtime "DOTNETCORE|9.0"
```

### 4. Create Application Insights
```bash
az monitor app-insights component create \
  --app SimpleGateway-AI \
  --location "East US" \
  --resource-group SimpleGateway-RG \
  --application-type web
```

## Phase 2: Database Configuration

### 1. Configure Firewall Rules
```bash
# Allow Azure services
az sql server firewall-rule create \
  --resource-group SimpleGateway-RG \
  --server simplegateway-sql-server \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Allow your local IP (for development)
az sql server firewall-rule create \
  --resource-group SimpleGateway-RG \
  --server simplegateway-sql-server \
  --name AllowLocalIP \
  --start-ip-address YOUR_LOCAL_IP \
  --end-ip-address YOUR_LOCAL_IP
```

### 2. Update Connection String
Update the connection string in Azure App Service configuration:
```
Server=tcp:simplegateway-sql-server.database.windows.net,1433;Database=SimpleGatewayDb;User ID=sqladmin;Password=YourSecurePassword123!;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;
```

## Phase 3: Application Configuration

### 1. Set App Service Configuration
```bash
# Set connection string
az webapp config connection-string set \
  --resource-group SimpleGateway-RG \
  --name simplegateway-app \
  --settings DefaultConnection="Server=tcp:simplegateway-sql-server.database.windows.net,1433;Database=SimpleGatewayDb;User ID=sqladmin;Password=YourSecurePassword123!;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;" \
  --connection-string-type SQLAzure

# Set Application Insights key
az webapp config appsettings set \
  --resource-group SimpleGateway-RG \
  --name simplegateway-app \
  --settings APPINSIGHTS_INSTRUMENTATIONKEY="YOUR_INSTRUMENTATION_KEY"
```

## Phase 4: Deployment

### Option A: Deploy from Local Machine
```bash
# Build and publish
dotnet publish -c Release -o ./publish

# Create deployment package
Compress-Archive -Path ./publish/* -DestinationPath deployment.zip

# Deploy to Azure
az webapp deployment source config-zip \
  --resource-group SimpleGateway-RG \
  --name simplegateway-app \
  --src deployment.zip
```

### Option B: GitHub Actions Deployment
Create `.github/workflows/azure-deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 9.0.x
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --no-restore
      
    - name: Publish
      run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp
      
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'simplegateway-app'
        slot-name: 'production'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ${{env.DOTNET_ROOT}}/myapp
```

## Phase 5: Post-Deployment Configuration

### 1. Enable Logging and Monitoring
```bash
# Enable Application Logging
az webapp log config \
  --resource-group SimpleGateway-RG \
  --name simplegateway-app \
  --application-logging filesystem \
  --level information

# Enable Web Server Logging
az webapp log config \
  --resource-group SimpleGateway-RG \
  --name simplegateway-app \
  --web-server-logging filesystem
```

### 2. Set up Custom Domain (Optional)
```bash
# Add custom domain
az webapp config hostname add \
  --webapp-name simplegateway-app \
  --resource-group SimpleGateway-RG \
  --hostname yourdomain.com
```

## Real-time Error Monitoring

### Application Insights Dashboard
- Navigate to Application Insights in Azure Portal
- Set up alerts for exceptions and performance issues
- Create custom dashboards for healthcare-specific metrics

### Log Streaming
```bash
# Stream live logs
az webapp log tail \
  --resource-group SimpleGateway-RG \
  --name simplegateway-app
```

## Security Considerations

1. **HTTPS Only**: Enable HTTPS-only in App Service settings
2. **IP Restrictions**: Configure IP restrictions if needed
3. **Key Vault**: Store secrets in Azure Key Vault
4. **Managed Identity**: Use managed identity for database access

## Cost Optimization

- Use Basic tier for SQL Database initially
- Monitor usage and scale as needed
- Consider reserved instances for production

## URLs After Deployment
- Application: `https://simplegateway-app.azurewebsites.net`
- Database: `simplegateway-sql-server.database.windows.net`
- Application Insights: Available in Azure Portal

## Troubleshooting
- Check Application Insights for errors
- Use Kudu console for file system access
- Review App Service logs for deployment issues
