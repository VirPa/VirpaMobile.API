using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Identity;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories {
    internal class MyLocation : IMyLocation {

        #region Initialization

        private readonly List<string> _infos = new List<string>();
        
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyLocation(IMapper mapper,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {
            
            _mapper = mapper;

            _userManager = userManager;
            _context = context;
        }

        #endregion
        
        #region Get



        #endregion

        #region Post

        public async Task<CustomResponse<PinLocationResponseModel>> PinMyLocation(PinLocationModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var location = _context.Location.FirstOrDefault(f => f.UserId == user.Id);

            if (location == null) {

                var pinnedLocation = await PinLocation();

                return new CustomResponse<PinLocationResponseModel> {
                    Succeed = true,
                    Data = new PinLocationResponseModel {
                        Location = _mapper.Map<PinLocationDetailResponseModel>(pinnedLocation)
                    }
                };
            }

            var modifiedLocation = ModifyLocation();

            return new CustomResponse<PinLocationResponseModel> {
                Succeed = true,
                Data = new PinLocationResponseModel {
                    Location = _mapper.Map<PinLocationDetailResponseModel>(modifiedLocation)
                }
            };

            #region Local Methods

            async Task<Location> PinLocation() {

                var mappedLocation = new Location {
                    UserId = user.Id,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Address = model.Address,
                    CityName = model.CityName,
                    State = model.State,
                    CountryName = model.CountryName,
                    PostalCode = model.PostalCode,
                    IpAddress = model.IpAddress,
                    MacAddress = model.MacAddress,
                    UpdatedAt = DateTime.UtcNow
                };

                var savedLocation = await _context.AddAsync(mappedLocation);

                await _context.SaveChangesAsync();

                return savedLocation.Entity;
            }

            Location ModifyLocation() {

                location.Latitude = model.Latitude;
                location.Longitude = model.Longitude;
                location.Address = model.Address;
                location.CityName = model.CityName;
                location.State = model.State;
                location.CountryName = model.CountryName;
                location.PostalCode = model.PostalCode;
                location.IpAddress = model.IpAddress;
                location.MacAddress = model.MacAddress;
                location.UpdatedAt = DateTime.UtcNow;

                var updatedLocation = _context.Update(location);

                _context.SaveChanges();

                return updatedLocation.Entity;
            }

            #endregion
        }

        #endregion
    }
}