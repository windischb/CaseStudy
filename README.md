# Case Study

REST API with .NET core to manage (create, update, retrieve) vendor master data.  
The REST API should handle the following data objects with attributes: 

Vendor: 
* Name 
* Name2 
* Address1 
* Address2 
* ZIP 
* Country 
* City 
* Mail 
* Phone 
* Notes

Bank Account 
* IBAN 
* BIC 
* Name

Contact Person 
* First name 
* Last name 
* Phone 
* Mail

The API should check if the user is authenticated and has the permission to manage the data.

Data can be stored in a backend of your choice (for example sqlite).  
The service should use a cache like redis.  