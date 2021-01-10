using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Exceptions
{
    public static class ErrorCodes
    {
        public static string InvalidUsername => "invalid_username";

        public static string InvalidEmail => "invalid_email";

        public static string InvalidPassword => "invalid_password";

        public static string RoleAlreadySet => "role_already_set";

        public static string InvalidName => "invalid_name";

        public static string InvalidValue => "invalid_value";

        public static string ObjectAlreadyAdded => "object_already_added";

        public static string ObjectNotFound => "object_not_found";

        public static string EmptyRefreshToken => "empty_refresh_token";

        public static string RevokedRefreshToken => "revoked_refresh_token";

    }
}
