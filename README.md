# 🛒 ECommerceAPI (.NET 8 + MySQL + Azure)

Web API project for managing orders and products in an e-commerce system. Application written in .NET 8 with a clean layer structure (Domain, Application, Infrastructure, API), based on MySQL and deployed automatically to Azure Web App using GitHub Actions.

---

## 📦 Features

### ✅ Products
- Add products to catalog
- Edit and delete products
- Browse product list and details
- Stock management (stock)

### ✅ Orders
- Create empty orders
- Assign products to orders (cart)
- Automatically update cart quantities
- Product availability validation (stock)
- Order updates (with stock return)

### ✅ Swagger UI
- API documentation and testing via /swagger

---

## 🧱 Project structure

```
ECommerceAPI/
├── ECommerce.Domain
│   └── Entities (Order, Product, OrderProduct)
├── ECommerce.Application
│   └── Interfaces (IProductRepository, IOrderRepository)
├── ECommerce.Infrastructure
│   └── Data (AppDbContext)
│   └── Repositories
├── ECommerceAPI
│   └── Controllers
│   └── DTOs
│   └── Program.cs, appsettings.json
```

---

## 🛠️ Technologies used

- .NET 8
- Entity Framework Core
- MySQL (Pomelo)
- Swagger / Swashbuckle
- Azure Web App
- GitHub Actions

---

## 🚀 Automated deployment (CI/CD)

### 🔧 GitHub Actions workflow

- Builds application after each push to `main`
- Deploys to Azure Web App
- Uses `AZURE_WEBAPP_NAME` and `AZURE_PUBLISH_PROFILE` as secrets

```yaml
# .github/workflows/deploy.yml
name: Deploy to Azure

on:
  push:
    branches:
      - main

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.x'

      - name: Publish
        run: dotnet publish -c Release -o publish

      - name: Deploy to Azure WebApp
        uses: azure/webapps-deploy@v2
        with:
          app-name: ${ secrets.AZURE_WEBAPP_NAME }
          publish-profile: ${ secrets.AZURE_PUBLISH_PROFILE }
          package: ./publish
```

---

## 🌐 Access to API

> After deployed, Swagger will be available at:
```
https://<twoja-nazwa>.azurewebsites.net/swagger
```

---

## 🗂️ Example DTO

```csharp
public class AddProductToOrderDto
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
}
```

---

## 🧪 Local Testing

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```


## 🚀 How to clone and run locally

1. **Clone the repository**

```bash
git clone https://github.com/your-username/ECommerceAPI.git
cd ECommerceAPI
```

2. **Create your local database (MySQL)**

- Use MySQL 8.0+
- Create a database named `ecommerce`
- Update your `appsettings.json` with correct connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ecommerce;User=root;Password=yourpassword;"
}
```

3. **Apply migrations**

```bash
dotnet ef database update
```

4. **Run the project**

```bash
dotnet run --project ECommerceAPI
```

5. **Test the API**

Visit:

```
https://localhost:5231/swagger
```

to explore and test endpoints via Swagger UI.

---


## ☁️ How to deploy to Azure Web App via GitHub Actions

### 🔹Automatic deployment with GitHub Actions

1. Go to your Web App in Azure
2. Open **Deployment Center > GitHub**
3. Authorize GitHub and select your repo + branch
4. Azure will auto-generate a GitHub Actions workflow file
5. Or, use this manually created workflow (`.github/workflows/deploy.yml`):

```yaml
name: Deploy to Azure Web App

on:
  push:
    branches: [main]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.x'

      - name: Publish
        run: dotnet publish -c Release -o publish

      - name: Deploy to Azure WebApp
        uses: azure/webapps-deploy@v2
        with:
          app-name: ${{ secrets.AZURE_WEBAPP_NAME }}
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
          package: ./publish
```

6. Go to **GitHub > Repository > Settings > Secrets and variables > Actions**
7. Add two secrets:
   - `AZURE_WEBAPP_NAME`: your app name (e.g. `ecommerce-api-blażej`)
   - `AZURE_PUBLISH_PROFILE`: entire XML from Azure "Get publish profile"

8. Push your code to `main`, and GitHub Actions will handle the deployment 