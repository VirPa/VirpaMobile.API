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
    internal class MyFiles : IMyFiles {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IOptions<Manifest> _options;
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyFiles(IOptions<Manifest> options,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {

            _options = options;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        #endregion

        #region Get

        public async Task<CustomResponse<GetFilesResponse>> GetFiles(GetFiles model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<GetFilesResponse> {
                    Message = _infos
                };
            }
            #endregion

            var files = model.Type == 0 ?
                _context.Files.Where(a => a.IsActive == true && a.UserId == user.Id).ToList() :
                _context.Files.Where(a => a.IsActive == true && a.UserId == user.Id && a.Type == model.Type).ToList();

            return new CustomResponse<GetFilesResponse> {
                Succeed = true,
                Data = new GetFilesResponse {
                    Files = _mapper.Map<List<GetFilesListResponse>>(files)
                }
            };
        }

        #endregion

        #region Post

        public async Task<CustomResponse<GetFilesResponse>> PostFiles(FileModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<GetFilesResponse> {
                    Message = _infos
                };
            }
            #endregion

            var fileData = _context.Files.FirstOrDefault(f => f.Id == model.FileId); 

            #region Add Files

            if (fileData == null) {

                var files = new List<GetFilesListResponse>();

                foreach (var file in model.Files) {

                    if (file.Length > 0) {

                        var attachmentCode = Guid.NewGuid();

                        var extension = Path.GetExtension(file.FileName);

                        var newFileName = attachmentCode + extension;

                        using (
                            var fileStream = new FileStream(Path.Combine(_options.Value.DirectoryPath,
                                    newFileName),
                                FileMode.Create)) {

                            await file.CopyToAsync(fileStream);

                            var mappedFiles = new Files {
                                UserId = user.Id,
                                FeedId = model.FeedId,
                                Name = file.FileName,
                                CodeName = attachmentCode.ToString(),
                                Extension = extension,
                                FilePath = _options.Value.Protocol + _options.Value.Uri + _options.Value.Files + newFileName,
                                Type = model.Type,
                                CreatedAt = DateTime.UtcNow,
                                IsActive = true
                            };

                            var savedAttachment = _context.Files.AddAsync(mappedFiles);

                            files.Add(_mapper.Map<GetFilesListResponse>(savedAttachment.Result.Entity));
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return new CustomResponse<GetFilesResponse> {
                    Succeed = true,
                    Data = new GetFilesResponse {
                        Files = files
                    }
                };
            }
            #endregion

            var updatedFiles = new List<GetFilesListResponse>();

            var updateFile = model.Files.FirstOrDefault();

            if (updateFile?.Length > 0) {

                var attachmentCode = fileData.CodeName;

                var extension = Path.GetExtension(updateFile.FileName);

                var newFileName = attachmentCode + extension;

                File.Delete(Path.Combine(_options.Value.DirectoryPath,
                    attachmentCode + fileData.Extension));

                using (

                    var fileStream = new FileStream(Path.Combine(_options.Value.DirectoryPath,
                            newFileName),
                        FileMode.Create)) {

                    await updateFile.CopyToAsync(fileStream);

                    fileData.Name = updateFile.FileName;
                    fileData.FilePath = _options.Value.Protocol + _options.Value.Uri +
                                          _options.Value.Files + newFileName;
                    fileData.Extension = extension;
                    fileData.UpdatedAt = DateTime.UtcNow;

                    _context.Update(fileData);

                    updatedFiles.Add(_mapper.Map<GetFilesListResponse>(fileData));
                }
            }

            _context.SaveChanges();

            return new CustomResponse<GetFilesResponse> {
                Succeed = true,
                Data = new GetFilesResponse {
                    Files = updatedFiles
                }
            };
        }

        //NOTE: SOFT DELETE ONLY.
        public async Task<CustomResponse<GetFilesResponse>> DeleteFiles(DeleteFiles model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<GetFilesResponse> {
                    Message = _infos
                };
            }
            #endregion

            foreach (var file in model.Files) {
                var attachedFile = _context.Files.FirstOrDefault(a => a.IsActive == true && a.UserId == user.Id && a.Id == file.Id);

                if (attachedFile == null) {
                    continue;
                }

                attachedFile.IsActive = false;

                _context.Update(attachedFile);
            }

            _context.SaveChanges();

            return new CustomResponse<GetFilesResponse> {
                Succeed = true,
                Data = GetFiles(new GetFiles {
                    Email = model.Email
                }).Result.Data
            };
        }

        #endregion
    }
}