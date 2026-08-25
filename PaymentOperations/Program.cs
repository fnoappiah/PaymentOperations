using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PaymentOperations.Components;
using PaymentOperations.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.MaxBufferedUnacknowledgedRenderBatches = 20;
    });

builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = long.MaxValue;
});

var app = builder.Build();



app.MapDelete("/api/delete", async (int id, AppDbContext db) =>
{
    var file = await db.PaymentFiles.FindAsync(id);
    if (file == null)
        return Results.NotFound();

    var fullPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        file.Path
    );

    if (File.Exists(fullPath))
        File.Delete(fullPath);

    db.PaymentFiles.Remove(file);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/api/download", async (string path, IConfiguration config) =>
{
    // Resolve the allowed base path from config (falls back to user Downloads)
    var basePath = config["UploadSettings:BasePath"]
        ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads", "PaymentUploads");

    // The stored path is already the full absolute path — use it directly
    var fullPath = path;

    // Prevent path traversal: file must live inside the configured base path
    if (!fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
        return Results.BadRequest("Invalid path.");

    if (!File.Exists(fullPath))
        return Results.NotFound("File not found.");

    var fileName = Path.GetFileName(fullPath);
    var fileBytes = await File.ReadAllBytesAsync(fullPath);

    var contentType = Path.GetExtension(fullPath).ToLower() switch
    {
        ".pdf" => "application/pdf",
        ".xml" => "application/xml",
        ".csv" => "text/csv",
        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ".xls" => "application/vnd.ms-excel",
        ".zip" => "application/zip",
        ".txt" => "text/plain",
        ".json" => "application/json",
        _ => "application/octet-stream"
    };

    return Results.File(fileBytes, contentType, fileName);
});



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();