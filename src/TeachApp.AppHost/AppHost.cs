var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImage("postgres:16")
    .WithDataVolume()
    .WithContainerRuntimeArgs("--pull=always");

var redis = builder.AddRedis("redis")
    .WithImage("redis:alpine");

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithImage("rabbitmq:management")
    .WithDataVolume();

var api = builder.AddProject<Projects.Teach_API>("api")
    .WithReference(postgres)
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

var authApi = builder.AddProject<Projects.Auth_API>("auth-api")
    .WithReference(rabbitmq)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

var communicationApi = builder.AddProject<Projects.Communication_API>("communication-api")
    .WithReference(rabbitmq)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.AddProject<Projects.Teach_Gateway>("teach-gateway")
    .WithReference(api)
    .WithReference(authApi)
    .WithReference(communicationApi)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithHttpEndpoint(port: 7061, name: "https");

builder.Build().Run();
