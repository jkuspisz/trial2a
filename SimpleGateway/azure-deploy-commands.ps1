# Azure Deployment Commands - Run these in PowerShell after installing Azure CLI

# 1. Login to Azure
az login

# 2. Create Resource Group
az group create --name SimpleGateway-RG --location "East US"

# 3. Create Azure SQL Server
az sql server create `
  --name simplegateway-sql-server `
  --resource-group SimpleGateway-RG `
  --location "East US" `
  --admin-user sqladmin `
  --admin-password "YourSecurePassword123!"

# 4. Create Azure SQL Database
az sql db create `
  --resource-group SimpleGateway-RG `
  --server simplegateway-sql-server `
  --name SimpleGatewayDb `
  --service-objective Basic

# 5. Configure Firewall (Allow Azure services)
az sql server firewall-rule create `
  --resource-group SimpleGateway-RG `
  --server simplegateway-sql-server `
  --name AllowAzureServices `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0

# 6. Create App Service Plan
az appservice plan create `
  --name SimpleGateway-Plan `
  --resource-group SimpleGateway-RG `
  --sku FREE `
  --is-linux false

# 7. Create Web App
az webapp create `
  --resource-group SimpleGateway-RG `
  --plan SimpleGateway-Plan `
  --name simplegateway-healthcare `
  --runtime "DOTNETCORE|9.0"

# 8. Set Connection String
az webapp config connection-string set `
  --resource-group SimpleGateway-RG `
  --name simplegateway-healthcare `
  --settings DefaultConnection="Server=tcp:simplegateway-sql-server.database.windows.net,1433;Database=SimpleGatewayDb;User ID=sqladmin;Password=YourSecurePassword123!;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;" `
  --connection-string-type SQLAzure

# 9. Deploy the application
dotnet publish -c Release -o ./publish
Compress-Archive -Path ./publish/* -DestinationPath deployment.zip -Force
az webapp deployment source config-zip `
  --resource-group SimpleGateway-RG `
  --name simplegateway-healthcare `
  --src deployment.zip
