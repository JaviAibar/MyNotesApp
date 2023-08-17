using BlazorNotesApp.Services;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSingleton<IDatabaseManager, DatabaseManager>();
builder.Services.AddSingleton<IDialogService, DialogService>();
builder.Services.AddSingleton<INotesState, NotesState>();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication("loginScheme").AddCookie("loginScheme", opts =>
{
    opts.LoginPath = "/login";
    opts.LogoutPath = "/logout";
    opts.AccessDeniedPath = "/accessDenied"; // logged in but without permission
});
var app = builder.Build();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
