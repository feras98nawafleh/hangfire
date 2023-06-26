using Hangfire;
using FireApp.Services;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(configuration => 
                            configuration.UseSimpleAssemblyNameTypeSerializer()
                            .UseRecommendedSerializerSettings()
                            .UseSqlServerStorage(builder.Configuration.GetConnectionString("hangfire")));

builder.Services.AddHangfireServer();
builder.Services.AddTransient<IServiceManagement, ServiceManagement>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    DashboardTitle = "Feras\'s Dashboard",
    Authorization = new [] {
        new HangfireCustomBasicAuthenticationFilter()
        {
            User = "Feras",
            Pass = "123Fer@s"
        }
    }
});
app.MapHangfireDashboard();
RecurringJob.AddOrUpdate<IServiceManagement>(x => x.SyncData(), "0 * * ? * *");

app.Run();
