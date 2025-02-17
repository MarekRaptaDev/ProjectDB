1. Project Description

The project consists of two parts: a database created on a Microsoft SQL Server and a web application built using ASP.NET technology. The database structure was created using Entity Framework. The project is a very basic implementation of an online store with login and registration functionality using .NET Identity. All subpages are styled with Bootstrap.

2. Database

a) Database Schema

The database schema can be divided into two parts. The first part is the basic structure related to the use of .NET Identity and Entity Framework.


The _EFMigrationsHistory table is responsible for storing the migration history of the database using Entity Framework. The remaining tables visible in the diagram are responsible for storing information about users registered in the application and their roles.

The second part of the database consists of tables created for storing data collected by the application, such as the inventory of available products (without stock levels), an order list linked to the product table via a many-to-many relationship using the OrderProducts table. Additionally, a Logs table was created to store the history of changes in the Products, Orders, and OrderProducts tables. Records from tables tracked by the Logs table are not deleted; instead, special triggers update their deletion date.

b) Functions, Triggers, Views, and Methods Created in the Database

I – TotalPrice Function (not used directly by the application)

This function allows users to calculate the total price of a product within an order or, using the SUM function, calculate the total order value.

II – SeeOrderInfo View (not used directly by the application)

This view provides a more accessible way to review order details, including key information from other tables.

III – CreateOrder Procedure

This procedure creates a new order along with the necessary records in the OrderProducts table. It accepts the delivery address, a list of product IDs with corresponding quantities separated by semicolons, and an output parameter returning the ID of the newly created order. To improve performance, it disables row count return using SET NOCOUNT ON. The procedure declares necessary variables, generates a new @OrderId, and inserts the order into the Orders table. Then, it parses the list of product IDs and quantities, iterates through them using a cursor, and adds records to the OrderProducts table.

IV – Triggers

The database includes triggers for "soft delete" functionality in the Orders, Products, and OrderProducts tables. Instead of deleting records, they are moved to the Logs table, and their deletion date is updated.

2. Description of the .NET Application

a) Additional Installed Packages

The following packages were installed during development, mainly for database handling, login and registration modules, and JSON file processing.

b) Program.cs File and Configuration Files

I – Program.cs File

The Program.cs file contains configuration settings for ASP.NET Identity, session management, and cookies. The session block enables the application to transfer data via cookies.

This block configures the database connection using the default connection string, defined in appsettings.json. The AppDbContext class used here will be discussed later.

Another configuration step is enabling authentication using ASP.NET Identity and setting account and login requirements.

The final part initializes session management, routing, authentication, and authorization.

Upon application startup, a seeding process checks whether an administrator account exists. If not, it creates a default admin user with appropriate roles.

II – appsettings.json File

The only modification in this file is adding the default connection string. The SERVERNAME should be replaced with the database server name, and ProjektDB should be updated if the database name differs.

III – AppDbContext Class

The AppDbContext class handles data models. It creates tables and relationships based on models during migrations and updates. This class is also used for database operations.

IV – SessionExtensions Class

The SessionExtensions class extends ASP.NET Core's ISession, adding helper methods to store and retrieve JSON-formatted objects in the session. These methods are essential for implementing the shopping cart functionality in the online store.

V – _ValidationScriptsPartial.cshtml

This file links to jQuery validation scripts used in login and registration views.

c) Layouts

The main layout remains mostly unchanged, except for removing the "Privacy" link and adding partial elements containing navigation links for database operations. This layout is used on all subpages except for login and registration pages.

The application injects the SignInManager object from Identity to check whether a user is logged in and displays appropriate links accordingly.

d) Data Models

I – Database Table Models

The Users model inherits from IdentityUser, with an additional FullName property.

The Product model defines the Products table structure, including validation attributes, primary key declaration, and default values. It also contains a navigation property for handling the relationship with OrderProducts.

The OrderProducts model includes validation, primary and foreign key declarations, and navigation properties for handling relationships between Products and Orders.

II – Models for View Data Transfer

RegisterViewModel includes validation attributes for user registration.

AllOrdersModel allows sending lists of various object types to views, enabling display based on meaningful names rather than IDs.

ProductsAndCart sends the list of products and cart items to the view.

CartItem represents an item inside the shopping cart.

e) Controllers

I – General Controller Structure

The application uses multiple controllers:

AccountController handles login, registration, and related actions.

HomeController manages the homepage.

LogController retrieves logs for the administrator.

OrdersController manages order processing.

ProductController handles product-related operations.

Each controller uses the [Authorize] attribute to restrict access.

II – AccountController

This controller uses SignInManager and UserManager for user authentication. The login function verifies credentials and redirects the user accordingly. The registration function validates user input, creates a new user, assigns a role, and redirects to the login page.

III – LogController

This controller retrieves and sorts logs from the database but does not allow modifications.

IV – OrdersController

Displays a list of products and cart contents.

Handles order placement by validating products and updating the cart.

Uses stored procedures for adding orders.

Soft deletes orders using dbContext.Database.ExecuteSqlRawAsync, ensuring records are archived rather than permanently deleted.

V – ProductController

Similar to OrdersController, it allows product management, including editing and adding new products.

f) Views

I – Homepage View

Displays different messages depending on the user's authentication status.

II – Log View

Displays logs using a foreach loop.

III – Order Views

AdminOrders.cshtml displays all orders with a delete option for administrators.

SeeAll.cshtml lists available products in a tile-based layout, allowing users to add them to the cart.

The cart section displays selected items, total price, and a form for delivery details.

IV – Product Views

These views do not introduce new functionalities and are not discussed further.

V – Account Views

Use a dedicated layout and validation scripts for user authentication pages.


 
