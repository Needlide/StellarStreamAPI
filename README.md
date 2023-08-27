# StellarStreamAPI
### Description
StellarStreamAPI is RESTful Web API which uses API keys, JWT and encryption (AES, RSA), and lets users to request data from NoSQL database where stored data from several public Web APIs (NASA Open APIs: APOD, Mars Rover Photos, NASA Image and Video Library. Spaceflight News API).
### Specifics
This API runs on ASP.NET Core 6, uses SwaggerUI, MongoDB and Docker Desktop.
#### NuGet Packages:
* BouncyCastle.NetCore - 1.9.0
* FluentValidation.AspNetCore - 11.3.0
* Microsoft.AspNetCore.Authentication.JwtBearer - 6.0.21
* Microsoft.AspNetCore.Mvc.Versioning - 5.1.0
* Microsoft.Extensions.Configuration.UserSecrets - 7.0.0
* Microsoft.VisualStudio.Azure.Containers.Tools.Targets - 1.19.5
* MongoDB.Driver - 2.21.0, Newtonsoft.Json - 13.0.3
* Swashbuckle.AspNetCore - 6.5.0.
API uses user secrets (secrets.json) to configure encryption and MongoClient.
#### Keys used:
* Security:EncryptionPassword
* Kestrel:Certificates:Development:Password
* ConnectionStrings:Security
* ConnectionStrings:Content

Be aware that for Docker you need to use another connection string than for local development (in Docker: mongodb://host.docker.internal:27017/). API uses private_key.pem and public_key.pem for RSA encryption. These files not included in the repository.
### Usage
Usage default for Web APIs with Swagger. Register/login, include JWT token in Authorize section (first field) of SwaggerUI, register API key, save API key (you see it once), include API key in Authorize section (second field) of SwaggerUI, make requests. Limit per hour: 60 requests. Can be changed in middleware.
### Parameters
#### News
* count - integer. Specifies count of records to return. Negative values treated as default (10).
* offset - integer. Specifies how much records needs to be skipped. Negative values treated as zero.
* title - string. Specifies words to search in Title. Will return records where Titles contains given value.
* newsSite - string. Specifies words to search in NewsSite. Will return records where NewsSite contains given value.
* startDateP - DateTime. Specifies date from which search started by PublishedAt parameter of record. Will return records where PublishedAt is bigger than given value.
* endDateP - DateTime. Specifies date which used to return records where PublishedAt parameter is smaller than given value. Records where PublishedAt parameter is sooner will be returned.
* startDateU - DateTime. Same as _startDateP_ but for UpdatedAt parameter.
* endDateU - DateTime. Same as _endDateP_ but for UpdatedAt parameter.
#### NasaImages
* count - integer. Specifies count of records to return. Negative values treated as default (10).
* offset - integer. Specifies how much records needs to be skipped. Negative values treated as zero.
* title - string. Specifies words to search in Title. Will return records which Titles contains given value.
* center - string. Specifies words to search in Center. Will return records where Center contains given value.
* nasaId - string. Specifies values to search in NasaId. Will return records where NasaId contains given value.
* mediaType - string. Specifies values to search in MediaType. Will return records where MediaType contains given value.
* keywords - array of strings. Specifies keywords by which record is searched. Will return records where Keywords contains any given value.
* startDate - DateTime. Specifies date which used to return records where DateCreated parameter is bigger than given value. Records where DateCreated parameter is bigger will be returned.
* endDate - DateTime. Specifies date which used to return records where DateCreated parameter is smaller than given value. Records where DateCreated parameter is sooner will be returned.
* secondaryDescription - string. Specifies text to search in SecondaryDescription. Will return records where SecondaryDescription contains given value.
* secondaryCreator - string. Specifies text to search in SecondaryCreator. Will return records where SecondaryCreator contains given value.
* description - string. Specifies text to search in Description. Will return records where Description contains given value.
#### MarsPhotos
* count - integer. Specifies count of records to return. Negative values treated as default (10).
* offset - integer. Specifies how much records needs to be skipped. Negative values treated as zero.
* startSol - integer. Specifies from which Sol (day on the Mars) start to search. Will return records where Sol is bigger than given value.
* endSol - integer. Specifies to which Sol (day on the Mars) provide search. Will return records where Sol is smaller than given value.
* cameraName - string. Specifies text to search in name of the camera of the rover. Will return records where ShortCameraName contains given value.
* roverName - string. Specifies text to search in name of the rover. Will return records where RoverName contains given value.
* startDate - DateTime. Specifies date which used to return records where DateCreated parameter is bigger than given value. Records where DateCreated parameter is bigger will be returned.
* endDate - DateTime. Specifies date which used to return records where DateCreated parameter is smaller than given value. Records where DateCreated parameter is sooner will be returned.
#### APOD
* count - integer. Specifies count of records to return. Negative values treated as default (10).
* offset - integer. Specifies how much records needs to be skipped. Negative values treated as zero.
* title - string. Specifies words to search in Title. Will return records which Titles contains given value.
* explanation - string. Specifies words to search in Explanation. Will return records where Explanation contains given value.
* startDate - DateTime. Specifies date which used to return records where DateCreated parameter is bigger than given value. Records where DateCreated parameter is bigger will be returned.
* endDate - DateTime. Specifies date which used to return records where DateCreated parameter is smaller than given value. Records where DateCreated parameter is sooner will be returned.
* copyright - string. Specifies text to search in Copyright. Will return records where Copyright contains given value.
* mediaType - string. Specifies values to search in MediaType. Will return records where MediaType contains given value.
