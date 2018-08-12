using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Identity;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories {
    internal class MyAttachment : IMyAttachment {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IOptions<Manifest> _options;
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyAttachment(IOptions<Manifest> options,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {

            _options = options;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        #endregion

        public async Task<CustomResponse<GetAttachmentsResponse>> Attach(AttachmentModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<GetAttachmentsResponse> {
                    Message = _infos
                };
            }
            #endregion

            var attachments = new List<GetAttachmentsListResponse>();

            foreach (var attachment in model.Attachments) {

                if (attachment.Length > 0) {

                    var attachmentCode = Guid.NewGuid();

                    var extension = Path.GetExtension(attachment.FileName);

                    var newFileName = attachmentCode + extension;

                    using (
                        var fileStream = new FileStream(Path.Combine(_options.Value.DirectoryPath,
                                newFileName),
                            FileMode.Create)) {

                        await attachment.CopyToAsync(fileStream);
                        
                        var mappedAttachment = new Attachments {
                            UserId = user.Id,
                            FeedId = model.FeedId,
                            Name = attachment.FileName,
                            CodeName = attachmentCode.ToString(),
                            Extension = extension,
                            FilePath = _options.Value.Protocol + _options.Value.Uri + _options.Value.Attachments + newFileName,
                            Type = model.Type,
                            CreatedAt = DateTime.UtcNow,
                            IsActive = true
                        };

                        var savedAttachment = _context.Attachments.AddAsync(mappedAttachment);

                        attachments.Add(_mapper.Map<GetAttachmentsListResponse>(savedAttachment.Result.Entity));
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new CustomResponse<GetAttachmentsResponse> {
                Succeed = true,
                Data = new GetAttachmentsResponse {
                    Attachments = attachments
                }
            };
        }

        public async Task<CustomResponse<GetAttachmentsResponse>> GetAttachments(GetAttachments model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<GetAttachmentsResponse> {
                    Message = _infos
                };
            }
            #endregion

            var attachments = model.Type == 0 ? 
                _context.Attachments.Where(a => a.IsActive == true && a.UserId == user.Id).ToList() : 
                _context.Attachments.Where(a => a.IsActive == true && a.UserId == user.Id && a.Type == model.Type).ToList();

            return new CustomResponse<GetAttachmentsResponse> {
                Succeed = true,
                Data = new GetAttachmentsResponse {
                    Attachments = _mapper.Map<List<GetAttachmentsListResponse>>(attachments)
                }
            };
        }

        //NOTE: SOFT DELETE ONLY.
        public async Task<CustomResponse<GetAttachmentsResponse>> DeleteAttachments(DeleteAttachments model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<GetAttachmentsResponse> {
                    Message = _infos
                };
            }
            #endregion

            foreach (var attachment in model.Attachments) {
                var attachedFile = _context.Attachments.FirstOrDefault(a => a.IsActive == true && a.UserId == user.Id && a.Id == attachment.Id);

                if (attachedFile == null) {
                    continue;
                }

                attachedFile.IsActive = false;

                _context.Update(attachedFile);
            }

            _context.SaveChanges();

            return new CustomResponse<GetAttachmentsResponse> {
                Succeed = true,
                Data = GetAttachments(new GetAttachments {
                    Email = model.Email
                }).Result.Data
            };
        }
    }
}