using E_CommerceApi.DataAccess;
using E_CommerceApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();


builder.Services.AddDbContext<CommerceDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<CommerceDbContext>();

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<PasswordResetService>();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowBlazorClient",
		builder => builder
			.WithOrigins("https://localhost:7161") // Replace with your Blazor WebAssembly URL
			.AllowAnyHeader()
			.AllowAnyMethod());
});


builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Issuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	};
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;
//	//DataSeeder.Seed(services);


//	var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
//	var user = userManager.FindByNameAsync("admin").Result;
//	if (user != null)
//	{
//		user.LockoutEnd = null;
//		userManager.UpdateAsync(user).Wait();
//	}

//	var passwordResetService = services.GetRequiredService<PasswordResetService>();
//	var logger = services.GetRequiredService<ILogger<Program>>();
//	var result = passwordResetService.ResetPasswordAsync("admin", "Password123!").Result;
//	if (!result)
//	{
//		logger.LogError("Error resetting password");
//	}
//	else
//	{
//		logger.LogError("Password reset successfully");
//	}
//}




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();

app.Run();
