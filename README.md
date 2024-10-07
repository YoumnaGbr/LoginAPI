# LoginAPI

A simple ASP.NET Core Web API for user authentication, email verification, password reset, and OTP-based password recovery. This API allows user registration, login, password reset, and email verification.

## Features

- **User Registration**: Allows users to register with a unique username and email.
- **Email Verification**: Sends an OTP for email verification.
- **Login**: Users can log in with their username and password.
- **Forgot Password**: Allows users to reset their password using an OTP sent to their email.
- **Reset Password**: Users can set a new password using the OTP.

## Technologies

- **.NET Core** (ASP.NET Core Web API)
- **Entity Framework Core** (for database operations)
- **ASP.NET Core Identity** (for user management and authentication)
- **SendGrid** (for sending email verification and password reset emails)

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) installed.
- A SQL Server database.
- A [SendGrid](https://sendgrid.com/) account for sending emails.

### Configuration

1. **Clone the Repository**

    ```bash
    git clone https://github.com/yourusername/LoginAPI.git
    cd LoginAPI
    ```

2. **Configure `appsettings.json`**

    Update the `appsettings.json` file with your database connection string and SendGrid API details:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "ConnectionStrings": {
        "DefaultConnection": "Server=.;Database=AuthDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
      },
      "AllowedHosts": "*",
      "SendGrid": {
        "ApiKey": "Your-SendGrid-API-Key-Here",
        "SenderEmail": "noreply@yourdomain.com",
        "SenderName": "LoginApp"
      }
    }
    ```

3. **Set Up the Database**

    Make sure you have your SQL Server instance running, then run the following command to create the database and apply migrations:

    ```bash
    dotnet ef database update
    ```

4. **Configure the Launch Settings**

    The `launchSettings.json` file is pre-configured to set the correct URLs for development:

    ```json
    {
      "$schema": "http://json.schemastore.org/launchsettings.json",
      "iisSettings": {
        "windowsAuthentication": false,
        "anonymousAuthentication": true,
        "iisExpress": {
          "applicationUrl": "http://localhost:17398",
          "sslPort": 44329
        }
      },
      "profiles": {
        "http": {
          "commandName": "Project",
          "dotnetRunMessages": true,
          "launchBrowser": true,
          "launchUrl": "swagger",
          "applicationUrl": "http://localhost:5291",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          }
        },
        "https": {
          "commandName": "Project",
          "dotnetRunMessages": true,
          "launchBrowser": true,
          "launchUrl": "swagger",
          "applicationUrl": "https://localhost:7204;http://localhost:5291",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          }
        },
        "IIS Express": {
          "commandName": "IISExpress",
          "launchBrowser": true,
          "launchUrl": "swagger",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          }
        }
      }
    }
    ```

    - **Development URLs**: The application is set to run on `https://localhost:7204` and `http://localhost:5291`.
    - **IIS Express**: Configured to use SSL on port `44329`.

    Make sure these URLs are correct for your development environment. You can change the ports if they conflict with other services on your machine.

5. **Run the Application**

    Start the application by running:

    ```bash
    dotnet run
    ```

    The API will be available at `https://localhost:7204` or `http://localhost:5291`. Swagger UI will be available for easy exploration of endpoints.

## Overview of API Endpoints

- **User Registration**: Register a new user with a username and email.
- **Email Verification**: Verify email address using an OTP sent during registration.
- **Login**: Authenticate users using their email and password.
- **Forgot Password**: Request an OTP to reset the user's password.
- **Reset Password**: Reset the password using the OTP.

---

## Security

- The API uses **ASP.NET Core Identity** for managing user authentication and password hashing.
- **SendGrid** is used for securely sending email messages.
- **OTPs** (One-Time Passwords) are generated for email verification and password reset. These OTPs are stored temporarily and should be securely managed.

## Future Improvements

- Implement secure token storage for OTPs (e.g., Redis or a database with a short TTL for expiration).
- Add expiration logic for OTPs to enhance security.
- Add frontend support for handling OTP submission and reset forms.
- Implement refresh tokens for better login session management.

## Development Notes

- **API Structure**: 
  - The main logic is handled within the `AccountController`, which includes endpoints for registration, login, email verification, and password reset.
  - **UserManager** and **SignInManager** are used for handling user authentication and registration.
  - A custom `IEmailSender` service is used to abstract email-sending operations via SendGrid.

- **Code Flow**:
  - **Registration**: Creates a new user, generates an OTP, and sends it via email for verification.
  - **Login**: Authenticates the user based on their email and password.
  - **Forgot Password**: Generates an OTP and sends it via email to reset the user's password.
  - **Reset Password**: Validates the OTP and updates the user's password.

## Contributing

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Push to the branch.
5. Create a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [SendGrid Documentation](https://docs.sendgrid.com/)

---

### Get in Touch

If you have any questions, feedback, or just want to say hi, feel free to reach out! I'd love to hear from you.

📧 Email: youmna.h.gabr@gmail.com
