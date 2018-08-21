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
    internal class MyFollowers : IMyFollowers {

        #region Initialization

        private readonly List<string> _infos = new List<string>();
        
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyFollowers(IMapper mapper,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {
            
            _mapper = mapper;

            _userManager = userManager;
            _context = context;
        }

        #endregion
        
        #region Get

        public async Task<CustomResponse<GetMyFollowersResponseModel>> GetFollowers(GetMyFollowersModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var followers = _context.Followers.Where(s => s.IsActive == true && s.FollowedId == user.Id).ToList();

            return new CustomResponse<GetMyFollowersResponseModel> {
                Succeed = true,
                Data = new GetMyFollowersResponseModel {
                    Followers = _mapper.Map<List<GetMyFollowersListModel>>(followers)
                }
            };
        }
        #endregion

        #region Post

        public async Task<CustomResponse<GetMyFollowersResponseModel>> PostFollower(PostMyFollowerModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var follower = _context.Followers.FirstOrDefault(f => f.FollowedId == model.FollowedId && f.FollowerId == user.Id);

            if (follower == null) {

                var savedFollower = SaveFollower();

                return new CustomResponse<GetMyFollowersResponseModel> {
                    Succeed = true,
                    Data = new GetMyFollowersResponseModel {
                        Followers = _mapper.Map<List<GetMyFollowersListModel>>(savedFollower)
                    }
                };
            }

            var modifiedFollower = ModifiedFollower();

            return new CustomResponse<GetMyFollowersResponseModel> {
                Succeed = true,
                Data = new GetMyFollowersResponseModel {
                    Followers = _mapper.Map<List<GetMyFollowersListModel>>(modifiedFollower)
                }
            };

            #region Local Methods

            async Task<Followers> SaveFollower() {

                var mappedFollower = new Followers {
                    FollowedId = model.FollowedId,
                    FollowerId = user.Id,
                    FollowedAt = DateTime.UtcNow
                };

                var savedFollower = await _context.AddAsync(mappedFollower);

                await _context.SaveChangesAsync();

                return savedFollower.Entity;
            }

            Followers ModifiedFollower() {

                follower.IsActive = true;
                follower.UpdatedAt = DateTime.UtcNow;

                var updatedFollower = _context.Update(follower);

                _context.SaveChanges();

                return updatedFollower.Entity;
            }

            #endregion
        }

        public async Task<CustomResponse<GetMyFollowersResponseModel>> UnFollow(PostMyFollowerModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var follower = _context.Followers.FirstOrDefault(f => f.FollowedId == model.FollowedId && f.FollowerId == user.Id);

            if (follower == null) {
                _infos.Add("Unfollow attempt failed! You did no follow this user.");

                return new CustomResponse<GetMyFollowersResponseModel> {
                    Message = _infos
                };
            }

            var unFollowed = UnFollow();

            return new CustomResponse<GetMyFollowersResponseModel> {
                Succeed = true,
                Data = new GetMyFollowersResponseModel {
                    Followers = _mapper.Map<List<GetMyFollowersListModel>>(unFollowed)
                }
            };

            #region Local Methods

            Followers UnFollow() {

                follower.IsActive = false;
                follower.UpdatedAt = DateTime.UtcNow;

                var updatedFollower = _context.Update(follower);

                _context.SaveChanges();

                return updatedFollower.Entity;
            }

            #endregion
        }
        #endregion
    }
}