🏢 MeetingRoomReservation

A project for managing meeting room reservations through a RESTful Web API.

The project was developed in C# using ASP.NET Core and .NET 10. It uses a layered architecture to separate API endpoints, domain logic, infrastructure, and data access.

📌 About the Project

MeetingRoomReservation was created as a backend project focused on meeting room reservation management.

The application is organized into separate modules responsible for API communication, domain models, application logic, and infrastructure.

The solution also includes a dedicated test project for verifying application functionality.

🛠️ Technologies
C#
.NET 10
ASP.NET Core Web API
Entity Framework Core
Microsoft SQL Server
MediatR
AutoMapper
FluentValidation
Swagger / OpenAPI
xUnit / .NET testing tools
Git / GitHub
🧩 Project Structure
MeetingRoomReservation/
│
├── MeetingRoomReservation.Api/            # API endpoints and configuration
├── MeetingRoomReservation.Domain/         # Domain models and business logic
├── MeetingRoomReservation.Infrastructure/ # Infrastructure and data access
├── MeetingRoomReservation.Tests/          # Automated tests
│
├── MeetingRoomReservation.Api.slnx        # Solution file
├── .gitignore
└── .gitattributes

🚀 Getting Started
Clone the repository:
   git clone https://github.com/KamilGretka/MeetingRoomReservation.git

Open the solution in Visual Studio.
Configure the database connection in the application settings.
Build and run the MeetingRoomReservation.Api project.
📖 API Documentation

The project uses Swagger / OpenAPI to provide API documentation and allow testing of available endpoints.

👨‍💻 Author

Kamil Gretka

GitHub: KamilGretka
