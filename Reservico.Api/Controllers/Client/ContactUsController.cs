//using Microsoft.AspNetCore.Mvc;

//namespace Reservico.Api.Controllers.Client
//{
//    public class ContactUsController : BaseClientController
//    {
//        private readonly IContactUsService contactUsService;
//        private readonly ILogger<ContactUsController> logger;

//        public ContactUsController(
//            IContactUsService contactUsService, 
//            ILogger<ContactUsController> logger)
//        {
//            this.contactUsService = contactUsService;
//            this.logger = logger;
//        }

//        /// <summary>
//        /// Creates new Contact Us request.
//        /// </summary>
//        /// <param name="model">Contact us request Model to Create.</param>
//        /// <returns>Status code indicating Success or Failure.</returns>        
//        [HttpPost("Create")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<IActionResult> CreateContactUsRequest(CreateContactUsRequestModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState.Values.Select(x => x.Errors));
//            }

//            var userId = this.GetUserId();

//            try
//            {
//                var response = await contactUsService.CreateContactUsRequestAsync(
//                    model, userId);

//                if (!response.IsSuccess)
//                {
//                    return BadRequest(response.ErrorMessage);
//                }

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, ex.Message);
//                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//            }
//        }
//    }
//}
