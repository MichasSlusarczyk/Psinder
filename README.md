# Psinder

# Application description 
Psinder is an application that allows you to manage multiple shelters in order to provide the user with a platform to find the perfect dog with the expected characteristics and close to the user's place of residence. The application provides an admin, a shelter employee, a user and a guest panel. 
The main functionalities in the admin panel include: 
- adding and editing shelter data 
- creating user accounts assigned to specific shelters 
- ability to block users 

A shelter employee has the possibility to: 
- add posts with dogs, edit and delete them 
- display all posts with dogs from your shelter
- display appointments scheduled at the shelter they belong to 

A logged in user has the following functionalities: 
- arranging and canceling visits with specific animals 
- editing your account to provide details for your visit

Guests without an account can browse all dogs available within the app and filter the results based on: selected shelter user's whereabouts and dog characteristics. Additionally, the results are sorted according to additional characteristics of the animals - the more the dog meets our expectations, the higher it will be visible on the website. 

Additional functionalities include: 
- creating a user account (with e-mail confirmation of registration) 
- logging in via Google and Facebook account 
- password reminder (with e-mail confirmation) 
- changing the page display language to Polish (excluding the translation of animal descriptions). 

# Start-up instructions from the developer's website: 
### Backend: 
Perform the installation of the Visual Studio 2022 development environment from https://visualstudio.microsoft.com/pl/vs/. To do this, during the installation of the environment itself in the Community version using the installer, choose the ASP.NET and Web Development package, and make sure that the latest version of .NET 7 is installed. Upon the first project launch, it should automatically download all necessary packages using the NuGet Package Manager tool.
Even though the SQL Server database is locally located within the project during the application development process, the Redis tool is still required to run the application. To achieve this, based on the Linux system, which can also be installed as a subsystem for the Windows system, install the Redis tool using the following commands:
*sudo apt-add-repository ppa:redislabs/redis*
*sudo apt-get update*
*sudo apt-get upgrade*
*sudo apt-get install redis-server*

The last step is to run this tool in the background using the command:
*sudo service redis-server start*

In case it is necessary to make changes to the database structure, perform migration using the command in the built-in Visual Studio Package Manager Console, and then update the database:
*Add-Migration InitialCreate -Project Psinder.DB*
*Update-Database -Project Psinder.DB*

### Frontend: 
To run the application frontend, installation of Node.js software is required. Additionally, to manage the front-end part of the application, it may be convenient to use the Visual Studio Code development environment. When opening a project in this environment, open a terminal and then run the npm install command to install the necessary libraries and dependencies for the project. Then, you can launch the application view using the ng serve --ssl true command. After completing the above steps, the application should be available in your browser at https://localhost:4200. 

### API Keys and External Integrations 
To ensure the proper functioning of our application and to enable the use of various external services, it is required to provide API keys and access identifiers. Below you will find information about specific integrations: 
#### Login Via Google 
To enable login using your Google account, please provide an API key from Google. You can obtain an API key by following the instructions available on the Google Developers website. 
To obtain the key you should:
1. Go to the Google Developer console available at https://console.cloud.google.com/ and create a project there. Then, select option APP & Services.
2. In the OAuth Consent Screen select External and create a base for your app. Enter your app name and contact email, and then in App Domain put the frontend address, which is https://localhost:4200
3. In Credentials select option Create Credentials -> OAuth Cient Id. Select that it is a web application. In Authorized Javascript Origins put https://localhost:4200 and https://localhost. In Authorized redirect URL put only https://localhost:4200.
4. After those steps your Client Id and Client Secret are generated. 

#### Login Via Facebook 
To enable login using your Facebook account, please provide your Facebook API access key. More information on obtaining keys is available on Meta for Developers. 
To obtain the key you should:
1. Create an app in the Meta for Developers page. 
2. In the basic settings of the created app put https://localhost:4200 in Website -> Site url.
3. In Use Cases -> Authentication and account creation -> Customize -> Facebook Login -> Settings toggle Login with the JavaScript SDK to YES. In Allowed Domains for the JavaScript SDK put https://localhost:4200
4. If a Sorry, something went wrong message appears after trying to sign in with use of Facebook account, go to your Use Cases -> Authentication and account creation and add email permission.

#### reCAPTCHA 
We use the reCAPTCHA service to protect against bots. To take full advantage of this feature, please provide your reCAPTCHA API key. You can get your API key on the reCAPTCHA website. 
#### App Password for Gmail 
To enable the application to send emails using your Gmail account, we recommend configuring the so-called app password. This is a one-time password created specifically for the application that does not require entering your master password. 
**Note**: The required data is available on the app backend in the appsettings.json file and on the frontend in the environment.ts file. 
**Important**: Providing correct API keys is crucial for the proper functioning of the application. Make sure your keys are configured correctly to avoid integration issues with external services. 
#### Test users:
You can test the application with predefined users for each role.
User:
[user1@gmail.com]
Worker:
[worker1@gmail.com]
Admin:
[admin1@gmail.com]
Password for all test accounts:
**P@ssw0rd123456**
# Usage from the user side: 
#### Installation and commissioning: 
To install the usable version of the application, you must have Docker installed. Having the environment required to run the application in question installed and active, download the docker-compose.yml file available in the repository, and then run the command line in the folder where the downloaded file is located. The next step to run the application is to execute the docker compose up command in the terminal, which creates and launches the defined containers. Execution of this command downloads the appropriate artifacts and launches containers containing the backend and frontend parts of the application in question, as well as the database management system. Once the containers and application are completely built, it is available in the browser at http://localhost:4200. 
# Instructions for using selected user interface elements:
1. Displaying results 
After starting the application, all dogs available in the application are displayed. The form displayed at the top of the page is used to filter the results depending on the selected dog characteristics. Additionally, in the Pet traits tab, more detailed dog traits are available, thanks to which the results are sorted - the more a dog matches the given traits, the higher it is on the list. It is possible to display dogs from a selected shelter available in the application, but you can also share your location - then dogs from all shelters available within 15 km of the user's location will be displayed. After clicking on the dog's photo, its details and the location on the map of the shelter it belongs to are displayed. 
2. Registration 
To register, click the Sign in button on the topbar and then select the Register option. After providing the correct data, an activation email will be sent to the e-mail address provided in the form. After clicking on the link there, your account is activated and you can log in. 
3. Login. 
You can log in by entering the email address and password provided during registration and completing the reCAPTCHA task or using your existing Google or Facebook account. 

# Description of the prepared API: 
Documentation of the prepared API is available in the api_documentation.json file. To view the entire documentation, copy the contents of the attached file into the Swagger Editor available at https://editor.swagger.io/.
# Open Source Libraries Used
The Psinder application leverages several popular open source libraries to support various aspects of functionality and system infrastructure. Below, you'll find a brief description of each used library:
##### Leaflet
Leaflet is an open-source JavaScript library for creating interactive maps on web pages. In the application, Leaflet is employed for displaying maps and enabling user interaction with geographical data.
##### Redis
Redis is an open-source, advanced key-value store. In our application, Redis is used for storing keys and data, contributing to efficient data management in memory.
##### Dapper
Dapper is a lightweight Object-Relational Mapping (ORM) library for C#, simplifying database operations. In the application, Dapper is utilized for handling pagination, filtering, and sorting of data.
##### Docker
Docker is a platform that facilitates developing, deploying, and running applications in containers. Our application is containerized using Docker, allowing for easy replication of the development environment and deployment across different platforms.


