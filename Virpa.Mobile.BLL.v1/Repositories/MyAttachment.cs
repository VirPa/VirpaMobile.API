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

        public async Task<CustomResponse<List<GetAttachmentsResponse>>> Attach(AttachmentModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<List<GetAttachmentsResponse>> {
                    Message = _infos
                };
            }
            #endregion

            var attachments = new List<GetAttachmentsResponse>();

            foreach (var attachment in model.Attachments) {

                if (attachment.Length > 0) {

                    var newFileName = Guid.NewGuid() + Path.GetExtension(attachment.FileName);

                    using (
                        var fileStream = new FileStream(Path.Combine(_options.Value.DirectoryPath,
                                newFileName),
                            FileMode.Create)) {

                        await attachment.CopyToAsync(fileStream);

                        attachments.Add(new GetAttachmentsResponse {
                            UserId = user.Id,
                            Name = attachment.FileName,
                            CodeName = newFileName,
                            Extension = Path.GetExtension(attachment.FileName),
                            FilePath = _options.Value.Protocol + _options.Value.Uri + _options.Value.Attachments + newFileName,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            var mappedAttachments = _mapper.Map<List<Attachments>>(attachments);

            await _context.Attachments.AddRangeAsync(mappedAttachments);

            await _context.SaveChangesAsync();

            return new CustomResponse<List<GetAttachmentsResponse>> {
                Succeed = true,
                Data = attachments
            };
        }

        public async Task<CustomResponse<List<GetAttachmentsResponse>>> GetAttachments(GetAttachments model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<List<GetAttachmentsResponse>> {
                    Message = _infos
                };
            }
            #endregion

            var attachments = _context.Attachments.Where(a => a.IsActive == true && a.UserId == user.Id).ToList();

            return new CustomResponse<List<GetAttachmentsResponse>> {
                Succeed = true,
                Data = _mapper.Map<List<GetAttachmentsResponse>>(attachments)
            };
        }
    }
}