var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MetroBoard_Api>("metro-api")
    .WithEnvironment("WMATA_API_KEY", Environment.GetEnvironmentVariable("WMATA_API_KEY"))
    .WithEnvironment("SLEEP_TIME", Environment.GetEnvironmentVariable("SLEEP_TIME"))
    .WithExternalHttpEndpoints();

builder.Build().Run();
