using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.OtherServices.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.OtherServices {
    public class ProcessRefreshToken : IProcessRefreshToken {

        private readonly VirpaMobileContext _context;
        private readonly IOptions<Manifest> _options;

        public ProcessRefreshToken(VirpaMobileContext context,
            IOptions<Manifest> options) {

            _context = context;
            _options = options;
        }

        public async Task<CustomResponse<TokenResource>> GenerateRefreshToken(RefreshTokenGetModel model) {

            var userSession = _context.AspNetUserSessions.FirstOrDefault(t => t.UserId == model.UserId && t.DeviceName == model.UserAgent);

            var token = Guid.NewGuid().ToString().Replace("-", "") + "." + model.UserId.Replace("-", "");

            var refreshTokenExpiredMins = _options.Value.RefreshTokenExpiryMins;

            var refreshedToken = await RefreshToken();

            return new CustomResponse<TokenResource> {
                Succeed = true,
                Data = new TokenResource {
                    Token = refreshedToken.Token,
                    ExpiredAt = refreshedToken.ExpiredAt
                }
            };

            #region Local Methods

            async Task<TokenResource> RefreshToken() {

                if (userSession == null) { //Add
                    var refreshToken = await _context.AspNetUserSessions.AddAsync(new AspNetUserSessions {
                        Id = Guid.NewGuid().ToString(),
                        UserId = model.UserId,
                        Token = token,
                        TokenExpiredAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiredMins),
                        Validity = true,
                        DeviceName = model.UserAgent,
                        AppVersion = model.AppVersion,
                        ApiVersion = model.ApiVersion
                    });

                    await _context.SaveChangesAsync();

                    return new TokenResource {
                        Token = refreshToken.Entity.Token,
                        ExpiredAt = refreshToken.Entity.TokenExpiredAt
                    };
                }

                //Update
                userSession.Token = token;
                userSession.TokenExpiredAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiredMins);
                userSession.AppVersion = model.AppVersion;
                userSession.ApiVersion = model.ApiVersion;
                userSession.Validity = true;

                _context.AspNetUserSessions.Update(userSession);

                await _context.SaveChangesAsync();

                return new TokenResource {
                    Token = token,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiredMins)
                };
            }

            #endregion
        }
    }
}