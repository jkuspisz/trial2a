# Azure Portal Deployment Guide

## Step 1: Create Azure SQL Database

1. **Go to Azure Portal**: https://portal.azure.com
2. **Click "Create a resource"**
3. **Search for "SQL Database"**
4. **Fill in details**:
   - **Resource Group**: Create new "SimpleGateway-RG"
   - **Database Name**: SimpleGatewayDb
   - **Server**: Create new server
     - **Server Name**: simplegateway-sql-server
     - **Admin Login**: sqladmin
     - **Password**: YourSecurePassword123!
     - **Location**: East US
   - **Compute + Storage**: Basic (5 DTU, 2GB) - $4.90/month
5. **Click "Review + Create"**

## Step 2: Configure Database Firewall

1. **Go to your SQL Server** (simplegateway-sql-server)
2. **Click "Networking"** in left menu
3. **Check "Allow Azure services and resources to access this server"**
4. **Click "Save"**

## Step 3: Create App Service

1. **Click "Create a resource"**
2. **Search for "Web App"**
3. **Fill in details**:
   - **Resource Group**: SimpleGateway-RG (same as database)
   - **Name**: simplegateway-healthcare
   - **Runtime Stack**: .NET 9 (STS)
   - **Operating System**: Windows
   - **Region**: East US
   - **Pricing Plan**: Free F1 (for testing)
4. **Click "Review + Create"**

## Step 4: Configure Connection String

1. **Go to your Web App** (simplegateway-healthcare)
2. **Click "Configuration"** in left menu
3. **Click "Connection strings"** tab
4. **Click "+ New connection string"**
5. **Fill in**:
   - **Name**: DefaultConnection
   - **Value**: Server=tcp:simplegateway-sql-server.database.windows.net,1433;Database=SimpleGatewayDb;User ID=sqladmin;Password=YourSecurePassword123!;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;
   - **Type**: SQLAzure
6. **Click "Save"**

## Step 5: Deploy Application

### Option A: GitHub Actions (Recommended)
1. **Push your code to GitHub**
2. **In Web App, go to "Deployment Center"**
3. **Choose "GitHub"**
4. **Authorize and select your repository**
5. **Azure will create GitHub Actions workflow automatically**

### Option B: Visual Studio Code
1. **Install Azure App Service extension**
2. **Right-click project folder**
3. **Select "Deploy to Web App"**
4. **Choose your subscription and web app**

### Option C: ZIP Deploy
1. **Build project**: `dotnet publish -c Release -o ./publish`
2. **Create ZIP**: Compress ./publish folder
3. **In Web App, go to "Advanced Tools" > "Go"**
4. **Drag ZIP file to /site/wwwroot in Kudu**

## Step 6: Verify Deployment

1. **Go to your Web App overview**
2. **Click the URL** (https://simplegateway-healthcare.azurewebsites.net)
3. **Test login with**: performer1 / password123
4. **Check database connection is working**

## Step 7: Enable Application Insights (Real-time Monitoring)

1. **In Web App, click "Application Insights"**
2. **Click "Turn on Application Insights"**
3. **Create new or use existing**
4. **Click "Apply"**
5. **Now you have real-time error monitoring!**
