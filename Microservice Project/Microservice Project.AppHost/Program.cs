var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.catalog_API>("catalog-api");

builder.Build().Run();
