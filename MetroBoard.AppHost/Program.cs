var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MetroBoard_Api>("metro-api")
    .WithExternalHttpEndpoints();

builder.Build().Run();
