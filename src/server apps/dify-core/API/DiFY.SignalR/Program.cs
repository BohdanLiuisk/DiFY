using System.IdentityModel.Tokens.Jwt;
using DiFY.SignalR.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
var clientOrigin = builder.Configuration["CorsOrigins:ClientOrigin"];
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
{
    b.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(clientOrigin);
}));
builder.Services.AddAuthorization();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = 
        JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = 
        JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.Authority = builder.Configuration["IdentityServer"];
    x.Audience = "DiFYCoreAPI";
    x.RequireHttpsMetadata = false;
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<DifyHub>("/difyHub");
});
app.Run();
