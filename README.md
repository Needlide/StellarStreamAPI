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
