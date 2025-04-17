using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApiAot
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public IActionResult Get()      
        {
            var model = new HomeModel
            {
                Id = 1,
                Title = "Hello, World",
                Message = "Hello, World",
                DueBy = DateOnly.FromDateTime(DateTime.Now),
                IsComplete = false
            };

            var sourceGenOptions = new JsonSerializerOptions
            {
                TypeInfoResolver = AppJsonSerializerContext.Default
            };
            var jsonString = JsonSerializer.Serialize(model, sourceGenOptions);
            return Ok(jsonString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Post")]
        public IActionResult Post()
        {
            var model = new HomeModel
            {
                Id = 1,
                Title = "Hello, World",
                DueBy = DateOnly.FromDateTime(DateTime.Now),
                IsComplete = false
            };
            return Ok("Hello, World");
        }
    }

    
    public class HomeModel : BaseModel
    {       
        public string Title { get; set; }
        public DateOnly DueBy { get; set; }
        public bool IsComplete { get; set; }
    }

    public class BaseModel
    {
        public int Id { get; set; }

        public string? Message { get; set; }
    }
}
