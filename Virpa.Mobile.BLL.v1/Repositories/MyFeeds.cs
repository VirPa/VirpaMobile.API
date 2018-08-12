using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Virpa.Mobile.BLL.v1.DataManagers.Interface;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Identity;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories {
    internal class MyFeeds : IMyFeeds {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyAttachment _myAttachment;
        private readonly IMapper _mapper;
        private readonly IFeedsDataManager _feedsDataManager;
        private readonly IOptions<Manifest> _options;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyFeeds(IMyAttachment myAttachment,
            IMapper mapper,
            IFeedsDataManager feedsDataManager,
            IOptions<Manifest> options,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {

            _myAttachment = myAttachment;
            _mapper = mapper;
            _feedsDataManager = feedsDataManager;
            _options = options;

            _userManager = userManager;
            _context = context;
        }

        #endregion

        #region Get

        public async Task<CustomResponse<GetMyFeedsResponseModel>> GetMyFeeds(GetMyFeedsModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var feeds = _feedsDataManager.GetMyFeeds(new GetMyFeedsModel {UserId = user.Id });

            return new CustomResponse<GetMyFeedsResponseModel> {
                Succeed = true,
                Data = feeds
            };
        }
        #endregion

        #region Post

        public async Task<CustomResponse<PostMyFeedResponseModel>> PostMyFeed(PostMyFeedModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var feed = _context.Feeds.FirstOrDefault(f => f.Id == model.FeedId);

            if (feed == null) {

                var savedFeed = Save();
                
                var savedCoverPhoto = SaveCoverPhoto(savedFeed.Id);

                return new CustomResponse<PostMyFeedResponseModel> {
                    Succeed = true,
                    Data = new PostMyFeedResponseModel {
                        Feed = _mapper.Map<PostMyFeedDetailResponseModel>(savedFeed),
                        CoverPhoto = savedCoverPhoto.Attachments.FirstOrDefault()
                    }
                };
            }

            var updatedFeed = Update();

            return new CustomResponse<PostMyFeedResponseModel> {
                Succeed = true,
                Data = new PostMyFeedResponseModel {
                    Feed = _mapper.Map<PostMyFeedDetailResponseModel>(updatedFeed)
                }
            };

            #region Local Methods

            // ReSharper disable once ImplicitlyCapturedClosure
            Feeds Save() {
                var myFeed = _mapper.Map<Feeds>(model);

                myFeed.UserId = user.Id;

                var postedFeed =  _context.Feeds.Add(myFeed);

                 _context.SaveChanges();

                return postedFeed.Entity;
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            Feeds Update() {
                feed.Body = model.Body;
                feed.Budget = model.Budget;
                feed.UpdatedAt = DateTime.UtcNow;
                feed.ExpiredAt = DateTime.UtcNow.AddDays(model.ExpiredOn);

                _context.Feeds.Update(feed);

                _context.SaveChanges();

                return feed;
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            GetAttachmentsResponse SaveCoverPhoto(string feedId) {

                if (model.CoverPhoto == null) return null;

                var attachment = new AttachmentModel {
                    Email = model.Email,
                    Attachments = model.CoverPhoto,
                    FeedId = feedId,
                    Type = 2
                };

                var attachedCoverPhoto = _myAttachment.Attach(attachment);

                return attachedCoverPhoto.Result.Data;
            }

            #endregion
        }

        public async Task<CustomResponse<UpdateCoverPhotoResponse>> UpdateMyFeedCoverPhoto(UpdateCoverPhotoModel model) {

            var attachment = _context.Attachments.FirstOrDefault(a => a.Id == model.Id);

            if (attachment == null) {
                _infos.Add("Cover Photo identity not exist.");

                return new CustomResponse<UpdateCoverPhotoResponse> {
                    Message = _infos
                };
            }

            var coverPhotos = new List<GetAttachmentsListResponse>();

            var coverPhoto = model.CoverPhoto.FirstOrDefault();

            if (coverPhoto?.Length > 0) {

                var attachmentCode = attachment.CodeName;

                var extension = Path.GetExtension(coverPhoto.FileName);

                var newFileName = attachmentCode + extension;

                File.Delete(Path.Combine(_options.Value.DirectoryPath,
                    attachmentCode + attachment.Extension));

                using (

                    var fileStream = new FileStream(Path.Combine(_options.Value.DirectoryPath,
                            newFileName),
                        FileMode.Create)) {

                    await coverPhoto.CopyToAsync(fileStream);

                    attachment.Name = coverPhoto.FileName;
                    attachment.FilePath = _options.Value.Protocol + _options.Value.Uri +
                                            _options.Value.Attachments + newFileName;
                    attachment.Extension = extension;
                    attachment.UpdatedAt = DateTime.UtcNow;

                    _context.Update(attachment);

                    coverPhotos.Add(_mapper.Map<GetAttachmentsListResponse>(attachment));
                }
            }
            

            _context.SaveChanges();

            return new CustomResponse<UpdateCoverPhotoResponse> {
                Succeed = true,
                Data = new UpdateCoverPhotoResponse {
                    CoverPhoto = coverPhotos
                }
            };
        }

        #endregion
        }
}