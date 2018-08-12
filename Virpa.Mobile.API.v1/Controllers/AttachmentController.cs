﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.BLL.v1.Validation;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("doc")]
    [ApiVersion("1.0")]
    public class AttachmentController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyAttachment _myAttachment;
        private readonly ResponseBadRequest _badRequest;
        private readonly AttachmentModelValidator _attachmentModelValidator;

        #endregion

        #region Constructor

        public AttachmentController(IMyAttachment myAttachment,
            ResponseBadRequest badRequest,
            AttachmentModelValidator attachmentModelValidator) {

            _myAttachment = myAttachment;
            _badRequest = badRequest;
            _attachmentModelValidator = attachmentModelValidator;
        }

        #endregion

        [HttpPost("Attachments", Name = "Attachments")]
        public async Task<IActionResult> Attachment(ICollection<IFormFile> attachments) {
            
            var model = new AttachmentModel {
                Attachments = attachments,
                Email = UserEmail
            };

            #region Validate Model

            if (model.Attachments.Count == 0) {
                return BadRequest(new { error = _badRequest.ShowError(ResponseBadRequest.ErrFileEmpty) });
            }

            var userInputValidated = _attachmentModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            model.Type = 1;

            var attached = await _myAttachment.Attach(model);

            return Ok(attached);
        }

        [HttpGet("MyAttachments", Name = "MyAttachments")]
        public async Task<IActionResult> Attachments(int type = 0) {

            var model = new GetAttachments {
                Email = UserEmail,
                Type = type
            };

            var fetchedAttachments = await _myAttachment.GetAttachments(model);

            return Ok(fetchedAttachments);
        }

        [HttpPost("MyAttachments/Delete", Name = "DeleteMyAttachments")]
        public async Task<IActionResult> Attachments([FromBody] DeleteAttachments model)  {

            model.Email = UserEmail;

            var fetchedAttachments = await _myAttachment.DeleteAttachments(model);

            return Ok(fetchedAttachments);
        }
    }
}