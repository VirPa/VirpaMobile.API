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
    internal class MyBidding : IMyBidding {

        #region Initialization

        private readonly List<string> _infos = new List<string>();
        
        private readonly IMapper _mapper;
        private readonly IMyUser _user;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyBidding(IMapper mapper,
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

        public CustomResponse<GetBiddersResponseModel> GetBidders(GetBiddersModel model) {

            var bidders = _context.FeedBidders.Where(f => f.FeedId == model.FeedId).ToList();

            #region Validate Bidders

            if (bidders.Count == 0) {
                _infos.Add("No bidders added.");

                return new CustomResponse<GetBiddersResponseModel> {
                    Message = _infos
                };
            }

            #endregion

            var biddersList = new List<GetBiddersListModel>();

            foreach (var bidder in bidders) {

                biddersList.Add(new GetBiddersListModel {
                    User = _user.GetUser(new GetUserModel {
                            UserId = bidder.UserId
                        }).Result.Data,
                    CreatedAt = bidder.CreatedAt,
                    Status = bidder.Status
                });
            }

            return new CustomResponse<GetBiddersResponseModel> {
                Succeed = true,
                Data = new GetBiddersResponseModel() {
                    Bidders = biddersList
                }
            };
        }
        #endregion

        #region Post

        public async Task<CustomResponse<PostBidderResponseModel>> PostBidder(PostBidderModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var feed = _context.Feeds.FirstOrDefault(f => f.Id == model.FeedId);

            #region Validate Feed

            if (feed == null) {

                _infos.Add("Feed is not exist.");

                return new CustomResponse<PostBidderResponseModel> {

                    Message = _infos
                };
            }

            #endregion

            var bidder = _context.FeedBidders.FirstOrDefault(f => f.FeedId == model.FeedId && f.UserId == user.Id);

            if (bidder == null) {

                var savedBidder = await SaveBidder();

                AddBidderCountToFeed();

                return new CustomResponse<PostBidderResponseModel> {
                    Succeed = true,
                    Data = new PostBidderResponseModel {
                        Bidder = _mapper.Map<PostBidderDetailResponseModel>(savedBidder)
                    }
                };
            }

            var modifiedBidder = ModifiedBidder();

            return new CustomResponse<PostBidderResponseModel> {
                Succeed = true,
                Data = new PostBidderResponseModel {
                    Bidder = _mapper.Map<PostBidderDetailResponseModel>(modifiedBidder)
                }
            };

            #region Local Methods

            async Task<FeedBidders> SaveBidder() {

                var mappedBidder = new FeedBidders {
                    FeedId = model.FeedId,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    InitialMessage = model.InitialMessage,
                    Status = 0
                };

                var savedBidder = await _context.AddAsync(mappedBidder);

                await _context.SaveChangesAsync();

                return savedBidder.Entity;
            }

            FeedBidders ModifiedBidder() {

                bidder.InitialMessage = model.InitialMessage;
                bidder.UpdatedAt = DateTime.UtcNow;

                var updatedBidder = _context.Update(bidder);

                _context.SaveChanges();

                return updatedBidder.Entity;
            }

            void AddBidderCountToFeed() {

                feed.BiddingCounts = feed.BiddingCounts + 1;

                using (var newContext = new VirpaMobileContext()) {
                    newContext.Update(feed);

                    newContext.SaveChanges();
                }
            }

            #endregion
        }

        #endregion
    }
}