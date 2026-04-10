# 🚀 Student Management System (ASP.NET Core Web API)

## 📌 Project Overview

This project is a **Student Management System** built using **ASP.NET Core Web API**.
It provides secure and scalable APIs to manage student data with proper architecture and best practices.

---

## 🛠️ Tech Stack

* ASP.NET Core Web API
* SQL Server
* Entity Framework Core
* JWT Authentication
* Serilog (Logging)
* Swagger (API Documentation)

---

## 📂 Project Architecture

This project follows **Layered Architecture**:

* **Controllers** → Handle HTTP requests
* **Services** → Business logic
* **Repositories** → Data access layer
* **Models** → Data structures
* **Middleware** → Global exception handling

---

## 🔐 Features

✔ JWT Authentication (Secure APIs)
✔ CRUD Operations on Students
✔ Global Exception Handling
✔ Logging using Serilog
✔ Swagger UI for API testing

---

## 📊 Database Schema

### Student Table

| Field       | Type     |
| ----------- | -------- |
| Id          | int      |
| Name        | string   |
| Email       | string   |
| Age         | int      |
| Course      | string   |
| CreatedDate | datetime |

---

## 📡 API Endpoints

### 🔹 Authentication

* `POST /api/auth/login` → Generate JWT Token

### 🔹 Student APIs

* `GET /api/students` → Get all students
* `GET /api/students/{id}` → Get student by ID
* `POST /api/students` → Add student
* `PUT /api/students/{id}` → Update student
* `DELETE /api/students/{id}` → Delete student

---

## ▶️ How to Run Project

### 1️⃣ Clone Repository

```bash
git clone https://github.com/divyanshsingh31422/Zest-India-Assignment-2-.git
cd Zest-India-Assignment-2-
```

### 2️⃣ Setup Database

* Open SQL Server
* Create database
* Update connection string in `appsettings.json`

### 3️⃣ Run Backend

```bash
dotnet restore
dotnet run
```

---

## 🧪 Swagger Testing

After running project, open:

```
http://localhost:5000/swagger
```

---

## 🔐 JWT Usage

1. Login using `/api/auth/login`
2. Copy token
3. Add in Swagger:

```
Authorization: Bearer <your_token>
```

---

## 📌 Output

✔ Fully functional APIs
✔ Secure endpoints
✔ Clean code structure

---

## ⭐ Bonus Features (if implemented)

* Docker support
* React UI
* Unit Testing

---

## 👨‍💻 Author

**Divyansh Singh**

---

## 📎 Submission

GitHub Repository:
👉 https://github.com/divyanshsingh31422/Zest-India-Assignment-2-.git
