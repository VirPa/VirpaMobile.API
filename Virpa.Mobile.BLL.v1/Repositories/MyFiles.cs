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

            var orderedFiles = files.OrderByDescending(f => f.CreatedAt);

            return new CustomResponse<GetFilesResponse> {
                Succeed = true,
                Data = new GetFilesResponse {
                    Files = _mapper.Map<List<GetFilesListResponse>>(orderedFiles)
                }
            };
        }

        #endregion

        #region Post

        public async Task<CustomResponse<GetFilesResponse>> SaveFiles(FileBase64Model model) {

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

                var addedFiles = await AddFiles();

                return new CustomResponse<GetFilesResponse> {
                    Succeed = true,
                    Data = new GetFilesResponse {
                        Files = addedFiles
                    }
                };
            }
            #endregion

            var modifiedFiles = ModifyFiles();

            return new CustomResponse<GetFilesResponse> {
                Succeed = true,
                Data = new GetFilesResponse {
                    Files = modifiedFiles
                }
            };

            #region Local Methods

            async Task<List<GetFilesListResponse>> AddFiles() {

                var files = new List<GetFilesListResponse>();

                foreach (var file in model.Files) {

                    var fileCode = Guid.NewGuid();

                    var extension = Path.GetExtension(file.Name);

                    var newFileName = fileCode + extension;
                    
                    var bytes = Convert.FromBase64String(file.Base64);

                    File.WriteAllBytes(Path.Combine(_options.Value.DirectoryPath,
                        newFileName), bytes);

                    var mappedFiles = new Files {
                        UserId = user.Id,
                        FeedId = model.FeedId,
                        Name = file.Name,
                        CodeName = fileCode + extension,
                        Extension = extension,
                        FilePath =
                            _options.Value.Protocol + _options.Value.Uri + _options.Value.Files + newFileName,
                        Type = model.Type,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    var savedFile = _context.Files.AddAsync(mappedFiles);

                    await _context.SaveChangesAsync();

                    files.Add(_mapper.Map<GetFilesListResponse>(savedFile.Result.Entity));
                }

                return files;
            }

            List<GetFilesListResponse> ModifyFiles() {
                var updatedFiles = new List<GetFilesListResponse>();

                var updateFile = model.Files.FirstOrDefault();

                var fileCode = fileData.CodeName;

                var extension = Path.GetExtension(updateFile?.Name);

                var updateFileName = fileCode + extension;

                File.Delete(Path.Combine(_options.Value.DirectoryPath,
                    fileCode + fileData.Extension));
                
                var bytes = Convert.FromBase64String(updateFile?.Base64);

                File.WriteAllBytes(Path.Combine(_options.Value.DirectoryPath,
                    updateFileName), bytes);

                fileData.Name = updateFile?.Name;
                fileData.FilePath = _options.Value.Protocol + _options.Value.Uri +
                                    _options.Value.Files + updateFileName;
                fileData.Extension = extension;
                fileData.UpdatedAt = DateTime.UtcNow;

                _context.Update(fileData);

                _context.SaveChanges();

                updatedFiles.Add(_mapper.Map<GetFilesListResponse>(fileData));

                return updatedFiles;
            }

            #endregion
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
                var dataFile = _context.Files.FirstOrDefault(a => a.IsActive == true && a.UserId == user.Id && a.Id == file.FileId);

                if (dataFile == null) {
                    continue;
                }

                dataFile.IsActive = false;

                _context.Update(dataFile);
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