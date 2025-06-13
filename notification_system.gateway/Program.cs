var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.UseHealthChecks("/health");

app.UseOcelot(oc =>
        oc.PreQueryStringBuilderMiddleware = async (context, next) =>
        {
            var contextAccessor =
                context.RequestServices.GetRequiredService<IHttpContextAccessor>();
            contextAccessor.HttpContext = context;
            await next.Invoke();
        }
    )
    .Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
