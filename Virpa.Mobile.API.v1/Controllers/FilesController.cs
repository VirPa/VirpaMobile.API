using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.BLL.v1.Validation;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("files")]
    [ApiVersion("1.0")]
    public class FilesController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyFiles _myFiles;
        private readonly ResponseBadRequest _badRequest;
        private readonly FileBase64ModelValidator _fileModelValidator;

        #endregion

        #region Constructor

        public FilesController(IMyFiles myFiles,
            ResponseBadRequest badRequest,
            FileBase64ModelValidator fileModelValidator) {

            _myFiles = myFiles;
            _badRequest = badRequest;
            _fileModelValidator = fileModelValidator;
        }

        #endregion

        #region Get

        [HttpGet]
        public async Task<IActionResult> Files() {

            var model = new GetFiles {
                Email = UserEmail,
                Type = 0
            };

            var fetchedFiles = await _myFiles.GetFiles(model);

            return Ok(fetchedFiles);
        }

        [HttpGet("{type}", Name = "GetFileById")]
        public async Task<IActionResult> SpecificFiles(int type = 0) {

            var model = new GetFiles {
                Email = UserEmail,
                Type = type
            };

            var fetchedFiles = await _myFiles.GetFiles(model);

            return Ok(fetchedFiles);
        }

        #endregion

        #region Post

        [HttpPost]
        public async Task<IActionResult> PostFiles([FromBody] FileBase64Model model) {

            model.Email = UserEmail;
            model.Type = 1;

            #region Validate Model

            if (model.Files == null) {
                _infos.Add(_badRequest.ShowError(ResponseBadRequest.ErrFileEmpty).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            var userInputValidated = _fileModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var savedFiles = await _myFiles.SaveFiles(model);

            return Ok(savedFiles);
        }

        [HttpPost("Delete", Name = "Delete")]
        public async Task<IActionResult> DeleteFiles([FromBody] DeleteFiles model) {

            model.Email = UserEmail;

            var fetchedFiles = await _myFiles.DeleteFiles(model);

            return Ok(fetchedFiles);
        }

        #endregion
    }
}
