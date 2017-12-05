Building and running the Project using Maven
---------------------------------------

1) Right CLick on the project in eclipse .Go to the Run As menu item and do 

  Maven clean 
  
2) And then repeat the step 1 and do a
  
  Maven install
  

To Run the application:
-------------------------

Right click on the project, go to the Run As Menu Item and select run on the server option.


Accessing the REST Patient Service
=================================


A RESTful patient services WADL can be accessed at this URL 

http://localhost:8080/restws/services/patientservice?_wadl
 

A HTTP GET request to URL http://localhost:8080/restws/services/patientservice/patients/123
returns a Patient instance whose id is 123. The XML document returned:

<?xml version="1.0" encoding="UTF-8" standalone="yes" ?>
 <Patient>
	<id>123</id>
	<name>John</name>
 </Patient>
 

A HTTP POST request to URL http://localhost:8080/restws/services/patientservice/patients
with the data:

<?xml version="1.0" encoding="UTF-8" standalone="yes" ?>
 <Patient>
	<name>Bharath</name>
 </Patient>

adds a patient whose name is Bharath 


A HTTP PUT request to URL http://localhost:8080/restws/services/patientservice/patients
with the data:

<Patient>
   <id>124</id>
   <name>Bharath T</name>
</Patient>

updates the patient instance whose id is 124


A HTTP DELETE request to URL http://localhost:8080/restws/services/patientservice/patients/124
will delete a Patient instance whose id is 124.


Sub Resource Mapping:

A HTTP GET request to URL http://localhost:8080/restws/services/patientservice/prescriptions/223/medicines/323
returns a Medicine instance 323 if it is present in the order 223. The XML document returned:

<?xml version="1.0" encoding="UTF-8" standalone="yes" ?>
 <Medicine>
	<description>Medicine 323</description>
	<id>323</id>
 </Medicine>
