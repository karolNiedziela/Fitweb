using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Exceptions
{
    public static class ErrorCodes
    {
        public static string InvalidCredentials => "invalid_credentials";

        public static string EmailInUse => "email_in_use";

        public static string UsernameInUse => "username_in_use";

        public static string UserNotFound => "user_not_found";

        public static string AthleteNotFound => "athlete_not_found";

        public static string ObjectAlreadyAdded => "object_already_added";

        public static string ObjectNotFound => "object_not_found";

        public static string InvalidValue => "invalid_value";

        public static string InvalidRefreshToken => "invalid_refresh_token";

        public static string EmailNotConfirmed => "email_not_confirmed";

        public static string InvalidToken => "invalid_token";
    }
}
