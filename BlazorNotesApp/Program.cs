using BlazorNotesApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSingleton<IDatabaseManager, DatabaseManager>();
builder.Services.AddSingleton<IDialogService, DialogService>();
builder.Services.AddAuthentication("loginScheme").AddCookie("loginScheme", opts =>
{
    opts.LoginPath = "/login";
    opts.LogoutPath = "/logout";
    opts.AccessDeniedPath = "/accessDenied"; // logeado pero sin permiso
});
var app = builder.Build();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
