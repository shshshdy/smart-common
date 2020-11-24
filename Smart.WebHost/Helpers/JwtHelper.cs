using Microsoft.IdentityModel.Tokens;
using System.Text;
using Smart.Infrastructure.Configs;

namespace Smart.Host.Helpers
{
    /// <summary>
    /// JwtHelper
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 获取jwt配置参数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TokenValidationParameters GetParameters(JwtConfig config)
        {
            var paramters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.SecurityKey)),
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            return paramters;
        }
    }
}
