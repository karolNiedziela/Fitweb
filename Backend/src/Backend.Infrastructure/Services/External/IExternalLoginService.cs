﻿using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.External
{
    public interface IExternalLoginService
    {
        Task<JwtDto> LoginWithFacebookAsync(string accessToken);
    }
}
