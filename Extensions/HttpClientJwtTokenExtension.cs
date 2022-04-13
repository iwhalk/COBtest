namespace ApiGateway.Extensions
{
    public static class HttpClientJwtTokenExtension
    {
        public static void AddBearerToken(this HttpClient client, IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor?.HttpContext?.User == null)
                return;
            if (contextAccessor.HttpContext.User.Identity.IsAuthenticated && contextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var jwtToken = contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(jwtToken))
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", jwtToken);
            }
        }
    }
}
