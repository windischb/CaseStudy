# Case Study

## Task 
REST API with .NET core to manage (create, update, retrieve) vendor master data.  
The REST API should handle the following data objects with attributes: 

Vendor: 
* Name 
* Name2 
* Address1 
* Address2 
* ZIP 
* Country 
* City 
* Mail 
* Phone 
* Notes

Bank Account 
* IBAN 
* BIC 
* Name

Contact Person 
* First name 
* Last name 
* Phone 
* Mail

The API should check if the user is authenticated and has the permission to manage the data.

Data can be stored in a backend of your choice (for example sqlite).  
The service should use a cache like redis.  


## Implementation

Even if this is a very basic and small Task (and because i really like **Clean Architecture**), this project is structured following **Clean Architecture** principles. While this approach might feel like an over-engineering for the current requirements, i see it as a starting point which establishes a solid foundation for a more complex solution.

### Architecture & Design Decisions

- **Clean Architecture**:  
    The solution is divided into clearly separated layers - Domain, Application, Infrastructure, and Presentation (Api). This separation of concerns makes the system easier to maintain and extend.
    
- **Database & ORM**:
    
    - **PostgreSQL with EF Core**: Chosen for robust data management and ease of migrations.
    - **Schema Separation**:
        - **public**: Stores application-specific data (vendors, bank accounts, and contact persons).
        - **auth**: Dedicated to authentication and user-related information, aligning with ASP.NET Core Identity.
- **Authentication & Authorization**:  
    ASP.NET Core Identity secures the API endpoints:
    
    - **All endpoints** require authentication.
    - **GET endpoints** (data retrieval) are accessible to any authenticated user.
    - **POST, PUT, DELETE endpoints** (data manipulation) are restricted to users with the `Administrator` role.
- **Caching**:  
    GET endpoints utilize output caching with **Redis**, ensuring improved performance by serving frequently requested data faster.
- **Logging**:  
**Serilog** is integrated for structured logging, configured to log messages to the console. This setup should be sufficient for the current task. In a production environment, additional sinks (such as file or centralized logging systems) could be easily added as needed.    
    

### Deployment & Automation

- **Containerization with Docker Compose**:  
    A `docker-compose` file is provided in the root directory to spin up all required containers. Whether you use Docker or Podman, the setup process remains straightforward.
    
- **Automatic Database Migrations**:  
    To simplify the "getting started" experience, the database is automatically created and migrated on startup, reducing manual setup efforts.
    

### User Management & Security

- **Preconfigured Users**:  
    Two users are automatically created during setup:
    - **Administrator**:
        - Email: `admin@domain.com`
        - Password: `ABC12abc!`
        - Role: `Administrator`
    - **Regular User**:
        - Email: `user1@domain.com`
        - Password: `ABC12abc!`
        - Role: _(none)_
- **Registration & Role Management**:  
    Users can register new accounts; however, role modification is not available in the current implementation. This means it is not possible to create additional administrators at this time.

### Developer Experience

- **Swagger Integration**:  
    For ease of testing and exploration, Swagger is integrated into the project and can be accessed at:  
    [https://localhost/swagger](https://localhost/swagger)