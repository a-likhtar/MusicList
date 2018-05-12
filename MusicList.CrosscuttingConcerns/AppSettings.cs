
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicList.CrosscuttingConcerns
{

    public class AppSettings
    {
        public Connectionstrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
        public Authoptions AuthOptions { get; set; }
    }

    public class Connectionstrings
    {
        public string IdentityConnection { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    public class Authoptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string Lifetime { get; set; }
        public string RememberMeLifetime { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
    }

}
