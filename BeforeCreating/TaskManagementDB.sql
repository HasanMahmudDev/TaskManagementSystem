/*==========================================================
  TASK MANAGEMENT SYSTEM WITH LOGIN & PAGE PERMISSION
  Author : Hasan Mahmud
  Login  : hasan / hasan123
==========================================================*/

-- ======================
-- DATABASE
-- ======================
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'TaskManagementDB')
BEGIN
    ALTER DATABASE TaskManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TaskManagementDB;
END
GO
CREATE DATABASE TaskManagementDB;
GO
USE TaskManagementDB;
GO

-- ======================
-- ROLE
-- ======================
CREATE TABLE UserRole (
    RoleID INT IDENTITY PRIMARY KEY,
    RoleName NVARCHAR(50) UNIQUE NOT NULL
);
GO

-- ======================
-- USER (LOGIN MASTER)
-- ======================
CREATE TABLE UserInfo (
    UserID INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    RoleID INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RoleID) REFERENCES UserRole(RoleID)
);
GO

-- ======================
-- CLIENT PROFILE
-- ======================
CREATE TABLE ClientProfile (
    ClientID INT IDENTITY PRIMARY KEY,
    UserID INT UNIQUE NOT NULL,
    Company NVARCHAR(100),
    Contact NVARCHAR(50),
    FOREIGN KEY (UserID) REFERENCES UserInfo(UserID)
);
GO

-- ======================
-- PAGE MASTER
-- ======================
CREATE TABLE AppPage (
    PageID INT IDENTITY PRIMARY KEY,
    PageName NVARCHAR(100) UNIQUE NOT NULL,
    PageUrl NVARCHAR(200) UNIQUE NOT NULL
);
GO

-- ======================
-- ROLE → PAGE PERMISSION
-- ======================
CREATE TABLE RolePagePermission (
    PermissionID INT IDENTITY PRIMARY KEY,
    RoleID INT NOT NULL,
    PageID INT NOT NULL,
    CanView BIT DEFAULT 0,
    CanCreate BIT DEFAULT 0,
    CanEdit BIT DEFAULT 0,
    CanDelete BIT DEFAULT 0,
    FOREIGN KEY (RoleID) REFERENCES UserRole(RoleID),
    FOREIGN KEY (PageID) REFERENCES AppPage(PageID),
    CONSTRAINT UQ_Role_Page UNIQUE (RoleID, PageID)
);
GO

-- ======================
-- TASK CATEGORY
-- ======================
CREATE TABLE TaskCategory (
    CategoryID INT IDENTITY PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    CreatedBy INT NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES UserInfo(UserID)
);
GO

-- ======================
-- TASK / PROJECT
-- ======================
CREATE TABLE TaskInfo (
    TaskID INT IDENTITY PRIMARY KEY,
    TaskName NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(MAX),
    CategoryID INT NOT NULL,
    StartDate DATETIME,
    EndDate DATETIME,
    ClientUserID INT NOT NULL,
    CreatedBy INT NOT NULL,
    AssignedManagerID INT NOT NULL,
    [Status] NVARCHAR(50),
    FOREIGN KEY (CategoryID) REFERENCES TaskCategory(CategoryID),
    FOREIGN KEY (ClientUserID) REFERENCES UserInfo(UserID),
    FOREIGN KEY (CreatedBy) REFERENCES UserInfo(UserID),
    FOREIGN KEY (AssignedManagerID) REFERENCES UserInfo(UserID)
);
GO

-- ======================
-- TASK UPDATE
-- ======================
CREATE TABLE TaskUpdate (
    UpdateID INT IDENTITY PRIMARY KEY,
    TaskID INT NOT NULL,
    UpdatedBy INT NOT NULL,
    UpdateInfo NVARCHAR(MAX) NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TaskID) REFERENCES TaskInfo(TaskID) ON DELETE CASCADE,
    FOREIGN KEY (UpdatedBy) REFERENCES UserInfo(UserID)
);
GO

-- ======================
-- SEED DATA
-- ======================

-- Roles
INSERT INTO UserRole (RoleName)
VALUES ('Admin'), ('Employee'), ('Client'), ('Manager');
GO

-- Users (hasan / hasan123)
INSERT INTO UserInfo (FullName, Email, Username, [Password], RoleID)
VALUES
('Hasan Mahmud','admin@hasan.com','hasan','hasan123',1),
('Hasan Mahmud','employee@hasan.com','hasan.emp','hasan123',2),
('Hasan Mahmud','client@hasan.com','hasan.client','hasan123',3),
('Hasan Mahmud','manager@hasan.com','hasan.manager','hasan123',4);
GO

-- Client Profile
INSERT INTO ClientProfile (UserID, Company, Contact)
VALUES (3,'Hasan Tech Ltd','01700000000');
GO

-- Pages
INSERT INTO AppPage (PageName, PageUrl)
VALUES
('Dashboard','/dashboard'),
('Project Create','/project/create'),
('Project List','/project/list'),
('Task Board','/task/board'),
('Task Update','/task/update'),
('User Management','/admin/users'),
('Reports','/reports');
GO

-- Admin Full Permission
INSERT INTO RolePagePermission (RoleID, PageID, CanView, CanCreate, CanEdit, CanDelete)
SELECT 1, PageID, 1,1,1,1 FROM AppPage;
GO

-- Manager Permission
INSERT INTO RolePagePermission (RoleID, PageID, CanView, CanCreate, CanEdit)
SELECT 4, PageID, 1,1,1 FROM AppPage
WHERE PageName IN ('Dashboard','Project List','Task Board','Task Update','Reports');
GO

-- Employee Permission
INSERT INTO RolePagePermission (RoleID, PageID, CanView)
SELECT 2, PageID, 1 FROM AppPage
WHERE PageName IN ('Dashboard','Task Board','Task Update');
GO

-- Client Permission
INSERT INTO RolePagePermission (RoleID, PageID, CanView)
SELECT 3, PageID, 1 FROM AppPage
WHERE PageName IN ('Dashboard','Project List','Reports');
GO

-- Category
INSERT INTO TaskCategory (CategoryName, CreatedBy)
VALUES ('Development',1);
GO

-- Task
INSERT INTO TaskInfo
(TaskName,[Description],CategoryID,StartDate,EndDate,ClientUserID,CreatedBy,AssignedManagerID,[Status])
VALUES
('RBAC Task Management System',
 'Complete login, role and page permission system',
 1,'2023-10-01','2023-10-31',3,1,4,'In Progress');
GO

-- Task Update
INSERT INTO TaskUpdate (TaskID,UpdatedBy,UpdateInfo)
VALUES (1,2,'Initial system design completed');
GO

PRINT '✅ TASK MANAGEMENT SYSTEM CREATED SUCCESSFULLY';
GO
