// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Todo.Service.Common;
using Todo.Service.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IResourceStore<long, TodoItem>, InMemoryStore<long, TodoItem>>();
builder.Services.AddSingleton<IResourceStore<long, List<TodoAttachment>>, InMemoryStore<long, List<TodoAttachment>>>();
builder.Services.AddSingleton<IResourceStore<long, User>, InMemoryStore<long, User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
