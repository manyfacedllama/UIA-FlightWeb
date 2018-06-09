# UIA-FlightWeb
A Flight Booking Web Application project for Ukraine International Airlines (University Project - Asia Pacific University (APU/APIIT)).

## Features
 * Intuitive Design using Bootstrap & jQuery
 * Create & Manage Customer Profile
 * Book & Manage Several Flight Tickets
 * External Configuration Store - Cloud Design Pattern
 * GeoIP-based Currency
 * Dynamic Seat Picker Implementation using SVG
 
## Getting Started
To deploy & host the web application on Microsoft Azure.

### Database
* Use Entity Framework Code First approach to create the database schema from the existing Entity Data Model.
* Migrate the local database to Azure SQL Database.
* Update the connection string values.

### Publish
* Ensure the database has been migrated to Azure SQL and the web.config has been updated.
* Publish the web application to Microsoft Azure as an App Service (Web App).
* You can implement Traffic Manager to direct and reduce the user load across varying web apps deployed in different regions
* Continuous Deployment through Git Repository has been demonstrated in the video below.

## Built With
* ASP.NET MVC
* ASP.NET Identity System
* Entity Framework
* Reactive Extensions

Video demonstration: https://web.microsoftstream.com/video/508f04dc-3404-4406-8997-73c86944b877
