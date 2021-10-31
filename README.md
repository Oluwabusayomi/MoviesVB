# ValueBlue Assessment Project - Backend

This project serves as a  backend designed to work with The Open Movie Database(OMDb). In order to demonstrate proper use of The Open Movie Database, visit *http://www.omdbapi.com* to create a free account. This server receives requests from client(s), and forwards these requests to The Movie Open Database.

The server itself is implemented in C# Net5.0 and uses MongoDb for persistence. It uses Microsoft.Extensions.Logging for logging exception, automapper for object mapping, Autofac for managing Depency Injection(DI), Swashbuckle swagger for API documentation and Microsoft.Extensions.Caching.Memory for cache

The server catches exception globally and logs the catched exception so that you could have an idea of the exception whithout having to look into the code.

## Integration with The Open Movie API
This sample backend interacts directly with OMDb's API, specifically for the purpose of searching movies by title. You can read more about the API *http://www.omdbapi.com*.
Communication with the API involves the use of a key, which can be found in the Appsetting file. Before sending a request to the OMDb API, the code checks the cache to see if the requested movie exists. This was implemented to reduce network calls and increase response time.
The in-memory cache is not ideal as the project grows; Redis Cache is more suitable.

## Running the server
If you wish to run the server, the first step after cloning the project is installing any supported Integrated development environment of your choice but preferably Visual studio 2019.
Open the project from the IDE, Restore the nugget packages, build and run the application. You will need a web browser for accessing the endpoints.

## Accessing the endpoints
All the endpoints are secured by an Api key except the one that searches movies by title. So you would need an Api key in the request header for accessing most of the endpoints. A sample Api is in the Appsetting Json file for the purpose of testing.
In the future, Api key would be generated per login user and stored in the database for security of the application. Also to keep things simple swagger is not configured to accept api key when making request to the server.
So you need Api testing software like Postman. If you do not have postman installed, you can also use Postwoman-online Api testing software.

## Additional/Bonus Points
A Unit testing project was added to the solution to do approximately 70% code coverage. Xunit testing framework is used, and Moq for mocking data.
Also additional secured endpoint was added for searching movie requests by query parameters. It can also be extended to cater for more searching options as the application grows.

