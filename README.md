# MaterCodeTest
Basic Web App for booking out hospital beds in an emergency unit


API Swagger help guide: https://localhost:7150/swagger/index.html
server running on port: 7150
client link:  http://127.0.0.1:5500/


**Instructions:**

Open MaterCodeTest.sln in Visual Studio.
Build and run project, swagger help guide should open in default browser.
Open the MaterWebClient files in Visual Studio Code.
Select the Go Live option in the bottom right corner.



**Summary**

I have built this app to match the sample format in the criteria documentation. the backend has been built in Visual Studio 2022 using an ASP.NET API framework. The front end was built in Visual Studio Code with a basic js, index, and css files. For simplicity, I have limited the web app for a single page. Ideally a real world version would have other pages to review or alter patients / admitions / staff data.

The criteria indicated the desire for a basic web app to preview the use of RESTapis, as such a very simple web application was created purposely avoiding more complex elements like DTO Classes for the sake of simplicity.




**Using the Client**

On load, the page will generate a table of the current Beds and their availability, as well as the summary totals below.
For testing purposes, I have seeded the web app's InMemory database with the sample data taken from the testing documentation

If the bed is in use, the patient details including the last updated time and comment and will also been shown on the table.
Clicking on a row with a bed in use will open a dialogue window with the patients full details including all comments.

To admit a new patient, select the Admit button
you will be prompted to enter a patient URN or create a new patient.
If the entered URN is not found the window will close without admitting the patient.
If users select the option to add a new patient. they will automatically be admitted to the bed selected.

Upon admitting a patient, a new comment is automatically created and assigned to the patient to indicate when they were admitted.

Users can add comments to an admitted patient by selcting the 'Add Comment' button.
on doing so, users are prompted to enter their staff ID and comment. A date and time value will automatically be assigned.

Users can discharge a patient by selecting the 'Discharge' button. At which point they will be prompted for confirmation.

Upon any changes made by a user, the page will automatically refresh.



**Assumptions Made**

Comments are assigned automatically when a patient is created, admitted, or discharged. for simplicity I set the staffID value on these comments = 1. In a real world example, users would first need to log in and the info would be taken automatically from their accounts.

the API used to lodged patients, comments and admittions can all be used to update the existing data as well. however, this functionality was not implemented on the client due to time constraints and noting it is not a required feature.

Included the functionality to add new patients on the fly when addmitting, assuming it would be a required functionality.

I had to create the entity framework based off the sample data and the most effecient way to return the required data in the least number of calls. As such, each Bed will link to a Admition record and Patient details, which in turn can be used to load comments and staff member data.

simarily, assuming addmitions should be tracked, I created the model with their own enum status to indicate when a patient was discharged.

Staff members have an enum value used to indicate their role, assuming Nurses may not be the only staff logging comments. However, sample staff data only holds Nurses based on the limited info in the sample provided.
