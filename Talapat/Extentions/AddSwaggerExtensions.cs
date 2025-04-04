namespace Talapat.Extentions
{
    public static class AddSwaggerExtensions
    {
        public static WebApplication AddSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
