# LibraryManagementApi

## About the Project

LibraryManagementApi is a book management API built with .NET 8 and SQL Server.  
It supports managing books and categories with secure access via JWT-based authentication and role-based authorization.  
The API features logging with Serilog, easy data transformation with AutoMapper, and global exception handling for robustness.  
Additionally, it supports image uploads for books and includes caching for improved performance.

---

## Technologies and Packages Used

- .NET 8  
- Entity Framework Core (EF Core)  
- AutoMapper  
- Serilog (Logging)  
- Microsoft.AspNetCore.Identity (User and Role Management)  
- JWT Bearer Authentication (Token-based Authentication)  
- SQL Server  

---

## Features

- **CRUD operations:** Create, read, update, and delete books and categories  
- **JWT Authentication:** Users get tokens to securely access the API  
- **Role-Based Authorization:**  
  - `Reader` role can only list and view book details  
  - `Writer` role can add, update, and delete books  
- **Image Upload:** Upload and manage images related to books  
- **Caching:** Improves performance on book listing and details endpoints  
- **Serilog Logging:** Logs important events and errors within the API  
- **Global Error Handling:** Catches all unexpected errors, logs them, and returns meaningful responses  
- **Swagger:** API documentation and testing interface  

---

## Running the Project

1. **Prerequisites:**  
   - .NET 8 SDK installed  
   - SQL Server (local or remote)  

2. **Configuration:**  
   - Update `appsettings.json` with your settings:  
     - `ConnectionStrings:DefaultConnection` — Your SQL Server connection string  
     - `Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key` — JWT token settings  

3. **Apply Database Migrations:**  
   ```bash
   dotnet ef database update

4. **Run:**  
   ```bash
   dotnet run

5. **Access Swagger UI::**  
   Open your browser and navigate to:
   ```bash
   https://localhost:<port>/swagger
Use this interface to explore and test the API endpoints.

---

## API Endpoint Examples

- **Books**  
  - `GET /api/books` — List all books (Reader role)  
  - `GET /api/books/{id}` — Get book details (Reader role)  
  - `POST /api/books` — Create a new book (Writer role)  
  - `PUT /api/books/{id}` — Update a book (Writer role)  
  - `DELETE /api/books/{id}` — Delete a book (Writer role)  

- **Images**  
  - `POST /api/images/upload` — Upload an image related to a book  

---

## Notes

- Serilog is configured to write logs to daily rolling files and the console.  
- AutoMapper simplifies conversion between Entities and DTOs.  
- Global exception handling middleware catches unhandled exceptions and logs them.  
- Identity framework manages users and roles securely.  
- JWT tokens provide secure, role-based API access.  
- Caching is implemented to optimize performance for frequently accessed data.

---

## Contribution

Feel free to open issues or submit pull requests to improve this project.

---

## License

This project is licensed under the MIT License.
