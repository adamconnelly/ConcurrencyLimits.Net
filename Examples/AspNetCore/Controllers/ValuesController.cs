namespace AspNetCore.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// An api controller used for testing the limits.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Waits for a delay (to make it easy to breach the limit) and then returns some values.
        /// </summary>
        /// <returns>Some values.</returns>
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            await Task.Delay(4000);

            return new string[] { "value1", "value2" };
        }
    }
}
