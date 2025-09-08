# Car Auction API

A backend API for a car auction platform built with ASP.NET Core and PostgreSQL. It handles user authentication, car ad management, bidding, and image uploads. This repository contains only the backend; the frontend is hosted in a separate repository.

## Features
- User authentication (login, registration)
- Car ad creation and management
- Bidding system with validation
- Fixed-price purchases
- Search and filtering of car ads
- Image upload and storage

```markdown
## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 16](https://www.postgresql.org/download/)
- [pgAdmin 4](https://www.pgadmin.org/download/) or another PostgreSQL client (optional)
- [Postman](https://www.postman.com/downloads/) or [cURL](https://curl.se/) for testing API endpoints
- A code editor like [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/car-auction-api.git
   cd car-auction-api
   ```

2. **Install Dependencies**:
   Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. **NuGet Packages**:
   The project depends on the following NuGet packages (included in `CarAuctionApi.csproj`):
   ```xml
   <ItemGroup>
     <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.6" />
     <PackageReference Include="Microsoft.AspNetCore.Http.Features" Version="5.0.17" />
     <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.6" />
     <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.4" />
     <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.6" />
     <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.6">
       <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
       <PrivateAssets>all</PrivateAssets>
     </PackageReference>
     <PackageReference Include="Microsoft.Extensions.Identity.Core" Version="9.0.6" />
     <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.4" />
     <PackageReference Include="Scalar.AspNetCore" Version="2.4.13" />
     <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.12.0" />
   </ItemGroup>
   ```

- **Frontend Client**:
  - The backend requires a running Car Auction Frontend (React with TypeScript) at `http://localhost:3000`.
  - See the [ChatApp Frontend repository](https://github.com/CruxAcrux/car-auction-frontend) for frontend setup.

### Additional Setup
- **PostgreSQL Database**:
  - Create a database named `CarAuctionDb`:
    ```sql
    CREATE DATABASE CarAuctionDb;
    ```
  - Configure with your PostgreSQL username and password.

- **Static Files**:
  - Ensure the `wwwroot/Uploads` folder exists for image storage:
    ```bash
    mkdir wwwroot/Uploads
    ```

## Installation
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/car-auction-api.git
   cd car-auction-api
   ```

2. **Install Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Apply Database Migrations**:
   ```bash
   dotnet ef database update
   ```
   - Sets up the `CarAuctionDb` schema for users, car ads, bids, and images.

4. **Start the Backend**:
   ```bash
   dotnet run
   ```
   - Runs the app at `http://localhost:5064`.

## Configuration
Update `appsettings.json` with the following:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=CarAuctionDb;Username=your_username;Password=your_password"
  },
  "Jwt": {
    "Key": "your_secure_jwt_key_32_chars_long!!",
    "Issuer": "CarAuctionApi",
    "Audience": "CarAuctionApi"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Replace:**
- `your_username`: Your PostgreSQL username
- `your_password`: Your PostgreSQL password
- `your_secure_jwt_key_32_chars_long!!`: A 32-character secret key

The backend serves API at `http://localhost:5064`. Ensure the frontend is configured to connect to this URL.

## Usage
1. **Start the Backend**:
   - Run `dotnet run` to start the server at `http://localhost:5064`.

2. **API Endpoints**:
   - Register: `POST /api/Account/register`
   - Login: `POST /api/Account/login`
   - Create Car Ad: `POST /api/CarAd`
   - Search Car Ads: `POST /api/CarAd/search`
   - Place Bid: `POST /api/Bid`
   - Buy Car: `POST /api/Bid/buy/{carAdId}`
   - Get Brands: `GET /api/CarAd/brands`

3. **Authentication**:
   - Include JWT token in the `Authorization` header for protected endpoints: `Bearer <your_token>`

## Known Issues
- **Image Uploads**: Ensure the `wwwroot/Uploads` directory has proper write permissions
- **Bid Validation**: Minimum bid amounts are validated based on current highest bid
- **Auction End Dates**: Bids are only accepted before the auction end date

  
