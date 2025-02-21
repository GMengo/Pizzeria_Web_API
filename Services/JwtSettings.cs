namespace pizzeria_web_api.Services
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public int DurationInMinutes { get; set; }
    }

    public class JwtAuthenticationService
    {
        private readonly IConfiguration _configuration;
        public readonly JwtSettings _jwtSettings;
        private readonly UtenteService _utenteService;

        public JwtAuthenticationService(IConfiguration configuration, UtenteService utenteService)
        {
            _configuration = configuration;
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            _utenteService = utenteService;
        }


    }
}
