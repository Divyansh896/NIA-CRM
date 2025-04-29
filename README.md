# CRM Web Application – Niagara Industrial Association

A custom-built CRM web application designed for the Niagara Industrial Association (NIA) to manage members, contacts, opportunities, and reports efficiently.

---

## 👥 About Stellar Co.

**Stellar Co.** is a student-led development team formed at **Niagara College** in early 2025. The team was created to tackle real-world software challenges through academic-industry partnerships. Our capstone project focused on designing and building a feature-rich, secure, and scalable **CRM Web Application** for the **Niagara Industrial Association (NIA)**.

This was a **community-sponsored project**, developed in close collaboration with a **real-world client (NIA)**. We engaged with stakeholders to gather requirements, received feedback during sprints, and tailored the solution to their operational needs—bringing real value to a functioning organization.

Stellar Co. combines technical expertise in **ASP.NET Core**, **MVC architecture**, **SQLite**, and **Azure deployment** with agile development practices and effective team collaboration. Each team member contributed across the full software development lifecycle—from requirements gathering to design, development, testing, and deployment.

> This repository reflects our shared commitment to clean code, intuitive UI/UX, and robust backend architecture. We are proud to showcase this project as a demonstration of our technical abilities and collaborative strength.

## 🤝 Collaborators

- [Divyansh](https://github.com/Divyansh896)
- [Elizaveta Lazareva](https://github.com/Eliza707707)
- [Rohit Thakur](https://github.com/rohitthaku-rt)
- [Khushi Vij](https://github.com/Khushivij08)  
- [Tania](https://github.com/Tania2024-design)

---

## 📌 Table of Contents
1. [Introduction](#introduction)  
2. [Project Overview](#project-overview)  
   - Objective  
   - Timeline  
   - Team Members  
3. [Key Features](#key-features)  
4. [Technical Stack](#technical-stack)  
5. [Functional Modules](#functional-modules)  
6. [Security & Compliance](#security--compliance)  
7. [Performance Requirements](#performance-requirements)  
8. [Interface Requirements](#interface-requirements)  
9. [Other Non-functional Attributes](#other-non-functional-attributes)  
10. [Technology Stack](#technology-stack)  
11. [Hosted Link & Login Info](#hosted-link--login-info)  

---

## 🧩 Introduction

The CRM Web Application is a custom-built software system designed to fulfill the unique customer relationship management needs of the Niagara Industrial Association (NIA). This web-based solution simplifies data entry, improves access to member and contact information, and supports the organization's goal of fostering industrial development through better communication and information sharing.

The application includes role-based access, real-time reporting, dashboards, and email notifications to enhance productivity and effectiveness. Developed between January and April 2025.

---

## 📊 Project Overview

### 🎯 Objective
To develop a secure and responsive CRM system for NIA staff to manage members, contacts, opportunities, and strategic operations efficiently.

### ⏳ Timeline
January 2025 – April 2025  
(Requirement gathering → Design → Development → Testing → Deployment)

### 👥 Team Members
- Elizaveta Lazareva  
- Divyansh  
- Rohit Thakur  
- Khushi Vij  
- Tania  

---

## 🛠 Key Features

| Feature                   | Description |
|--------------------------|-------------|
| User Roles & Access Control | Admins and Supervisors with specific permissions |
| Authentication System | ASP.NET Identity for secure logins |
| Data Dashboards | Role-specific visual metrics |
| Module-based CRUD | Full CRUD support for Members, Contacts, Opportunities |
| Dynamic Reporting | Export reports in PDF, CSV |
| Automated Email Alerts | SMTP/Mailtrap integration |
| Relational Database Design | SQLite with normalized schema and EF Core |

---

## ⚙️ Technical Stack

### Frontend:
- Razor Views  
- HTML, CSS, JavaScript  

### Backend:
- C#  
- ASP.NET Core MVC  

### Database:
- SQLite (Development)  
- EF Core (ORM)  

### Authentication:
- ASP.NET Identity  

### Third-party Services:
- Mailtrap (Development Email Testing)  
- SMTP (Email Sending)

---

## 🧩 Functional Modules

### 1. User & Role Management  
Admins can create/edit users, assign roles, and restrict access.

### 2. Member Management  
Manage member companies, their statuses, and strategic data.

### 3. Contact Management  
Track communication history and outreach records.

### 4. Opportunity Tracking  
Track initiatives, assign stages, and link them to members.

### 5. Dashboard View  
Chart-based and tabular summaries for quick insights.

### 6. Reports & Exports  
Filter data dynamically and export reports as PDF/CSV.

### 7. Notification System  
Receive automated alerts for updates, changes, and deadlines.

---

## 🔐 Security & Compliance

- Role-based route protection  
- Input sanitization  
- Passwords stored using hashing  
- Strong data validation rules  

---

## 🚀 Performance Requirements

- **System Requirements**: ASP.NET Core, SQLite-compatible system  
- **Response Time**: ≤ 2 seconds under normal load  
- **Scalability**: Designed to support future growth  
- **Availability**: 99.9% uptime (excluding maintenance)

---

## 💻 Interface Requirements

### User Interfaces
- Graphical: Razor Views + HTML/CSS/JS  
- No CLI  
- Potential API exposure for integrations  

### Hardware Interfaces
- Interacts with server hardware, file systems  

### Communication Interfaces
- HTTP/HTTPS for client-server communication  
- SMTP for email  

### Software Interfaces
- EF Core for ORM  
- Mailtrap and SMTP for emails

---

## 🔧 Other Non-functional Attributes

- **Security**: Role-based access, hashing, sanitization  
- **Binary Compatibility**: ASP.NET Core & SQLite compatible  
- **Reliability**: Backups, testing, fault tolerance  
- **Maintainability**: Well-documented and modular  
- **Portability**: Platform-independent  
- **Extensibility**: Easily expandable  
- **Reusability**: Adaptable for other organizations  
- **Application Compatibility**: Compatible with other systems  

---

## 🧱 Technology Stack

- **Frontend**: MVC (Model-View-Controller)  
- **Database**: SQLite  
- **Hosting**: Azure  
- **Version Control**: GitHub  

---

## 🌐 Hosted Link & Login Info

**🔗 URL**: [https://nia-crm.azurewebsites.net](https://nia-crm.azurewebsites.net)

### Test Users
| Email              | Password   | Role     |
|-------------------|------------|----------|
| admin@outlook.com | Pa55w@rd   | Admin    |
| super@outlook.com | Pa55w@rd   | Supervisor |

---
📄 **Download the full user manual here:**  
[📥 NIA CRM User Manual.pdf](https://github.com/Divyansh896/NIA-CRM/raw/master/wwwroot/docs/NIA%20CRM%20User%20Manual.pdf)

---
> Developed by **Stellar Co.** 🚀
