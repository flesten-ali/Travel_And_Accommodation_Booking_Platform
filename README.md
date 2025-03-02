# Travel and Accommodation Booking Platform

## 🚀 Project Overview

This project is a **RESTful API** for an advanced **Travel and Accommodation Booking Platform** built with **.NET 8**. It provides a seamless experience for users to search, book, and manage accommodations. The system includes authentication, booking management, and an admin panel for managing hotels, rooms, and cities.

## ✨ Features

- ✅ **User Authentication** (JWT-based login & registration)
- ✅ **Hotel & Room Search** with advanced filters
- ✅ **Hotel Details Page** with images (**Cloudinary integration**)
- ✅ **Room Availability & Booking System**
- ✅ **Booking Confirmation & Invoicing**
- ✅ **Admin Panel** for managing hotels, rooms, and cities
- ✅ **API Versioning** (Supports multiple API versions)
- ✅ **Logging & Monitoring** (Serilog integration)
- ✅ **Role-based Access Control** (Admin, Guest)
- ✅ **Swagger API Documentation**
- ✅ **Dockerized for easy deployment**

## 🛠 Tech Stack

| Category             | Technology                                                             |
| -------------------- | ---------------------------------------------------------------------- |
| **Backend**          | .NET 8 (C#), RESTful API                                               |
| **Database**         | SQL Server                                                             |
| **Authentication**   | JWT-based authentication                                               |
| **Cloud Services**   | Cloudinary (for image uploads)                                         |
| **Logging**          | Serilog                                                                |
| **Security**         | Microsoft.AspNetCore.Identity.PasswordHasher<TUser>                    |
| **Tools**            | Swagger (for API documentation)                                        |
| **Architecture**     | Clean Architecture (Application, Domain, Infrastructure, Presentation) |
| **Containerization** | Docker & Docker Compose                                                |

## 📂 Project Structure

```bash
TABP/
├── src/
│   ├── Core/
│   │   ├── TABP.Application/       # Business logic layer
│   │   ├── TABP.Domain/            # Domain entities and interfaces
│   ├── External/
│   │   ├── TABP.Infrastructure/    # Data access and external dependencies
│   │   ├── TABP.Presentation/      # API presentation layer
│   ├── TABP.WebAPI/                # Main API project
│   │   ├── Filters/                # Custom filters for API requests
│   │   ├── Logs/                   # Logging setup and files
│   │   ├── Middlewares/            # Custom middleware components
│   │   ├── appsettings.json        # Application configuration settings
│   │   ├── Dockerfile              # Docker setup for Web API
│   │   ├── Program.cs              # Application entry point
│   │   ├── WebApiDependencyInjection.cs  # Dependency Injection setup
├── docker-compose/
│   ├── .dockerignore               # Files to ignore in Docker builds
│   ├── .env                        # Environment variables (DO NOT COMMIT)
│   ├── docker-compose.yml          # Docker Compose configuration
│   ├── launchSettings.json         # Development launch settings
├── test/                            # Unit and integration tests
├── GitHub Actions/                  # CI/CD pipeline configurations
```

## 📦 Installation & Setup

### Prerequisites

- .NET 8 SDK
- SQL Server
- Docker & Docker Compose
- Visual Studio 2022 / VS Code

### Setup Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/flesten-ali/Travel_And_Accommodation_Booking_Platform.git
   cd Travel_And_Accommodation_Booking_Platform

   ```

2. 🔧 Environment Configuration

To run this project, you need to configure your environment variables.

1. **Create a `.env` file** in the root directory.
2. **Add the following variables** (replace with your actual values):

```ini
# Database Configuration
CONNECTION_STRING="Server=your-server;Database=your-db;User Id=your-user;Password=your-password;TrustServerCertificate=True;"

# JWT Authentication
JWT_SECRET="your-secret-key"
JwtAuthConfig__Issuer="https://localhost:8080"
JwtAuthConfig__Audience="TABP"

# Cloudinary Configuration
CLOUDINARY_CLOUD_NAME="your-cloud-name"
CLOUDINARY_API_KEY="your-api-key"
CLOUDINARY_API_SECRET="your-api-secret"

# SMTP Email Configuration
SMTPConfig__Host="smtp.gmail.com"
SMTPConfig__Port=587
SMTPConfig__From="your-email@gmail.com"
SMTPConfig__Password="your-email-app-password"
```

3.  Run the Application

```bash
docker-compose up --build
```

4. Access the API

- API URL: http://localhost:8080/swagger

## Endpoints

### Amenities

| HTTP Method | Endpoint                              | Description                            |
| ----------- | ------------------------------------- | -------------------------------------- |
| **POST**    | `/api/v{version}/amenities`           | Creates a new amenity.                 |
| **GET**     | `/api/v{version}/amenities/{id:guid}` | Retrieves an amenity by its unique ID. |
| **PUT**     | `/api/v{version}/amenities/{id:guid}` | Updates an existing amenity.           |
| **DELETE**  | `/api/v{version}/amenities/{id:guid}` | Deletes an amenity by its unique ID.   |

### Bookings

| HTTP Method | Endpoint                                          | Description                                             |
| ----------- | ------------------------------------------------- | ------------------------------------------------------- |
| **GET**     | `/api/v{version}/user/bookings/{id:guid}`         | Retrieves booking details for a given ID.               |
| **POST**    | `/api/v{version}/user/bookings`                   | Creates a new hotel booking for a guest.                |
| **GET**     | `/api/v{version}/user/bookings/{id:guid}/invoice` | Generates and retrieves an invoice PDF for the booking. |
| **DELETE**  | `/api/v{version}/user/bookings/{id:guid}`         | Deletes an existing booking by its unique ID.           |

### Cart Items

| HTTP Method | Endpoint                                    | Description                                          |
| ----------- | ------------------------------------------- | ---------------------------------------------------- |
| **POST**    | `/api/v{version}/user/cart-items`           | Adds a new item to the guest's shopping cart.        |
| **DELETE**  | `/api/v{version}/user/cart-items/{id:guid}` | Deletes an existing cart item by its unique ID.      |
| **GET**     | `/api/v{version}/user/cart-items`           | Retrieves a list of cart items for a specific guest. |

### Cities

| HTTP Method | Endpoint                                     | Description                                                    |
| ----------- | -------------------------------------------- | -------------------------------------------------------------- |
| **GET**     | `/api/v{version}/cities/trending-cities`     | Retrieves a list of trending cities based on hotel bookings.   |
| **GET**     | `/api/v{version}/cities/cities-for-admin`    | Retrieves a paginated list of cities for admin users.          |
| **POST**    | `/api/v{version}/cities`                     | Adds a new city to the system.                                 |
| **GET**     | `/api/v{version}/cities/{id:guid}`           | Retrieves details of a specific city by its unique identifier. |
| **DELETE**  | `/api/v{version}/cities/{id:guid}`           | Removes a city from the system.                                |
| **PUT**     | `/api/v{version}/cities/{id:guid}`           | Updates an existing city's details.                            |
| **POST**    | `/api/v{version}/cities/{id:guid}/thumbnail` | Uploads a thumbnail image for a specific city.                 |

### Discounts

## Discounts Endpoints

| HTTP Method | Endpoint                                                                         | Description                                                                            |
| ----------- | -------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------- |
| **POST**    | `/api/v{version:apiVersion}/room-classes/{roomClassId:guid}/discounts`           | Creates a new discount for a specific room class.                                      |
| **GET**     | `/api/v{version:apiVersion}/room-classes/{roomClassId:guid}/discounts/{id:guid}` | Retrieves a discount by its ID for a specific room class.                              |
| **DELETE**  | `/api/v{version:apiVersion}/room-classes/{roomClassId:guid}/discounts/{id:guid}` | Deletes an existing discount for a specific room class.                                |
| **GET**     | `/api/v{version:apiVersion}/room-classes/{roomClassId:guid}/discounts`           | Retrieves a list of discounts for a specific room class with pagination and filtering. |

### Guest

| HTTP Method | Endpoint                                                    | Description                                                         |
| ----------- | ----------------------------------------------------------- | ------------------------------------------------------------------- |
| **GET**     | `/api/v{version:apiVersion}/guests/recently-visited-hotels` | Retrieves a list of hotels recently visited by the specified guest. |

### Hotels

| HTTP Method | Endpoint                                                   | Description                                                                               |
| ----------- | ---------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| **GET**     | `/api/v{version:apiVersion}/hotels/search`                 | Searches for hotels using various filters such as location, price, and amenities.         |
| **GET**     | `/api/v{version:apiVersion}/hotels/{id:guid}/details`      | Retrieves detailed information about a specific hotel by its ID.                          |
| **POST**    | `/api/v{version:apiVersion}/hotels`                        | Creates a new hotel by providing necessary details such as name, address, and facilities. |
| **GET**     | `/api/v{version:apiVersion}/hotels/{id:guid}`              | Retrieves a hotel by its unique ID.                                                       |
| **POST**    | `/api/v{version:apiVersion}/hotels/{id:guid}/thumbnail`    | Uploads a thumbnail image for a specific hotel.                                           |
| **POST**    | `/api/v{version:apiVersion}/hotels/{id:guid}/gallery`      | Uploads images to the gallery for a specific hotel.                                       |
| **GET**     | `/api/v{version:apiVersion}/hotels/featured-deals`         | Retrieves featured hotel deals based on the specified limit.                              |
| **GET**     | `/api/v{version:apiVersion}/hotels/get-for-admin`          | Retrieves hotels for admin management purposes.                                           |
| **PUT**     | `/api/v{version:apiVersion}/hotels/{id:guid}`              | Updates the details of an existing hotel using its unique ID.                             |
| **DELETE**  | `/api/v{version:apiVersion}/hotels/{id:guid}`              | Deletes an existing hotel by its unique ID.                                               |
| **GET**     | `/api/v{version:apiVersion}/hotels/{id:guid}/room-classes` | Retrieves a list of room classes for a specific hotel.                                    |

### Owners

| HTTP Method | Endpoint                                      | Description                                           |
| ----------- | --------------------------------------------- | ----------------------------------------------------- |
| **POST**    | `/api/v{version:apiVersion}/owners`           | Creates a new owner by providing necessary details.   |
| **GET**     | `/api/v{version:apiVersion}/owners/{id:guid}` | Retrieves an owner by their unique identifier.        |
| **DELETE**  | `/api/v{version:apiVersion}/owners/{id:guid}` | Deletes an existing owner by their unique identifier. |

### Reviews

| HTTP Method | Endpoint                                                      | Description                                          |
| ----------- | ------------------------------------------------------------- | ---------------------------------------------------- |
| **GET**     | `/api/v{version:apiVersion}/{hotelId:guid}/reviews`           | Retrieves a list of reviews for the specified hotel. |
| **POST**    | `/api/v{version:apiVersion}/{hotelId:guid}/reviews`           | Creates a new review for the specified hotel.        |
| **GET**     | `/api/v{version:apiVersion}/{hotelId:guid}/reviews/{id:guid}` | Retrieves a review by its unique identifier.         |
| **PUT**     | `/api/v{version:apiVersion}/{hotelId:guid}/reviews/{id:guid}` | Updates the details of an existing review.           |
| **DELETE**  | `/api/v{version:apiVersion}/{hotelId:guid}/reviews/{id:guid}` | Deletes an existing review by its unique identifier. |

### Room Classes

| HTTP Method | Endpoint                                                    | Description                                                   |
| ----------- | ----------------------------------------------------------- | ------------------------------------------------------------- |
| **GET**     | `/api/v{version:apiVersion}/room-classes/get-for-admin`     | Retrieves a list of room classes for administrative purposes. |
| **POST**    | `/api/v{version:apiVersion}/room-classes`                   | Creates a new room class with the provided details.           |
| **GET**     | `/api/v{version:apiVersion}/room-classes/{id:guid}`         | Retrieves a room class by its unique identifier.              |
| **PUT**     | `/api/v{version:apiVersion}/room-classes/{id:guid}`         | Updates an existing room class.                               |
| **DELETE**  | `/api/v{version:apiVersion}/room-classes/{id:guid}`         | Deletes an existing room class by its unique identifier.      |
| **POST**    | `/api/v{version:apiVersion}/room-classes/{id:guid}/gallery` | Uploads gallery images for a room class.                      |

### Rooms

| HTTP Method | Endpoint                                                                    | Description                                                                          |
| ----------- | --------------------------------------------------------------------------- | ------------------------------------------------------------------------------------ |
| **GET**     | `/api/v{version:apiVersion}/room-classes/{roomClassId}/rooms/get-for-admin` | Retrieves a list of rooms for administrative purposes with filtering and pagination. |
| **POST**    | `/api/v{version:apiVersion}/room-classes/{roomClassId}/rooms`               | Creates a new room in a specific room class.                                         |
| **GET**     | `/api/v{version:apiVersion}/room-classes/{roomClassId}/rooms/{id}`          | Retrieves details of a room by its unique ID.                                        |
| **PUT**     | `/api/v{version:apiVersion}/room-classes/{roomClassId}/rooms/{id}`          | Updates an existing room in a room class.                                            |
| **DELETE**  | `/api/v{version:apiVersion}/room-classes/{roomClassId}/rooms/{id}`          | Deletes a room by its unique ID.                                                     |

### Users

| HTTP Method | Endpoint                                          | Description                                             |
| ----------- | ------------------------------------------------- | ------------------------------------------------------- |
| **POST**    | `/api/v{version:apiVersion}/users/login`          | Authenticates a user and generates an access token.     |
| **POST**    | `/api/v{version:apiVersion}/users/register-user`  | Registers a new user with the role of Guest.            |
| **POST**    | `/api/v{version:apiVersion}/users/register-admin` | Registers a new admin user (requires admin privileges). |

## 🔑 Admin Access

To log in as an Admin, use the following credentials:

- Email: admin@example.com
- Password: admin

## 🤝 Contributing

Contributions are welcome! If you’d like to improve this project, follow these steps:

1. Fork the project
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Commit changes (`git commit -m "Added new feature"`)
4. Push to your fork (`git push origin feature/new-feature`)
5. Submit a pull request

## 👨‍💻 Developed By

This project was developed by **Falastin Bawaqna**.

🔗 **Connect with me:**

- GitHub: flesten-ali
- LinkedIn: www.linkedin.com/in/falastin-bawaqna-71178623a
