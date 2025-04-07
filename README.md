# Minimal APIs protected by centralized Identity

Example showing how to configure authentication and authorization using .NET Identity and JWT token shared between two microservices (2 web api).

Being an example it is a minimal configuration, which excludes the possibility of token refresh, role management, claims, etc.. but it is still possible by adding the missing configuration.

At the moment the configuration of the JWT token parameters have been replicated in the appsettings.json file of the 3 API projects.

It is possible to implement complete JWT token management by adding users, roles, permissions and much more dynamically. To do this, you need to integrate this [library](https://github.com/AngeloDotNet/Packages.MinimalApi.Identity) into your project.

Comments and/or suggestions are always welcome.
