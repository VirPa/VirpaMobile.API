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
        private readonly IMyUser _user;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyFollowers(IMapper mapper,
            IMyUser user,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {
            
            _mapper = mapper;
            _user = user;

            _userManager = userManager;
            _context = context;
        }

        #endregion
        
        #region Get

        public async Task<CustomResponse<GetMyFollowersResponseModel>> GetFollowers(GetMyFollowersModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var followers = _context.Followers.Where(s => (s.IsActive == true || s.IsActive == null) && s.FollowedId == user.Id).ToList();

            var followerList = new List<GetMyFollowersListModel>();

            foreach (var follower in followers) {

                followerList.Add(new GetMyFollowersListModel {
                    User = _user.GetUser(new GetUserModel {
                            UserId = follower.FollowerId
                        }).Result.Data,
                    FollowedAt = follower.FollowedAt,
                    UpdatedAt = follower.UpdatedAt
                });
            }

            return new CustomResponse<GetMyFollowersResponseModel> {
                Succeed = true,
                Data = new GetMyFollowersResponseModel {
                    Followers = followerList
                }
            };
        }

        public async Task<CustomResponse<GetMyFollowedResponseModel>> GetFollowed(GetMyFollowersModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var followedUsers = _context.Followers.Where(s => (s.IsActive == true || s.IsActive == null ) && s.FollowerId == user.Id).ToList();

            var followedList = new List<GetMyFollowedListModel>();

            foreach (var followed in followedUsers) {

                followedList.Add(new GetMyFollowedListModel {
                    User = _user.GetUser(new GetUserModel {
                            UserId = followed.FollowedId
                        }).Result.Data,
                    FollowedAt = followed.FollowedAt,
                    UpdatedAt = followed.UpdatedAt
                });
            }

            return new CustomResponse<GetMyFollowedResponseModel> {
                Succeed = true,
                Data = new GetMyFollowedResponseModel {
                    Followed = followedList
                }
            };
        }
        #endregion

        #region Post

        public async Task<CustomResponse<PostMyFollowerResponseModel>> PostFollower(PostMyFollowerModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var follower = _context.Followers.FirstOrDefault(f => f.FollowedId == model.FollowedId && f.FollowerId == user.Id);

            if (follower == null) {

                var savedFollower = SaveFollower();

                AddFollowCountToUser();

                return new CustomResponse<PostMyFollowerResponseModel> {
                    Succeed = true,
                    Data = _mapper.Map<PostMyFollowerResponseModel>(savedFollower)
                };
            }

            var modifiedFollower = ModifiedFollower();

            return new CustomResponse<PostMyFollowerResponseModel> {
                Succeed = true,
                Data = _mapper.Map<PostMyFollowerResponseModel>(modifiedFollower)
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

            void AddFollowCountToUser() {
                user.FollowersCount = user.FollowersCount + 1;

                _context.Update(user);

                _context.SaveChanges();
            }

            #endregion
        }

        public async Task<CustomResponse<PostMyFollowerResponseModel>> UnFollow(PostMyFollowerModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var follower = _context.Followers.FirstOrDefault(f => f.FollowedId == model.FollowedId && f.FollowerId == user.Id);

            if (follower == null) {
                _infos.Add("Unfollow attempt failed! You did no follow this user.");

                return new CustomResponse<PostMyFollowerResponseModel> {
                    Message = _infos
                };
            }

            var unFollowed = UnFollow();

            SubtractFollowCountToUser();

            return new CustomResponse<PostMyFollowerResponseModel> {
                Succeed = true,
                Data = _mapper.Map<PostMyFollowerResponseModel>(unFollowed)
            };

            #region Local Methods

            Followers UnFollow() {

                follower.IsActive = false;
                follower.UpdatedAt = DateTime.UtcNow;

                var updatedFollower = _context.Update(follower);

                _context.SaveChanges();

                return updatedFollower.Entity;
            }

            void SubtractFollowCountToUser() {

                user.FollowersCount = user.FollowersCount - 1;

                _context.Update(user);

                _context.SaveChanges();
            }
            #endregion
        }
        #endregion
    }
}