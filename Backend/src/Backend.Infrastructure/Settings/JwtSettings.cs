using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Settings
{
    // https://github.com/spetz/workshops-asp/blob/3313ac4df1cef94ecbd01c3ecdf9c9041ccdd6f8/src/Trill.Infrastructure/Auth/JwtOptions.cs#L6

    public class JwtSettings
    {
        public bool AuthenticationDisabled { get; set; }

        public string Algorithm { get; set; }

        public string Issuer { get; set; }

        public string IssuerSigningKey { get; set; }

        public string Authority { get; set; }

        public string Audience { get; set; }

        public string Challenge { get; set; } = "Bearer";

        public string MetadataAddress { get; set; }

        public bool SaveToken { get; set; } = true;

        public bool SaveSigninToken { get; set; }

        public bool RequireAudience { get; set; } = false;

        public bool RequireHttpsMetadata { get; set; } = true;

        public bool RequireExpirationTime { get; set; } = true;

        public bool RequireSignedTokens { get; set; } = true;

        public int ExpiryMinutes { get; set; }

        public TimeSpan? Expiry { get; set; }

        public string ValidAudience { get; set; }

        public IEnumerable<string> ValidAudiences { get; set; }

        public string ValidIssuer { get; set; }

        public IEnumerable<string> ValidIssuers { get; set; }

        public bool ValidateActor { get; set; }

        public bool ValidateAudience { get; set; } = true;

        public bool ValidateIssuer { get; set; } = true;

        public bool ValidateLifetime { get; set; } = true;

        public bool ValidateTokenReplay { get; set; }

        public bool ValidateIssuerSigningKey { get; set; } = true;

        public bool RefreshOnIssuerKeyNotFound { get; set; } = true;

        public bool IncludeErrorDetails { get; set; } = true;

        public string AuthenticationType { get; set; }

        public string NameClaimType { get; set; }

        public string RoleClaimType { get; set; }

    }
}
