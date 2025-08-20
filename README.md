# 🌐 HTTP Endpoint Monitor API

A robust and secure RESTful API backend service built with **ASP.NET Core** for monitoring the health and status of HTTP endpoints. This service automatically checks configured URLs at regular intervals, tracks their response statuses, and alerts users via notifications when endpoints exceed a predefined error threshold.

👨‍🏫**Under the Supervision of Parham Alvani**  
🍂***Fall 2022***

[![CSS](https://img.shields.io/badge/CSS-1572B6?logo=css3&logoColor=white)](https://developer.mozilla.org/en-US/docs/Web/CSS)
[![HTML5](https://img.shields.io/badge/HTML5-E34F26?logo=html5&logoColor=white)](https://developer.mozilla.org/en-US/docs/Web/HTML)
[![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=black)](https://developer.mozilla.org/en-US/docs/Web/JavaScript)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)


## ✨ Features

- **🔐 JWT Authentication & Authorization**: Secure user registration and login.
- **📊 Endpoint Monitoring**: Automatically checks configured HTTP endpoints at customizable intervals (e.g., every 30 seconds, 1 minute, 5 minutes).
- **⚠️ Smart Alerting**: Generates alerts for users when their endpoint's failure count exceeds a defined threshold.
- **📈 Detailed Statistics**: Provides daily reports on endpoint health (successful vs. failed requests).
- **🧾 RESTful Design**: Clean and predictable API endpoints following REST conventions.
- **💾 SQLite Database**: Lightweight and easy-to-set-up data persistence using Entity Framework Core.

## 🏗️ Project Structure (Based on the Assignment)

The API is structured around three main entities as specified in the project brief:

1.  **👥 Users**: Manage user accounts and authentication.
2.  **🌐 URLs (Endpoints)**: CRUD operations for endpoints to monitor. Each endpoint has a configurable failure threshold.
3.  **⚠️ Alerts**: Generated automatically when an endpoint's error count surpasses its threshold. Users can view alerts for their endpoints.

## 🚀 Getting Started

### Prerequisites

- **[.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)** or later
- An IDE or code editor (e.g., [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/))

### Installation & Running

1.  **Clone the repository**
    ```bash
    git clone https://github.com/Amirbehnam1009/Http-Endpoint-Monitor.git
    cd Http-Endpoint-Monitor
    ```

2.  **Restore dependencies**
    ```bash
    dotnet restore
    ```

3.  **Run database migrations**
    This will create the SQLite database file (`app.db`) and apply the necessary schema.
    ```bash
    dotnet ef database update
    ```

4.  **Run the application**
    ```bash
    dotnet run
    ```
    The API will start, typically on `http://localhost:5000` or `https://localhost:7000`.

## 📡 API Endpoints Overview

### 🔓 Public Endpoints (No Authentication Required)

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/users/register` | Register a new user. |
| `POST` | `/api/users/login` | Authenticate a user and receive a JWT token. |

### 🔐 Protected Endpoints (Require JWT in `Authorization: Bearer <token>` Header)

#### 👥 User Management
*`(Most user management is public; fetching all users might be admin-only)`*

#### 🌐 Endpoints (URLs)
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/endpoints` | Create a new endpoint to monitor. |
| `GET` | `/api/endpoints` | Get a list of all endpoints for the authenticated user. |
| `GET` | `/api/endpoints/{id}/stats` | Get daily statistics (successful/failed calls) for a specific endpoint. |

#### ⚠️ Alerts
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/endpoints/{id}/alerts` | Get all alerts generated for a specific endpoint. |

## 🧪 Example Usage

### 1. Register a New User
**Request:** `POST /api/users/register`
```json
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "YourSecurePassword123!"
}
```

### 2. Login & Get Token
**Request:** `POST /api/users/login`
```json
{
  "username": "john_doe",
  "password": "YourSecurePassword123!"
}
```
**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```
*Use this token in the `Authorization: Bearer <token>` header for all subsequent requests.*

### 3. Create a New Endpoint to Monitor
**Request:** `POST /api/endpoints`
```json
{
  "url": "https://api.example.com/health",
  "monitoringInterval": "00:01:00", // Check every 1 minute
  "failureThreshold": 5 // Generate an alert after 5 consecutive failures
}
```

## 🗄️ Database Schema (Simplified)

- **Users**: `Id`, `Username`, `Email`, `PasswordHash`, `PasswordSalt`
- **Endpoints**: `Id`, `Url`, `MonitoringInterval`, `FailureThreshold`, `UserId` (FK)
- **Requests**: `Id`, `EndpointId` (FK), `StatusCode`, `Timestamp`, `WasSuccessful`
- **Alerts**: `Id`, `EndpointId` (FK), `Message`, `TriggeredOn`, `IsActive`

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 7.0
- **Authentication**: JWT (JSON Web Tokens)
- **ORM**: Entity Framework Core
- **Database**: SQLite
- **Automated Monitoring**: Background services with `IHostedService`

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.


``` bash
dotnet run --project HttpEndpointMonitor.API
```
